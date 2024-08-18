namespace Mystrose.DataFormats.GameModels.Base;

public class SkillManager : List<ActiveSkill>
{

    #region Constructor
    public SkillManager(GameHost host, List<ActiveSkill>? skills = null)
    {
        Host = host;

        AddRange(skills ?? []);
    }
    #endregion

    #region Fields
    public ActiveSkill? this[int index]
    {
        get => base[index];
    }

    public ActiveSkill? this[ActionType type]
    {
        get => Find(x => x.ActionType == type);
    }

    public ActiveSkill? this[string name]
    {
        get => Find(x => x.Name == name);
    }
    #endregion

    #region Properties
    public GameHost? Host
    {
        get;
        set;
    }
    #endregion

    #region Methods
    public SkillManager AddSkills(List<ActiveSkill> skills)
    {
        AddRange(skills);

        return this;
    }

    public void LockSkill(Aura aura)
    {
        if (!aura.Name.Equals("Skill Locked"))
        {
            return;
        }

        if (!aura.TargetID.Equals(Host.World.Master.Name, StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        ActiveSkill? skill = this[aura.Value];
        if (skill is not null && !skill.IsLocked)
        {
            skill.IsLocked = true;

            Host.ScriptManager.InvokeTriggerSystems(ScriptTriggerType.Skill, skill);
        }
    }

    public void UnlockSkill(Aura aura)
    {
        if (!aura.Name.Equals("Skill Locked"))
        {
            return;
        }

        if (!aura.TargetID.Equals(Host.World.Master.Name, StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        ActiveSkill? skill = this[aura.Value];
        if (skill is not null && skill.IsLocked)
        {
            skill.IsLocked = false;

            Host.ScriptManager.InvokeTriggerSystems(ScriptTriggerType.Skill, skill);
        }
    }
    #endregion

}
