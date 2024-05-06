using Mystrose.Controls.Main;
using Mystrose.GameModels.Base;
using Mystrose.GameModels.General;
using Mystrose.Network.Messages;
using Mystrose.Network.Messages.Interfaces;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Mystrose.Network.Handlers.JSON;

public class HWearItem : IJSONMessageHandler
{

    #region Commands
    public string[] HandledCommands
    {
        get =>
        [
            "wearItem",
            "unwearItem"
        ];
    }
    #endregion

    #region Methods: Handler
    public void Handle(GameHost host, JSONMessage message)
    {
        switch (message.Command)
        {
            case "wearItem":
                HandleWear(host, message.DataObject);
                break;
            case "unwearItem":
                HandleUnwear(host, message.DataObject);
                break;
        }
    }
    #endregion

    #region Methods: Wear
    private void HandleWear(GameHost host, JsonObject obj)
    {
        int id = obj["uid"].GetValue<int>();
        string type = obj["sES"].GetValue<string>();
        BaseItem item = obj.GetValue<JsonObject>().Deserialize<BaseItem>();

        Avatar? avatar = host.World.Area.Players.Find(
            (avt) =>
            {
                return avt.EntityID == id;
            });

        if (avatar == null)
        {
            return;
        }

        avatar.CosmeticEquipments[type] = item;

        if (host.World.Master.EntityID == id)
        {
            bool isSuccess = obj["success"].GetValue<bool>();

            if (!isSuccess)
            {
                return;
            }

            host.World.Master.CosmeticEquipments[type] = item;
        }
    }
    #endregion

    #region Methods: Unwear
    private void HandleUnwear(GameHost host, JsonObject obj)
    {
        int id = obj["uid"].GetValue<int>();
        string type = obj["sES"].GetValue<string>();

        Avatar? avatar = host.World.Area.Players.Find(
            (avt) =>
            {
                return avt.EntityID == id;
            });

        if (avatar == null)
        {
            return;
        }

        avatar.CosmeticEquipments[type] = null;

        if (host.World.Master.EntityID == id)
        {
            bool isSuccess = obj["success"].GetValue<bool>();

            if (!isSuccess)
            {
                return;
            }

            host.World.Master.CosmeticEquipments[type] = null;
        }
    }
    #endregion

}
