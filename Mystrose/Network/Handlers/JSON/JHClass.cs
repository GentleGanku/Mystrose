using Mystrose.Controls.Main;
using Mystrose.GameModels.General;
using Mystrose.Network.Messages;
using Mystrose.Network.Messages.Interfaces;
using Mystrose.ScriptMachine.Enumerations;
using System.Text.Json;

namespace Mystrose.Network.Handlers.JSON;

public class JHClass : IJSONMessageHandler
{

    #region Commands
    public string[] HandledCommands
    {
        get =>
        [
            "updateClass"
        ];
    }
    #endregion

    #region Methods: Handler
    public void Handle(GameHost host, JSONMessage message)
    {
        Avatar? avatar = host.World.Area.Players.Find(
            (avt) =>
            {
                return avt.EntityID == message.DataObject["uid"].Deserialize<int>();
            });

        if (avatar is null)
        {
            return;
        }

        int cp = message.DataObject["iCP"].Deserialize<int>();
        string className = message.DataObject["sClassName"].Deserialize<string>();

        if (avatar.EntityID == host.World.Master.EntityID)
        {
            host.World.Master.ClassPoints = cp;
            host.World.Master.Class = className;

            host.ScriptManager.InvokeTriggerSystems(ScriptTriggerType.Self, host.World.Master);
        }

        avatar.ClassPoints = cp;
        avatar.Class = className;

        host.ScriptManager.InvokeTriggerSystems(ScriptTriggerType.Player, avatar);
    }
    #endregion

}
