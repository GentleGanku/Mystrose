namespace Mystrose.Network.Handlers.JSON;

public class JHGender : IJSONMessageHandler
{

    #region Commands
    public string[] HandledCommands
    {
        get =>
        [
            "genderSwap"
        ];
    }
    #endregion

    #region Methods: Handler
    public void Handle(JSONMessage message)
    {
        World world = message.World;
        JsonObject obj = message.DataObject;

        bool isSuccess = message.DataObject["bitSuccess"].Deserialize<int>() == 1;

        if (!isSuccess)
        {
            return;
        }

        Avatar? avatar = world.Environment.Area.Players.Find(
            (avt) =>
            {
                return avt.EntityID == message.DataObject["uid"].Deserialize<int>();
            });

        if (avatar is null)
        {
            return;
        }

        GenderType gender = message.DataObject["gender"].Deserialize<string>() switch
        {
            "M" => GenderType.Male,
            "F" => GenderType.Female,
            _ => GenderType.Unknown
        };

        if (avatar.EntityID == world.Avatar.EntityID)
        {
            world.Avatar.Coins -= message.DataObject["intCoins"].Deserialize<int>();
            world.Avatar.Gender = gender;

            SVCScriptManager.InvokeTrigger(message.Identifier.Codename, world.Avatar);
        }

        avatar.Gender = gender;
    }
    #endregion

}
