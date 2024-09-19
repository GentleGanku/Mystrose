namespace Mystrose.Network.Handlers.temp;

public class HItem : IJSONMessageHandler
{

    #region Commands
    public string[] HandledCommands
    {
        get =>
        [
            "buyItem",
            "sellItem",
            "removeItem"
        ];
    }
    #endregion

    #region Methods: Handler
    public async void Handle(JSONMessage message)
    {
        switch (message.Command)
        {
            case "buyItem":
                HandleBuy(message);
                break;
            case "sellItem":
                break;
            case "removeItem":
                break;
        }
    }
    #endregion

    // {"t":"xt","b":{"r":-1,"o":{"ItemID":"35599","cmd":"buyItem","bitSuccess":1,"bBank":0,"CharItemID":6.17963437E8,"iQty":1}}}

    // {"t":"xt","b":{"r":-1,"o":{"iQtyNow":0,"cmd":"sellItem","intAmount":1250,"CharItemID":6.17963437E8,"bCoins":0,"iQty":1}}}

    // {"t":"xt","b":{"r":-1,"o":{"iQtyNow":0,"cmd":"removeItem","bSuccess":1,"bBank":0,"CharItemID":6.17963501E8,"iQty":1}}}

    #region Methods: Buy Item
    private void HandleBuy(JSONMessage message)
    {
        //bool isSuccess = buyObject["bitSuccess"].GetValue<int>() == 1;

        //if (!isSuccess)
        //{
        //    return;
        //}

        //int id = buyObject["ItemID"].GetValue<int>();

        //ShopItem? shopItem = host.World.Shop.Items.Find(
        //    (item) =>
        //    {
        //        return item.ID == id;
        //    });

        //if (shopItem == null)
        //{
        //    return;
        //}

        //int qty = buyObject["iQty"].GetValue<int>();
        //int charItemId = buyObject["CharItemID"].GetValue<int>();

        //if (shopItem.IsCoinTagged)
        //{
        //    host.World.Master.AdventureCoins -= shopItem.Cost * qty;
        //}
        //else
        //{
        //    host.World.Master.Gold -= shopItem.Cost * qty;
        //}

        //InventoryItem? newItem = JsonSerializer.Deserialize<InventoryItem>(JsonObject.Parse(shopItem.ToString()));
        //newItem.Quantity = qty;
        //newItem.CharItemID = charItemId;

        //InventoryItem? inventoryItem =  host.MainAvatar.Inventory.Find(
        //    (item) =>
        //    {
        //        return item.ID == id;
        //    });

        //if (inventoryItem == null)
        //{
        //    host.MainAvatar.Inventory.Add(newItem);
        //}
        //else
        //{
        //    inventoryItem.Quantity += qty;
        //}
    }
    #endregion

}
