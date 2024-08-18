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
    public void Handle(GameHost host, JSONMessage message)
    {
        bool isSuccess = message.DataObject["bitSuccess"].Deserialize<int>() == 1;

        if (!isSuccess)
        {
            return;
        }

        Avatar? avatar = host.World.Area.Players.Find(
            (avt) =>
            {
                return avt.EntityID == message.DataObject["uid"].Deserialize<int>();
            });

        if (avatar == null)
        {
            return;
        }

        GenderType gender = message.DataObject["gender"].Deserialize<string>() switch
        {
            "M" => GenderType.Male,
            "F" => GenderType.Female,
            _ => GenderType.Unknown
        };

        if (avatar.EntityID == host.World.Master.EntityID)
        {
            host.World.Master.AdventureCoins -= message.DataObject["intCoins"].Deserialize<int>();
            host.World.Master.Gender = gender;

            host.ScriptManager.InvokeTriggerSystems(ScriptTriggerType.Self, host.World.Master);
        }

        avatar.Gender = gender;
    }
    #endregion

}
