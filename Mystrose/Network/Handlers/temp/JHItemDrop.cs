namespace Mystrose.Network.Handlers.temp;

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
    public void Handle(JSONMessage message)
    {
        switch (message.Command)
        {
            case "dropItem":
            case "referralReward":
                HandleDrop(message);
                break;
            case "getDrop":
                HandleGet(message);
                break;
            case "addItems":
            case "powerGem":
            case "forceAddItem":
                //HandleAdd(host, message.DataObject);
                break;
            case "Wheel":
                HandleWheel(message);
                break;
        }
    }
    #endregion

    #region Methods: Drop
    private void HandleDrop(JSONMessage message)
    {
        World world = message.World;
        JsonObject obj = message.DataObject;

        foreach (KeyValuePair<string, JsonNode> itemObj in obj["items"].Deserialize<JsonObject>())
        {
            int id = int.Parse(itemObj.Key);

            BaseItem? item = itemObj.Value.Deserialize<BaseItem>();
            item.ID = id;

            BaseItem? existingItem = world.Environment.Drops.Find(
                (inventoryItem) =>
                {
                    return inventoryItem.ID == item.ID;
                });

            if (existingItem is null)
            {
                world.Environment.Drops.Add(item);

                SVCScriptManager.InvokeTrigger(message.Identifier.Codename, item);
            }
            else
            {
                existingItem.Quantity += item.Quantity;

                SVCScriptManager.InvokeTrigger(message.Identifier.Codename, existingItem);
            }
        }
    }
    #endregion

    #region Methods: Get
    private void HandleGet(JSONMessage message)
    {
        World world = message.World;
        JsonObject obj = message.DataObject;

        bool isSuccess = obj["bSuccess"].Deserialize<int>() == 1;

        if (!isSuccess)
        {
            return;
        }

        int id = obj["ItemID"].Deserialize<int>();
        int charItemId = obj["CharItemID"].Deserialize<int>();
        int qty = obj["iQtyNow"].Deserialize<int>();

        bool isBanked = obj["bBank"].Deserialize<int>() == 1;

        BaseItem? item = world.Environment.Drops.Find(
            (inventoryItem) =>
            {
                return inventoryItem.ID == id;
            });

        world.Environment.Drops.Remove(item);

        // WIP
    }
    #endregion

    #region Methods: Add
    private void HandleAdd(JSONMessage message)
    {
        World world = message.World;
        JsonObject obj = message.DataObject;

        foreach (KeyValuePair<string, JsonNode> itemObj in obj["items"].Deserialize<JsonObject>())
        {
            BaseItem? item = itemObj.Value.Deserialize<BaseItem>();

            InventoryItem? invItem = world.Inventories[InventoryType.Base][item.ID];

            // WIP
        }
    }
    #endregion

    #region Methods: Wheel
    private void HandleWheel(JSONMessage message)
    {
        World world = message.World;
        JsonObject obj = message.DataObject;

        foreach (KeyValuePair<string, JsonNode> itemObj in obj["dropItems"].Deserialize<JsonObject>())
        {
            BaseItem? item = itemObj.Value.Deserialize<BaseItem>();

            InventoryItem? invItem = world.Inventories[InventoryType.Base][item.ID];

            // WIP
        }
    }
    #endregion

}
