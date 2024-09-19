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
    public void Handle(JSONMessage message)
    {
        World world = message.World;
        JsonObject obj = message.DataObject;

        Avatar? avatar = world.Environment.Area.Players.Find(
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

        if (avatar.EntityID == world.Avatar.EntityID)
        {
            world.Avatar.ClassPoints = cp;
            world.Avatar.Class = className;

            SVCScriptManager.InvokeTrigger(message.Identifier.Codename, world.Avatar);
        }

        avatar.ClassPoints = cp;
        avatar.Class = className;

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, avatar);
    }
    #endregion

}
