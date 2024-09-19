namespace Mystrose.Network.Handlers.temp;

public class HBuySlots : IJSONMessageHandler
{

    #region Commands
    public string[] HandledCommands
    {
        get =>
        [
            "buyBagSlots",
            "buyBankSlots",
            "buyHouseSlots"
        ];
    }
    #endregion

    #region Methods: Handler
    public void Handle(JSONMessage message)
    {
        //bool isSuccess = message.DataObject["bitSuccess"].GetValue<bool>();

        //if (!isSuccess)
        //{
        //    return;
        //}

        //int slots = message.DataObject["iSlots"].GetValue<int>();

        //host.World.Master.AdventureCoins -= slots * 200;

        //switch (message.Command)
        //{
        //    case "buyBagSlots":
        //        host.World.Master.InventorySlots += slots;
        //        break;
        //    case "buyBankSlots":
        //        host.World.Master.BankSlots += slots;
        //        break;
        //    case "buyHouseSlots":
        //        host.World.Master.HouseSlots += slots;
        //        break;
        //}
    }
    #endregion

}
