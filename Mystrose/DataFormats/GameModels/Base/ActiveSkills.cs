namespace Mystrose.DataFormats.GameModels.Base;

public class ActiveSkills : List<ActiveSkill>
{

    #region Constructor
    public ActiveSkills(List<ActiveSkill>? skills = null)
    {
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

    #region Methods
    public void AddRange(List<ActiveSkill> skills)
    {
        Clear();
        base.AddRange(skills);
    }
    #endregion

}
