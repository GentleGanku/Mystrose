namespace Mystrose.Network.Handlers.JSON;

public static class JHBankInterface
{

    #region Fields
    private static readonly Dictionary<string, Action<JSONMessage>> _handlers = new()
    {
        ["loadBank"] = HandleBankLoad,
        ["bankFromInv"] = HandleBankFromInv,
        ["bankToInv"] = HandleBankToInv,
        ["bankSwapInv"] = HandleBankSwapInv
    };
    #endregion

    #region Methods: Invoker
    public static void Invoke(JSONMessage message)
    {
        if (!_handlers.TryGetValue(message.Command, out var handler))
        {
            return;
        }

        try
        {
            handler.Invoke(message);
        }
        catch (Exception ex)
        {
            SVCLogger.LogOnException($"({nameof(message)} - {message.Command}) {ex.ToString()}");
        }
    }
    #endregion

    #region Handlers
    public static void HandleBankLoad(JSONMessage message)
    {
        bool isSuccess = message.DataObject["bitSuccess"]!.GetValue<int>() == 1;

        if (!isSuccess)
        {
            return;
        }

        List<InventoryItem> items = message.DataObject["items"].Deserialize<List<InventoryItem>>()!;
        message.World.Inventories[InventoryType.Bank].Clear();
        message.World.Inventories[InventoryType.Bank].AddRange(items);
    }

    public static void HandleBankFromInv(JSONMessage message)
    {
        bool isSuccess = message.DataObject["bSuccess"]!.GetValue<int>() == 1;

        if (!isSuccess)
        {
            return;
        }

        int id = message.DataObject["ItemID"]!.GetValue<int>();
        InventoryItem? inventoryItem = message.World.Inventories[InventoryType.Base][id];

        if (inventoryItem is null)
        {
            return;
        }

        message.World.Inventories[InventoryType.Base].Remove(id);

        inventoryItem.InventoryType = InventoryType.Bank;
        message.World.Inventories[InventoryType.Bank].Add(id, inventoryItem);

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, inventoryItem);
    }

    public static void HandleBankToInv(JSONMessage message)
    {
        int id = message.DataObject["ItemID"]!.GetValue<int>();
        InventoryItem? bankItem = message.World.Inventories[InventoryType.Bank][id];

        if (bankItem is null)
        {
            return;
        }

        message.World.Inventories[InventoryType.Bank].Remove(id);

        bankItem.InventoryType = InventoryType.Base;
        message.World.Inventories[InventoryType.Base].Add(id, bankItem);

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, bankItem);
    }

    public static void HandleBankSwapInv(JSONMessage message)
    {
        int bankItemID = message.DataObject["bankItemID"]!.GetValue<int>();
        int baseItemID = message.DataObject["invItemID"]!.GetValue<int>();

        InventoryItem? bankItem = message.World.Inventories[InventoryType.Bank][bankItemID];
        InventoryItem? baseItem = message.World.Inventories[InventoryType.Base][baseItemID];

        if (bankItem is null || baseItem is null)
        {
            return;
        }

        message.World.Inventories[InventoryType.Bank].Remove(bankItemID);
        message.World.Inventories[InventoryType.Base].Remove(baseItemID);

        bankItem.InventoryType = InventoryType.Base;
        baseItem.InventoryType = InventoryType.Bank;

        message.World.Inventories[InventoryType.Bank].Add(baseItemID, baseItem);
        message.World.Inventories[InventoryType.Base].Add(bankItemID, bankItem);

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, bankItem);
        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, baseItem);
    }
    #endregion

}
