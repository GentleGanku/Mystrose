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
    public void Handle(JSONMessage message)
    {
        switch (message.Command)
        {
            case "initUserData":
                HandleData(message);
                break;
            case "initUserDatas":
                HandleDatas(message);
                break;
        }
    }
    #endregion

    #region Methods: User Data
    private void HandleData(JSONMessage message)
    {
        World world = message.World;
        JsonObject obj = message.DataObject;

        Avatar? avatar = world.Environment.Area.Players.Find(
            (avt) =>
            {
                return avt.EntityID == obj["uid"].Deserialize<int>();
            });

        if (avatar is null)
        {
            avatar = obj["data"].Deserialize<Avatar>();
            world.Environment.Area.Players.Add(avatar);
        }
        else
        {
            avatar.SetProperties(obj["data"].Deserialize<JsonObject>());
        }

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, avatar);

        if (avatar.EntityID == world.Avatar.UserID)
        {
            int masterId = world.Avatar.UserID;
            string masterName = world.Avatar.Name;

            world.RefreshAvatar(obj["data"].Deserialize<MainAvatar>());
            world.Avatar.UserID = masterId;
            world.Avatar.Name = masterName;
        }
    }

    private void HandleDatas(JSONMessage message)
    {
        World world = message.World;
        JsonObject obj = message.DataObject;

        foreach (JsonObject userObj in (JsonArray)obj["a"])
        {
            Avatar? avatar = world.Environment.Area.Players.Find(
                (avt) =>
                {
                    return avt.EntityID == obj["uid"].Deserialize<int>();
                });

            if (avatar is null)
            {
                avatar = obj["data"].Deserialize<Avatar>();
                world.Environment.Area.Players.Add(avatar);
            }
            else
            {
                avatar.SetProperties(obj["data"].Deserialize<JsonObject>());
            }

            SVCScriptManager.InvokeTrigger(message.Identifier.Codename, avatar);

            if (avatar.EntityID == world.Avatar.UserID)
            {
                int masterId = world.Avatar.UserID;
                string masterName = world.Avatar.Name;

                world.RefreshAvatar(obj["data"].Deserialize<MainAvatar>());
                world.Avatar.UserID = masterId;
                world.Avatar.Name = masterName;
            }
        }
    }
    #endregion

}
