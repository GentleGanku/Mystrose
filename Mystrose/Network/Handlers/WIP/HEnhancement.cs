using Mystrose.Controls.Main;
using Mystrose.Network.Messages;
using Mystrose.Network.Messages.Interfaces;

namespace Mystrose.Network.Handlers.WIP;

public class HEnhanceItem : IJSONMessageHandler
{

    #region Commands
    public string[] HandledCommands
    {
        get =>
        [
            "enhanceItemShop",
            "enhanceItemLocal"
        ];
    }
    #endregion

    #region Methods: Handler
    public void Handle(GameHost host, JSONMessage message)
    {
        if (message.DataObject["iCost"] is not null)
        {
            host.World.Master.Gold -= message.DataObject["iCost"].GetValue<int>();
        }

        //if (message.DataObject["iCost"] != null)
        //{
        //    Player.Self.Extended.Gold = Player.Self.Extended.Gold - (int)message.DataObject["iCost"];
        //}
        //Player.Self.Extended.Inventory = Flash.Call<List<InventoryItem>>("GetInventoryItems", new string[0]);
    }
    #endregion

}
