namespace Mystrose.Network.Handlers.JSON;

public static class JHEntityStats
{

    #region Fields
    private static readonly Dictionary<string, Action<JSONMessage>> _handlers = new()
    {
        ["uotls"] = HandleUserStats,
        ["mtls"] = HandleMonsterStats
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
    public static void HandleUserStats(JSONMessage message)
    {
        Avatar? avatar = message.World.Area.Players.Find(
            (avt) =>
            {
                return avt.Name.Equals(message.DataObject["unm"].Deserialize<string>());
            });

        if (avatar is null)
        {
            avatar = message.DataObject["o"].Deserialize<JsonObject>().Deserialize<Avatar>();
            message.World.Area.Players.Add(avatar!);
        }
        else
        {
            avatar.SetProperties(message.DataObject["o"].Deserialize<JsonObject>()!);

            if (avatar.Name.Equals(message.World.Avatar.Name))
            {
                message.World.Avatar.SetProperties(message.DataObject["o"].Deserialize<JsonObject>()!);
                SVCScriptManager.InvokeTrigger(message.Identifier.Codename, message.World.Avatar);
            }
        }

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, avatar!);
    }

    public static void HandleMonsterStats(JSONMessage message)
    {
        Monster? monster = message.World.Area.Monsters.Find(
            (mst) =>
            {
                return mst.MonMapID == message.DataObject["id"].Deserialize<int>();
            });

        if (monster is null)
        {
            return;
        }

        if (message.DataObject.ContainsKey("targets"))
        {
            monster.Targets = message.DataObject["targets"].Deserialize<List<string>>()!;
        }

        monster.SetProperties(message.DataObject["o"].Deserialize<JsonObject>()!);

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, monster);
    }
    #endregion

}
