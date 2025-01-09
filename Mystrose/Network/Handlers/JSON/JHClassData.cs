namespace Mystrose.Network.Handlers.JSON;

public class JHClassData() : MessageHandler<JSONMessage>(new()
{
    ["updateClass"] = HandleUpdateClass,
    ["sAct"] = HandleSkillActions,
    ["seia"] = HandleConsumable
})
{

    #region Methods: Handlers
    private static void HandleUpdateClass(JSONMessage message)
    {
        Avatar? avatar = message.HostWorld.Area.Players.Find(
            (avt) =>
            {
                return avt.EntityID == message.DataObject["uid"].Deserialize<int>();
            });

        if (avatar is null)
        {
            return;
        }

        int cp = message.DataObject["iCP"].Deserialize<int>();
        string className = message.DataObject["sClassName"].Deserialize<string>()!;

        if (avatar.EntityID == message.HostWorld.Avatar.EntityID)
        {
            message.HostWorld.Avatar.ClassPoints = cp;
            message.HostWorld.Avatar.Class = className;

            MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, message.HostWorld.Avatar);
        }

        avatar.ClassPoints = cp;
        avatar.Class = className;

        MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, avatar);
    }

    private static void HandleSkillActions(JSONMessage message)
    {
        List<ActiveSkill> activeSkills = message.DataObject["actions"]!["active"].Deserialize<List<ActiveSkill>>()!;

        message.HostWorld.Skills = new(activeSkills);
        message.HostWorld.Skills.ForEach(s =>
        {
            s.Index = message.HostWorld.Skills.IndexOf(s);
        });
    }

    private static void HandleConsumable(JSONMessage message)
    {
        bool isReset = message.DataObject["iRes"].Deserialize<int>() == 1;

        if (!isReset)
        {
            return;
        }

        ActiveSkill consumable = message.HostWorld.Skills[5]!;
        consumable.SetProperties(message.DataObject["o"].Deserialize<JsonObject>()!);

        MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, consumable);
    }
    #endregion

}
