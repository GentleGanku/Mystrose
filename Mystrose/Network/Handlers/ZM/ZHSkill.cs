namespace Mystrose.Network.Handlers.ZM;

public class ZHSkill : IZMMessageHandler
{

    #region Commands
    public string[] HandledCommands
    {
        get =>
        [
            "cooldownAct"
        ];
    }
    #endregion

    #region Methods: Handler
    public void Handle(GameHost host, ZMMessage message)
    {
        switch (message.Command)
        {
            case "cooldownAct":
                HandleSkillCooldown(host, message.Arguments);
                break;
        }
    }
    #endregion

    #region Methods: Skill
    private void HandleSkillCooldown(GameHost host, string[] args)
    {
        int index = int.Parse(args[4]) - 1;
        bool isUsable = bool.Parse(args[5]);

        ActiveSkill activeSkill = host.World.Skills[index];

        activeSkill.IsUsable = isUsable;

        host.ScriptManager.InvokeTriggerSystems(ScriptTriggerType.Skill, activeSkill);
    }
    #endregion

}
