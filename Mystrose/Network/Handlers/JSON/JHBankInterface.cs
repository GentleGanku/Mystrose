using Mystrose.DataRecords.Game;

namespace Mystrose.Network.Handlers.JSON;

public class JHBankInterface() : MessageHandler<JSONMessage>(new()
{
    ["loadBank"] = HandleBankLoad,
    ["bankFromInv"] = HandleBankFromInv,
    ["bankToInv"] = HandleBankToInv,
    ["bankSwapInv"] = HandleBankSwapInv
})
{

    #region Methods: Handlers
    private static void HandleBankLoad(JSONMessage message)
    {
        bool isSuccess = message.DataObject["bitSuccess"]!.GetValue<int>() == 1;

        if (!isSuccess)
        {
            return;
        }

        List<InventoryItem> items = message.DataObject["items"].Deserialize<List<InventoryItem>>()!;
        message.HostWorld.Inventories[InventoryType.Bank].Clear();
        message.HostWorld.Inventories[InventoryType.Bank].AddRange(items);
    }

    private static void HandleBankFromInv(JSONMessage message)
    {
        bool isSuccess = message.DataObject["bSuccess"]!.GetValue<int>() == 1;

        if (!isSuccess)
        {
            return;
        }

        int id = message.DataObject["ItemID"]!.GetValue<int>();
        InventoryItem? inventoryItem = message.HostWorld.Inventories[InventoryType.Base][id];

        if (inventoryItem is null)
        {
            return;
        }

        message.HostWorld.Inventories[InventoryType.Base].Remove(id);

        inventoryItem.InventoryType = InventoryType.Bank;
        message.HostWorld.Inventories[InventoryType.Bank].Add(id, inventoryItem);

        MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, inventoryItem);
    }

    private static void HandleBankToInv(JSONMessage message)
    {
        int id = message.DataObject["ItemID"]!.GetValue<int>();
        InventoryItem? bankItem = message.HostWorld.Inventories[InventoryType.Bank][id];

        if (bankItem is null)
        {
            return;
        }

        message.HostWorld.Inventories[InventoryType.Bank].Remove(id);

        bankItem.InventoryType = InventoryType.Base;
        message.HostWorld.Inventories[InventoryType.Base].Add(id, bankItem);

        MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, bankItem);
    }

    private static void HandleBankSwapInv(JSONMessage message)
    {
        int bankItemId = message.DataObject["bankItemID"]!.GetValue<int>();
        int baseItemId = message.DataObject["invItemID"]!.GetValue<int>();

        InventoryItem? bankItem = message.HostWorld.Inventories[InventoryType.Bank][bankItemId];
        InventoryItem? baseItem = message.HostWorld.Inventories[InventoryType.Base][baseItemId];

        if (bankItem is null || baseItem is null)
        {
            return;
        }

        message.HostWorld.Inventories[InventoryType.Bank].Remove(bankItemId);
        message.HostWorld.Inventories[InventoryType.Base].Remove(baseItemId);

        bankItem.InventoryType = InventoryType.Base;
        baseItem.InventoryType = InventoryType.Bank;

        message.HostWorld.Inventories[InventoryType.Bank].Add(baseItemId, baseItem);
        message.HostWorld.Inventories[InventoryType.Base].Add(bankItemId, bankItem);

        MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, bankItem);
        MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, baseItem);
    }
    #endregion

}
