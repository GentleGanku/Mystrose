namespace Mystrose.Network.Handlers.JSON;

public class JHInventory() : MessageHandler<JSONMessage>(new()
{
    ["loadInventoryBig"] = HandleInventoryLoad,
    ["turnIn"] = HandleTurnIn
})
{

    #region Methods: Handlers
    private static void HandleInventoryLoad(JSONMessage message)
    {
        List<InventoryItem> inventoryItems = message.DataObject["items"].Deserialize<List<InventoryItem>>()!;
        List<InventoryItem> houseInventoryItems = message.DataObject["hitems"].Deserialize<List<InventoryItem>>()!;
        List<Faction> factions = message.DataObject["factions"].Deserialize<List<Faction>>()!;

        message.HostWorld.Inventories[InventoryType.Base].AddRange(inventoryItems);
        message.HostWorld.Inventories[InventoryType.House].AddRange(houseInventoryItems);
        message.HostWorld.Factions.AddRange(factions);
    }

    private static void HandleTurnIn(JSONMessage message)
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

            InventoryItem? inventoryItem = message.HostWorld.Inventories[InventoryType.Base][itemId];
            inventoryItem ??= message.HostWorld.Inventories[InventoryType.Temp][itemId];
            inventoryItem ??= message.HostWorld.Inventories[InventoryType.House][itemId];

            if (inventoryItem is null)
            {
                continue;
            }

            inventoryItem.Quantity -= qty;

            if (inventoryItem.Quantity <= 0)
            {
                inventoryItem.Quantity = 0;
                message.HostWorld.Inventories[inventoryItem.InventoryType].Remove(inventoryItem.ID);
            }

            MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, inventoryItem);
        }
    }
    #endregion

}
