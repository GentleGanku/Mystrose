namespace Mystrose.Network.Handlers.JSON;

public class JHItemDrop : IJSONMessageHandler
{

    #region Commands
    public string[] HandledCommands
    {
        get =>
        [
            "dropItem",
            "referralReward",
            "getDrop",
            "addItems",
            "Wheel",
            "powerGem",
            "forceAddItem"
        ];
    }
    #endregion

    #region Methods: Handler
    public void Handle(GameHost host, JSONMessage message)
    {
        switch (message.Command)
        {
            case "dropItem":
            case "referralReward":
                HandleDrop(host, message.DataObject);
                break;
            case "getDrop":
                HandleGet(host, message.DataObject);
                break;
            case "addItems":
            case "powerGem":
            case "forceAddItem":
                //HandleAdd(host, message.DataObject);
                break;
            case "Wheel":
                HandleWheel(host, message.DataObject);
                break;
        }
    }
    #endregion

    #region Methods: Drop
    private void HandleDrop(GameHost host, JsonObject obj)
    {
        foreach (KeyValuePair<string, JsonNode> itemObj in obj["items"].Deserialize<JsonObject>())
        {
            int id = int.Parse(itemObj.Key);

            BaseItem? item = itemObj.Value.Deserialize<BaseItem>();
            item.ID = id;

            BaseItem? existingItem = host.World.Drops.Find(
                (inventoryItem) =>
                {
                    return inventoryItem.ID == item.ID;
                });

            if (existingItem is null)
            {
                host.World.Drops.Add(item);

                host.ScriptManager.InvokeTriggerSystems(ScriptTriggerType.Drop, item);
            }
            else
            {
                existingItem.Quantity += item.Quantity;

                host.ScriptManager.InvokeTriggerSystems(ScriptTriggerType.Drop, existingItem);
            }
        }
    }
    #endregion

    #region Methods: Get
    private void HandleGet(GameHost host, JsonObject obj)
    {
        bool isSuccess = obj["bSuccess"].Deserialize<int>() == 1;

        if (!isSuccess)
        {
            return;
        }

        int id = obj["ItemID"].Deserialize<int>();
        int charItemId = obj["CharItemID"].Deserialize<int>();
        int qty = obj["iQtyNow"].Deserialize<int>();

        bool isBanked = obj["bBank"].Deserialize<int>() == 1;

        BaseItem? item = host.World.Drops.Find(
            (inventoryItem) =>
            {
                return inventoryItem.ID == id;
            });

        host.World.Drops.Remove(item);
        
        // WIP
    }
    #endregion

    #region Methods: Add
    private void HandleAdd(GameHost host, JsonObject obj)
    {
        foreach (KeyValuePair<string, JsonNode> itemObj in obj["items"].Deserialize<JsonObject>())
        {
            BaseItem? item = itemObj.Value.Deserialize<BaseItem>();

            InventoryItem? invItem = host.World.Inventory[item.ID];

            // WIP
        }
    }
    #endregion

    #region Methods: Wheel
    private void HandleWheel(GameHost host, JsonObject obj)
    {
        foreach (KeyValuePair<string, JsonNode> itemObj in obj["dropItems"].Deserialize<JsonObject>())
        {
            BaseItem? item = itemObj.Value.Deserialize<BaseItem>();

            InventoryItem? invItem = host.World.Inventory[item.ID];

            // WIP
        }
    }
    #endregion

}
