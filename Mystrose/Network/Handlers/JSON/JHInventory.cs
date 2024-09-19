namespace Mystrose.Network.Handlers.JSON;

public static class JHInventory
{

    #region Fields
    private static readonly Dictionary<string, Action<JSONMessage>> _handlers = new()
    {
        ["loadInventoryBig"] = HandleInventoryLoad,
        ["turnIn"] = HandleTurnIn
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
    public static void HandleInventoryLoad(JSONMessage message)
    {
        List<InventoryItem> inventoryItems = message.DataObject["items"].Deserialize<List<InventoryItem>>()!;
        List<InventoryItem> houseInventoryItems = message.DataObject["hitems"].Deserialize<List<InventoryItem>>()!;
        List<Faction> factions = message.DataObject["factions"].Deserialize<List<Faction>>()!;

        message.World.Inventories[InventoryType.Base].AddRange(inventoryItems);
        message.World.Inventories[InventoryType.House].AddRange(houseInventoryItems);
        message.World.Factions = new(factions);
    }

    public static void HandleTurnIn(JSONMessage message)
    {
        if (!message.DataObject.ContainsKey("sItems"))
        {
            return;
        }

        string itemsString = message.DataObject["sItems"]!.GetValue<string>()!;
        string[] items = itemsString.Split(",");

        foreach (string itemInfo in items)
        {
            string[] itemData = itemInfo.Split(":");

            int itemId = int.Parse(itemData[0]);
            int qty = int.Parse(itemData[1]);

            InventoryItem? inventoryItem = message.World.Inventories[InventoryType.Base][itemId];
            inventoryItem ??= message.World.Inventories[InventoryType.Temp][itemId];
            inventoryItem ??= message.World.Inventories[InventoryType.House][itemId];

            if (inventoryItem is null)
            {
                continue;
            }

            inventoryItem.Quantity -= qty;

            if (inventoryItem.Quantity <= 0)
            {
                inventoryItem.Quantity = 0;
                message.World.Inventories[inventoryItem.InventoryType].Remove(inventoryItem.ID);
            }

            SVCScriptManager.InvokeTrigger(message.Identifier.Codename, inventoryItem);
        }
    }
    #endregion

}
