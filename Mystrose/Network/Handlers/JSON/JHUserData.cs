namespace Mystrose.Network.Handlers.JSON;

public class JHUserData() : MessageHandler<JSONMessage>(new()
{
    ["initUserData"] = HandleUserData,
    ["initUserDatas"] = HandleUserDatas,
    ["genderSwap"] = HandleGenderSwap,
    ["levelUp"] = HandleLevelUp,
    ["stu"] = HandleStats
})
{

    #region Methods: Handlers
    private static void HandleUserData(JSONMessage message)
    {
        Avatar? avatar = message.HostWorld.Area.Players.Find(
            (avt) =>
            {
                return avt.EntityID == message.DataObject["uid"].Deserialize<int>();
            });

        if (avatar is null)
        {
            avatar = message.DataObject["data"].Deserialize<Avatar>();
            message.HostWorld.Area.Players.Add(avatar!);
        }
        else
        {
            avatar.SetProperties(message.DataObject["data"].Deserialize<JsonObject>()!);
        }

        MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, avatar!);

        if (avatar.EntityID == message.HostWorld.Avatar.EntityID)
        {
            MainAvatar mainAvatar = message.DataObject["data"].Deserialize<MainAvatar>()!;
            message.HostWorld.RefreshAvatar(mainAvatar);

            BoostStatuses boostStatuses = message.DataObject["data"].Deserialize<BoostStatuses>()!;
            message.HostWorld.Boosts = boostStatuses;
        }
    }

    private static void HandleUserDatas(JSONMessage message)
    {
        JsonArray usersArray = (JsonArray)message.DataObject["a"]!;

        foreach (JsonNode user in usersArray)
        {
            Avatar? avatar = message.HostWorld.Area.Players.Find(
                (avt) =>
                {
                    return avt.EntityID == user["uid"].Deserialize<int>();
                });

            if (avatar is null)
            {
                avatar = user["data"].Deserialize<Avatar>();
                message.HostWorld.Area.Players.Add(avatar!);
            }
            else
            {
                avatar.SetProperties(user["data"].Deserialize<JsonObject>()!);
            }

            MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, avatar!);

            if (avatar.EntityID == message.HostWorld.Avatar.EntityID)
            {
                MainAvatar mainAvatar = user["data"].Deserialize<MainAvatar>()!;
                message.HostWorld.RefreshAvatar(mainAvatar);

                BoostStatuses boostStatuses = user["data"].Deserialize<BoostStatuses>()!;
                message.HostWorld.Boosts = boostStatuses;
            }
        }
    }

    private static void HandleGenderSwap(JSONMessage message)
    {
        bool isSuccess = message.DataObject["bitSuccess"].Deserialize<int>() == 1;

        if (!isSuccess)
        {
            return;
        }

        Avatar? avatar = message.HostWorld.Area.Players.Find(
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

        if (avatar.EntityID == message.HostWorld.Avatar.EntityID)
        {
            int coins = message.DataObject["intCoins"].Deserialize<int>();
            message.HostWorld.Avatar.Coins -= coins;

            message.HostWorld.Avatar.Gender = gender;

            MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, message.HostWorld.Avatar);
        }

        avatar.Gender = gender;

        MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, avatar);
    }

    private static void HandleLevelUp(JSONMessage message)
    {
        int newLevel = message.DataObject["intLevel"].Deserialize<int>();
        message.HostWorld.Avatar.Level = newLevel;

        MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, message.HostWorld.Avatar);
    }

    private static void HandleStats(JSONMessage message)
    {
        if (message.HostWorld.Avatar.Stats is null)
        {
            message.HostWorld.Avatar.Stats = message.DataObject["sta"].Deserialize<Stats>()!;
        }
        else
        {
            message.HostWorld.Avatar.Stats.SetProperties(message.DataObject["sta"].Deserialize<JsonObject>()!);
        }

        MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, message.HostWorld.Avatar.Stats);
    }
    #endregion

}
