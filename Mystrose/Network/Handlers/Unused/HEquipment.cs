using Mystrose.Controls.Main;
using Mystrose.GameModels.Base;
using Mystrose.GameModels.General;
using Mystrose.Network.Messages;
using Mystrose.Network.Messages.Interfaces;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Mystrose.Network.Handlers.JSON;

public class HEquipment : IJSONMessageHandler
{

    #region Commands
    public string[] HandledCommands
    {
        get =>
        [
            "equipItem",
            "unequipItem"
        ];
    }
    #endregion

    #region Methods: Handler
    public void Handle(GameHost host, JSONMessage message)
    {
        switch (message.Command)
        {
            case "equipItem":
                HandleEquip(host, message.DataObject);
                break;
            case "unequipItem":
                HandleUnequip(host, message.DataObject);
                break;
        }
    }
    #endregion

    #region Methods: Equip
    private void HandleEquip(GameHost host, JsonObject obj)
    {
        int id = obj["uid"].GetValue<int>();
        string type = obj["strES"].GetValue<string>();
        BaseItem item = obj.GetValue<JsonObject>().Deserialize<BaseItem>();

        if (host.World.Master.EntityID == id)
        {
            host.World.Master.Equipments[type] = item;
        }

        Avatar? avatar = host.World.Area.Players.Find(
            (avt) =>
            {
                return avt.EntityID == id;
            });

        if (avatar == null)
        {
            return;
        }

        avatar.Equipments[type] = item;
    }
    #endregion

    #region Methods: Unequip
    private void HandleUnequip(GameHost host, JsonObject obj)
    {
        int id = obj["uid"].GetValue<int>();
        string type = obj["strES"].GetValue<string>();

        Avatar? avatar = host.World.Area.Players.Find(
            (avt) =>
            {
                return avt.EntityID == id;
            });

        if (avatar == null)
        {
            return;
        }

        if (avatar.EntityID == host.World.Master.EntityID)
        {
            host.World.Master.Equipments[type] = null;
        }

        avatar.Equipments[type] = null;
    }
    #endregion

}
