namespace Mystrose.Network.Handlers.JSON;

public static class JHClassData
{

    #region Fields
    private static readonly Dictionary<string, Action<JSONMessage>> _handlers = new()
    {
        ["updateClass"] = HandleUpdateClass,
        ["sAct"] = HandleSkillActions,
        ["seia"] = HandleConsumable
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
    public static void HandleUpdateClass(JSONMessage message)
    {
        Avatar? avatar = message.World.Area.Players.Find(
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

        if (avatar.EntityID == message.World.Avatar.EntityID)
        {
            message.World.Avatar.ClassPoints = cp;
            message.World.Avatar.Class = className;

            SVCScriptManager.InvokeTrigger(message.Identifier.Codename, message.World.Avatar);
        }

        avatar.ClassPoints = cp;
        avatar.Class = className;

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, avatar);
    }

    public static void HandleSkillActions(JSONMessage message)
    {
        List<ActiveSkill> activeSkills = message.DataObject["actions"]!["active"].Deserialize<List<ActiveSkill>>()!;

        message.World.Skills = new(activeSkills);
        message.World.Skills.ForEach(s =>
        {
            s.Index = message.World.Skills.IndexOf(s);
        });
    }

    public static void HandleConsumable(JSONMessage message)
    {
        bool isReset = message.DataObject["iRes"].Deserialize<int>() == 1;

        if (!isReset)
        {
            return;
        }

        ActiveSkill consumable = message.World.Skills[5]!;
        consumable.SetProperties(message.DataObject["o"].Deserialize<JsonObject>()!);

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, consumable);
    }
    #endregion

}
