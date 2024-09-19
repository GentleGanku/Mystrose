namespace Mystrose.Network.Handlers.ZM;

public static class ZHSkillCooldown
{

    #region Fields
    private static readonly Dictionary<string, Action<ZMMessage>> _handlers = new()
    {
        ["cooldownAct"] = HandleSkillUsability
    };
    #endregion

    #region Methods: Invoker
    public static void Invoke(ZMMessage message)
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
    public static void HandleSkillUsability(ZMMessage message)
    {
        int index = int.Parse(message.Arguments[4]) - 1;
        bool isUsable = bool.Parse(message.Arguments[5]);

        ActiveSkill activeSkill = message.World.Skills[index]!;
        activeSkill.IsUsable = isUsable;

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, activeSkill);
    }
    #endregion

}
