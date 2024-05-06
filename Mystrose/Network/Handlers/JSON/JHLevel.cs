using Mystrose.Controls.Main;
using Mystrose.Network.Messages;
using Mystrose.Network.Messages.Interfaces;
using Mystrose.ScriptMachine.Enumerations;
using System.Text.Json;

namespace Mystrose.Network.Handlers.JSON;

public class JHLevel : IJSONMessageHandler
{

    #region Commands
    public string[] HandledCommands
    {
        get =>
        [
            "levelUp"
        ];
    }
    #endregion

    #region Methods: Handler
    public void Handle(GameHost host, JSONMessage message)
    {
        host.World.Master.Level = message.DataObject["intLevel"].Deserialize<int>();

        host.ScriptManager.InvokeTriggerSystems(ScriptTriggerType.Self, host.World.Master);
    }
    #endregion

}
