namespace Mystrose.Network.Handlers.JSON;

public static class JHDrop
{

    #region Fields
    private static readonly Dictionary<string, Action<JSONMessage>> _handlers = new()
    {
        ["dropItem"] = HandleItemDrop,
        ["referralReward"] = HandleItemDrop,
        ["getDrop"] = HandleGetDrop,
        ["addItems"] = HandleAddItems,
        ["powerGem"] = HandleAddItems,
        ["forceAddItem"] = HandleAddItems,
        ["Wheel"] = HandleWheel
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
    public static void HandleItemDrop(JSONMessage message)
    {
        JsonObject items = message.DataObject["items"].Deserialize<JsonObject>()!;
        foreach (KeyValuePair<string, JsonNode> itemInfo in items)
        {
            BaseItem item = itemInfo.Value.Deserialize<BaseItem>()!;
            BaseItem? existingItem = message.World.Drops.Find(
                (drop) =>
                {
                    return drop.ID == item.ID;
                });

            if (existingItem is null)
            {
                message.World.Drops.Add(item);

                SVCScriptManager.InvokeTrigger(message.Identifier.Codename, item);
            }
            else
            {
                existingItem.Quantity += item.Quantity;

                SVCScriptManager.InvokeTrigger(message.Identifier.Codename, existingItem);
            }
        }
    }

    public static void HandleGetDrop(JSONMessage message)
    {
        bool isSuccess = message.DataObject["bSuccess"].Deserialize<int>() == 1;

        if (!isSuccess)
        {
            return;
        }

        int id = message.DataObject["ItemID"].Deserialize<int>();
        BaseItem? item = message.World.Drops.Find(
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

        message.World.Drops.Remove(item);
        if (isBanked)
        {
            if (message.World.Inventories[InventoryType.Bank].TryGetValue(inventoryItem.ID, out InventoryItem? bankItem))
            {
                bankItem.Quantity += inventoryItem.Quantity;

                SVCScriptManager.InvokeTrigger(message.Identifier.Codename, bankItem);
            }
            else
            {
                inventoryItem.InventoryType = InventoryType.Bank;
                message.World.Inventories[InventoryType.Bank].Add(inventoryItem.ID, inventoryItem);

                SVCScriptManager.InvokeTrigger(message.Identifier.Codename, inventoryItem);
            }
        }
        else
        {
            if (message.World.Inventories[InventoryType.Base].TryGetValue(inventoryItem.ID, out InventoryItem? baseItem))
            {
                baseItem.Quantity += inventoryItem.Quantity;

                SVCScriptManager.InvokeTrigger(message.Identifier.Codename, baseItem);
            }
            else
            {
                inventoryItem.InventoryType = InventoryType.Base;
                message.World.Inventories[InventoryType.Base].Add(inventoryItem.ID, inventoryItem);

                SVCScriptManager.InvokeTrigger(message.Identifier.Codename, inventoryItem);
            }
        }
    }

    public static void HandleAddItems(JSONMessage message)
    {
        JsonObject items = message.DataObject["items"].Deserialize<JsonObject>()!;
        foreach (KeyValuePair<string, JsonNode> itemInfo in items)
        {
            InventoryItem item = itemInfo.Value.Deserialize<InventoryItem>()!;

            if (item.IsTemporary)
            {
                if (message.World.Inventories[InventoryType.Temp].TryGetValue(item.ID, out InventoryItem? tempItem))
                {
                    tempItem.Quantity += item.Quantity;

                    SVCScriptManager.InvokeTrigger(message.Identifier.Codename, tempItem);
                }
                else
                {
                    item.InventoryType = InventoryType.Temp;
                    message.World.Inventories[InventoryType.Temp].Add(item.ID, item);

                    SVCScriptManager.InvokeTrigger(message.Identifier.Codename, item);
                }
            }
            else
            {
                if (message.World.Inventories[InventoryType.Base].TryGetValue(item.ID, out InventoryItem? baseItem))
                {
                    baseItem.Quantity += item.Quantity;

                    SVCScriptManager.InvokeTrigger(message.Identifier.Codename, baseItem);
                }
                else
                {
                    item.InventoryType = InventoryType.Base;
                    message.World.Inventories[InventoryType.Base].Add(item.ID, item);

                    SVCScriptManager.InvokeTrigger(message.Identifier.Codename, item);
                }
            }
        }
    }

    public static void HandleWheel(JSONMessage message)
    {
        if (message.DataObject.ContainsKey("Item"))
        {
            InventoryItem item = message.DataObject["Item"].Deserialize<InventoryItem>()!;
            message.World.Inventories[InventoryType.Base].Add(item.ID, item);

            SVCScriptManager.InvokeTrigger(message.Identifier.Codename, item);
        }

        JsonObject drops = message.DataObject["dropItems"].Deserialize<JsonObject>()!;
        foreach (KeyValuePair<string, JsonNode> dropItemInfo in drops)
        {
            InventoryItem dropItem = dropItemInfo.Value.Deserialize<InventoryItem>()!;
            dropItem.InventoryType = InventoryType.Base;

            message.World.Inventories[InventoryType.Base].Add(dropItem.ID, dropItem);

            SVCScriptManager.InvokeTrigger(message.Identifier.Codename, dropItem);
        }
    }
    #endregion

}
