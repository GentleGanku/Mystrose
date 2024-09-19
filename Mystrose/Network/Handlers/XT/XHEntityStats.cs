namespace Mystrose.Network.Handlers.XT;

public static class XHEntityStats
{

    #region Fields
    private static readonly Dictionary<string, Action<XTMessage>> _handlers = new()
    {
        ["uotls"] = HandleUserStats,
        ["mtls"] = HandleMonsterStats
    };
    #endregion

    #region Methods: Invoker
    public static void Invoke(XTMessage message)
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
    public static void HandleUserStats(XTMessage message)
    {
        string avatarName = message.Arguments[4];
        Avatar? avatar = message.World.Area.Players.Find(
            (avt) =>
            {
                return avt.Name.Equals(avatarName);
            });

        if (avatar is null)
        {
            return;
        }

        string avatarInfo = message.Arguments[5];
        string[] avatarData = avatarInfo.Split(',');

        foreach (string dataObj in avatarData)
        {
            string[] data = dataObj.Split(":");
            avatar.SetProperty(data[0], data[1]);
        }

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, avatar);
    }

    public static void HandleMonsterStats(XTMessage message)
    {
        int monMapId = int.Parse(message.Arguments[4]);
        Monster? monster = message.World.Area.Monsters.Find(
            (mst) =>
            {
                return mst.MonMapID == monMapId;
            });

        if (monster is null)
        {
            return;
        }

        string monsterInfo = message.Arguments[5];
        string[] monsterData = monsterInfo.Split(',');

        foreach (string dataObj in monsterData)
        {
            string[] data = dataObj.Split(":");
            monster.SetProperty(data[0], data[1]);
        }

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, monster);
    }
    #endregion

}
