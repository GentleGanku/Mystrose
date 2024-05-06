using Mystrose.Controls.Main;
using Mystrose.GameModels.General;
using Mystrose.GameModels.Master;
using Mystrose.Network.Messages;
using Mystrose.Network.Messages.Interfaces;
using Mystrose.ScriptMachine.Enumerations;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Mystrose.Network.Handlers.JSON;

public class JHEntityData : IJSONMessageHandler
{

    #region Commands
    public string[] HandledCommands
    {
        get =>
        [
            "initUserData",
            "initUserDatas"
        ];
    }
    #endregion

    #region Methods: Handler
    public void Handle(GameHost host, JSONMessage message)
    {
        switch (message.Command)
        {
            case "initUserData":
                HandleData(host, message.DataObject);
                break;
            case "initUserDatas":
                HandleDatas(host, message.DataObject);
                break;
        }
    }
    #endregion

    #region Methods: User Data
    private void HandleData(GameHost host, JsonObject obj)
    {
        Avatar? avatar = host.World.Area.Players.Find(
            (avt) =>
            {
                return avt.EntityID == obj["uid"].Deserialize<int>();
            });

        if (avatar is null)
        {
            avatar = obj["data"].Deserialize<Avatar>();
            host.World.Area.Players.Add(avatar);
        }
        else
        {
            avatar.SetProperties(obj["data"].Deserialize<JsonObject>());
        }

        host.ScriptManager.InvokeTriggerSystems(ScriptTriggerType.Player, avatar);

        if (avatar.EntityID == host.World.Master.UserID)
        {
            int masterId = host.World.Master.UserID;
            string masterName = host.World.Master.Name;

            host.World.Master = obj["data"].Deserialize<MainAvatar>();
            host.World.Master.UserID = masterId;
            host.World.Master.Name = masterName;
        }
    }

    private void HandleDatas(GameHost host, JsonObject obj)
    {
        foreach (JsonObject userObj in (JsonArray)obj["a"])
        {
            HandleData(host, userObj);
        }
    }
    #endregion

}
