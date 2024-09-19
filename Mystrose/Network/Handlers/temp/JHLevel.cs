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
    public void Handle(JSONMessage message)
    {
        World world = message.World;

        world.Avatar.Level = message.DataObject["intLevel"].Deserialize<int>();

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, world.Avatar);
    }
    #endregion

}
