namespace Mystrose.Network.Handlers.JSON;

public class JHEntityStats() : MessageHandler<JSONMessage>(new()
{
    ["uotls"] = HandleUserStats,
    ["mtls"] = HandleMonsterStats
})
{

    #region Methods: Handlers
    private static void HandleUserStats(JSONMessage message)
    {
        Avatar? avatar = message.HostWorld.Area.Players.Find(
            (avt) =>
            {
                return avt.Name.Equals(message.DataObject["unm"].Deserialize<string>());
            });

        if (avatar is null)
        {
            avatar = message.DataObject["o"].Deserialize<JsonObject>().Deserialize<Avatar>();
            message.HostWorld.Area.Players.Add(avatar!);
        }
        else
        {
            avatar.SetProperties(message.DataObject["o"].Deserialize<JsonObject>()!);

            if (avatar.Name.Equals(message.HostWorld.Avatar.Name))
            {
                message.HostWorld.Avatar.SetProperties(message.DataObject["o"].Deserialize<JsonObject>()!);
                MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, message.HostWorld.Avatar);
            }
        }

        MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, avatar!);
    }

    private static void HandleMonsterStats(JSONMessage message)
    {
        Monster? monster = message.HostWorld.Area.Monsters.Find(
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

        MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, monster);
    }
    #endregion

}
