namespace Mystrose.Network.Handlers.JSON;

public static class JHUserData
{

    #region Fields
    private static readonly Dictionary<string, Action<JSONMessage>> _handlers = new()
    {
        ["initUserData"] = HandleUserData,
        ["initUserDatas"] = HandleUserDatas,
        ["genderSwap"] = HandleGenderSwap,
        ["levelUp"] = HandleLevelUp,
        ["stu"] = HandleStats
    };
    #endregion

    #region Methods: Invoker
    public static void Invoke(JSONMessage message)
    {
        if (!_handlers.TryGetValue(message.Command, out var handler))
        {
            return;
        }

        try
        {
            handler.Invoke(message);
        }
        catch (Exception ex)
        {
            SVCLogger.LogOnException($"({nameof(message)} - {message.Command}) {ex.ToString()}");
        }
    }
    #endregion

    #region Handlers
    public static void HandleUserData(JSONMessage message)
    {
        Avatar? avatar = message.World.Area.Players.Find(
            (avt) =>
            {
                return avt.EntityID == message.DataObject["uid"].Deserialize<int>();
            });

        if (avatar is null)
        {
            avatar = message.DataObject["data"].Deserialize<Avatar>();
            message.World.Area.Players.Add(avatar!);
        }
        else
        {
            avatar.SetProperties(message.DataObject["data"].Deserialize<JsonObject>()!);
        }

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, avatar!);

        if (avatar.EntityID == message.World.Avatar.EntityID)
        {
            MainAvatar mainAvatar = message.DataObject["data"].Deserialize<MainAvatar>()!;
            message.World.RefreshAvatar(mainAvatar);

            BoostStatuses boostStatuses = message.DataObject["data"].Deserialize<BoostStatuses>()!;
            message.World.Boosts = boostStatuses;
        }
    }

    public static void HandleUserDatas(JSONMessage message)
    {
        JsonArray usersArray = (JsonArray)message.DataObject["a"]!;

        foreach (JsonNode user in usersArray)
        {
            Avatar? avatar = message.World.Area.Players.Find(
                (avt) =>
                {
                    return avt.EntityID == user["uid"].Deserialize<int>();
                });

            if (avatar is null)
            {
                avatar = user["data"].Deserialize<Avatar>();
                message.World.Area.Players.Add(avatar!);
            }
            else
            {
                avatar.SetProperties(user["data"].Deserialize<JsonObject>()!);
            }

            SVCScriptManager.InvokeTrigger(message.Identifier.Codename, avatar!);

            if (avatar.EntityID == message.World.Avatar.EntityID)
            {
                MainAvatar mainAvatar = user["data"].Deserialize<MainAvatar>()!;
                message.World.RefreshAvatar(mainAvatar);

                BoostStatuses boostStatuses = user["data"].Deserialize<BoostStatuses>()!;
                message.World.Boosts = boostStatuses;
            }
        }
    }

    public static void HandleGenderSwap(JSONMessage message)
    {
        bool isSuccess = message.DataObject["bitSuccess"].Deserialize<int>() == 1;

        if (!isSuccess)
        {
            return;
        }

        Avatar? avatar = message.World.Area.Players.Find(
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

        if (avatar.EntityID == message.World.Avatar.EntityID)
        {
            int coins = message.DataObject["intCoins"].Deserialize<int>();
            message.World.Avatar.Coins -= coins;

            message.World.Avatar.Gender = gender;

            SVCScriptManager.InvokeTrigger(message.Identifier.Codename, message.World.Avatar);
        }

        avatar.Gender = gender;

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, avatar);
    }

    public static void HandleLevelUp(JSONMessage message)
    {
        int newLevel = message.DataObject["intLevel"].Deserialize<int>();
        message.World.Avatar.Level = newLevel;

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, message.World.Avatar);
    }

    public static void HandleStats(JSONMessage message)
    {
        if (message.World.Avatar.Stats is null)
        {
            message.World.Avatar.Stats = message.DataObject["sta"].Deserialize<Stats>()!;
        }
        else
        {
            message.World.Avatar.Stats.SetProperties(message.DataObject["sta"].Deserialize<JsonObject>()!);
        }

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, message.World.Avatar.Stats);
    }
    #endregion

}
