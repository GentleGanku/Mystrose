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
    public void Handle(ZMMessage message)
    {
        switch (message.Command)
        {
            case "cooldownAct":
                HandleSkillCooldown(message);
                break;
        }
    }
    #endregion

    #region Methods: Skill
    private void HandleSkillCooldown(ZMMessage message)
    {
        World world = message.World;
        string[] args = message.Arguments;

        int index = int.Parse(args[4]) - 1;
        bool isUsable = bool.Parse(args[5]);

        ActiveSkill activeSkill = world.Skills[index]!;

        activeSkill.IsUsable = isUsable;

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, activeSkill);
    }
    #endregion

}
