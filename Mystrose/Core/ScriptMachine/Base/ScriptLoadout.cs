namespace Mystrose.Core.ScriptMachine.Base;

public class ScriptLoadout
{

    #region Constructor
    public ScriptLoadout(string name = "Loadout", string description = "The default description of the loadout.", string author = "Unknown")
    {
        Name = name;
        Description = description;
        Author = author;

        Stances =
        [
            new("Main")
        ];
        Triggers = [];
        PresetVariables = [];
        Variables = [];
    }
    #endregion

    #region Properties: Information
    [JsonIgnore] // This attribute is temporary.
    public string UID
    {
        get;
        private set;
    }

    public string Name
    {
        get;
        set;
    }

    public string Description
    {
        get;
        set;
    }

    public string Author
    {
        get;
        set;
    }
    #endregion

    #region Properties: Collection
    [JsonInclude]
    public List<ScriptStance> Stances
    {
        get;
        set;
    }

    [JsonInclude]
    public List<SCMDTrigger> Triggers
    {
        get;
        set;
    }

    [JsonInclude]
    public List<SCMDVariable> PresetVariables
    {
        get;
        set;
    }

    [JsonIgnore]
    public ScriptVariableDictionary Variables
    {
        get;
        private set;
    }
    #endregion

    #region Methods: Stance
    public ScriptStance? GetStance(string name)
    {
        return Stances.Find(stance => stance.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public ScriptStance? GetStance(int index)
    {
        return Stances[index];
    }

    public void AddStance(ScriptStance stance)
    {
        Stances.Add(stance);
    }

    public void AddStance(string name)
    {
        Stances.Add(new(name));
    }

    public void RemoveStance(ScriptStance stance)
    {
        Stances.Remove(stance);
    }

    public void RemoveStance(string name)
    {
        Stances.Remove(GetStance(name));
    }

    public void RemoveStance(int index)
    {
        Stances.RemoveAt(index);
    }

    public void ClearStances()
    {
        Stances.Clear();
    }
    #endregion

    #region Methods: Trigger
    public SCMDTrigger? GetTrigger(string name)
    {
        return Triggers.Find(trigger => trigger.LabelName.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public SCMDTrigger? GetTrigger(int index)
    {
        return Triggers[index];
    }

    public void AddTrigger(SCMDTrigger trigger)
    {
        Triggers.Add(trigger);
    }

    public void AddTrigger(SCMDTrigger trigger, int index)
    {
        Triggers.Insert(index, trigger);
    }

    public void RemoveTrigger(SCMDTrigger trigger)
    {
        Triggers.Remove(trigger);
    }

    public void RemoveTrigger(string name)
    {
        Triggers.Remove(GetTrigger(name));
    }

    public void RemoveTrigger(int index)
    {
        Triggers.RemoveAt(index);
    }

    public void ClearTriggers()
    {
        Triggers.Clear();
    }
    #endregion

    #region Methods: Variable
    public ScriptVariable? GetVariable(string name)
    {
        return Variables.TryGetValue(name, out ScriptVariable? variable) ? variable : null;
    }

    public void AddVariable(ScriptVariable variable)
    {
        Variables.Add(variable);
    }

    public void AddVariable(string key, object value)
    {
        Variables.Add(new(key, value));
    }

    public void RemoveVariable(string key)
    {
        Variables.Remove(key);
    }

    public void ClearVariables()
    {
        Variables.Clear();
    }
    #endregion

}
