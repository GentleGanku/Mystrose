namespace Mystrose.Network.Handlers.ZM;

public class ZHSkillCooldown() : MessageHandler<ZMMessage>(new()
{
    ["cooldownAct"] = HandleSkillUsability
})
{

    #region Methods: Handlers
    private static void HandleSkillUsability(ZMMessage message)
    {
        int index = int.Parse(message.Arguments[4]) - 1;
        bool isUsable = bool.Parse(message.Arguments[5]);

        ActiveSkill activeSkill = message.HostWorld.Skills[index]!;
        activeSkill.IsUsable = isUsable;

        MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, activeSkill);
    }
    #endregion

}
