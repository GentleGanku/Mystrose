namespace Mystrose.Network.Handlers.JSON;

public class JHDrop() : MessageHandler<JSONMessage>(new()
{
    ["dropItem"] = HandleItemDrop,
    ["referralReward"] = HandleItemDrop,
    ["getDrop"] = HandleGetDrop,
    ["addItems"] = HandleAddItems,
    ["powerGem"] = HandleAddItems,
    ["forceAddItem"] = HandleAddItems,
    ["Wheel"] = HandleWheel
})
{

    #region Methods: Handlers
    private static void HandleItemDrop(JSONMessage message)
    {
        JsonObject items = message.DataObject["items"].Deserialize<JsonObject>()!;
        foreach (KeyValuePair<string, JsonNode> itemInfo in items)
        {
            BaseItem item = itemInfo.Value.Deserialize<BaseItem>()!;
            BaseItem? existingItem = message.HostWorld.Drops.Find(
                (drop) =>
                {
                    return drop.ID == item.ID;
                });

            if (existingItem is null)
            {
                message.HostWorld.Drops.Add(item);

                MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, item);
            }
            else
            {
                existingItem.Quantity += item.Quantity;

                MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, existingItem);
            }
        }
    }

    private static void HandleGetDrop(JSONMessage message)
    {
        bool isSuccess = message.DataObject["bSuccess"].Deserialize<int>() == 1;

        if (!isSuccess)
        {
            return;
        }

        int id = message.DataObject["ItemID"].Deserialize<int>();
        BaseItem? item = message.HostWorld.Drops.Find(
            (inventoryItem) =>
            {
                return inventoryItem.ID == id;
            });

        if (item is null)
        {
            return;
        }

        int qty = message.DataObject["iQty"].Deserialize<int>();
        bool isBanked = message.DataObject["bBank"].Deserialize<int>() == 1;

        InventoryItem inventoryItem = (InventoryItem)item;
        inventoryItem.Quantity = qty;

        message.HostWorld.Drops.Remove(item);
        if (isBanked)
        {
            if (message.HostWorld.Inventories[InventoryType.Bank].TryGetValue(inventoryItem.ID, out InventoryItem? bankItem))
            {
                bankItem.Quantity += inventoryItem.Quantity;

                MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, bankItem);
            }
            else
            {
                inventoryItem.InventoryType = InventoryType.Bank;
                message.HostWorld.Inventories[InventoryType.Bank].Add(inventoryItem.ID, inventoryItem);

                MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, inventoryItem);
            }
        }
        else
        {
            if (message.HostWorld.Inventories[InventoryType.Base].TryGetValue(inventoryItem.ID, out InventoryItem? baseItem))
            {
                baseItem.Quantity += inventoryItem.Quantity;

                MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, baseItem);
            }
            else
            {
                inventoryItem.InventoryType = InventoryType.Base;
                message.HostWorld.Inventories[InventoryType.Base].Add(inventoryItem.ID, inventoryItem);

                MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, inventoryItem);
            }
        }
    }

    private static void HandleAddItems(JSONMessage message)
    {
        JsonObject items = message.DataObject["items"].Deserialize<JsonObject>()!;
        foreach (KeyValuePair<string, JsonNode> itemInfo in items)
        {
            InventoryItem item = itemInfo.Value.Deserialize<InventoryItem>()!;

            if (item.IsTemporary)
            {
                if (message.HostWorld.Inventories[InventoryType.Temp].TryGetValue(item.ID, out InventoryItem? tempItem))
                {
                    tempItem.Quantity += item.Quantity;

                    MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, tempItem);
                }
                else
                {
                    item.InventoryType = InventoryType.Temp;
                    message.HostWorld.Inventories[InventoryType.Temp].Add(item.ID, item);

                    MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, item);
                }
            }
            else
            {
                if (message.HostWorld.Inventories[InventoryType.Base].TryGetValue(item.ID, out InventoryItem? baseItem))
                {
                    baseItem.Quantity += item.Quantity;

                    MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, baseItem);
                }
                else
                {
                    item.InventoryType = InventoryType.Base;
                    message.HostWorld.Inventories[InventoryType.Base].Add(item.ID, item);

                    MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, item);
                }
            }
        }
    }

    private static void HandleWheel(JSONMessage message)
    {
        if (message.DataObject.ContainsKey("Item"))
        {
            InventoryItem item = message.DataObject["Item"].Deserialize<InventoryItem>()!;
            message.HostWorld.Inventories[InventoryType.Base].Add(item.ID, item);

            MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, item);
        }

        JsonObject drops = message.DataObject["dropItems"].Deserialize<JsonObject>()!;
        foreach (KeyValuePair<string, JsonNode> dropItemInfo in drops)
        {
            InventoryItem dropItem = dropItemInfo.Value.Deserialize<InventoryItem>()!;
            dropItem.InventoryType = InventoryType.Base;

            message.HostWorld.Inventories[InventoryType.Base].Add(dropItem.ID, dropItem);

            MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, dropItem);
        }
    }
    #endregion

}
