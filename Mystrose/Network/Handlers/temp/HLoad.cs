namespace Mystrose.Network.Handlers.temp;

public class HLoad : IJSONMessageHandler
{

    #region Commands
    public string[] HandledCommands
    {
        get =>
        [
            "loadBank",
            "loadShop",
            "loadEnhShop"
        ];
    }
    #endregion

    #region Methods: Handler
    public void Handle(JSONMessage message)
    {
        //switch (message.Command)
        //{
        //    case "loadBank":
        //        bool isSuccess = message.DataObject["bitSuccess"].GetValue<int>() == 1;

        //        if (!isSuccess)
        //        {
        //            return;
        //        }

        //        host.World.BankInventory.AddRange(message.DataObject["items"].Deserialize<List<InventoryItem>>());
        //        break;
        //    case "loadShop":
        //        Shop? shop = message.DataObject["shopinfo"].Deserialize<Shop>();
        //        host.World.Shop = shop;
        //        break;
        //    case "loadEnhShop":
        //        Shop? enhShop = message.DataObject["shopinfo"].Deserialize<Shop>();
        //        host.World.EnhancementShop = enhShop;
        //        break;
        //}
    }
    #endregion

}
