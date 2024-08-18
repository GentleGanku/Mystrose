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
