namespace Mystrose.Network.Handlers.XT;

public class XHEntityStats() : MessageHandler<XTMessage>(new()
{
    ["uotls"] = HandleUserStats,
    ["mtls"] = HandleMonsterStats
})
{

    #region Methods: Handlers
    private static void HandleUserStats(XTMessage message)
    {
        string avatarName = message.Arguments[4];
        Avatar? avatar = message.HostWorld.Area.Players.Find(
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

        MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, avatar);
    }

    private static void HandleMonsterStats(XTMessage message)
    {
        int monMapId = int.Parse(message.Arguments[4]);
        Monster? monster = message.HostWorld.Area.Monsters.Find(
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

        MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, monster);
    }
    #endregion

}
