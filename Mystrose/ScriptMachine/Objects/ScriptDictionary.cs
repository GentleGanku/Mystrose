using Mystrose.ScriptMachine.Commands.Action;
using Mystrose.ScriptMachine.Enumerations;
using System.Collections.Generic;
using System.Linq;

namespace Mystrose.ScriptMachine.Objects;

public class ScriptDictionary : Dictionary<ScriptCodelineType, List<ScriptCommand>>
{

    #region Constructor
    public ScriptDictionary() : base()
    {
        base[ScriptCodelineType.Action] =
        [
            new ACMDIndexJump(),
            new ACMDTargetSetter(),
            new ACMDStanceSwitch(),
            new ACMDVariableSetter(),
            new ACMDWait(),
            new ACMDSkillUse(),
            new ACMDRest()
        ];
        base[ScriptCodelineType.Trigger] =
        [
            new SCMDTrigger()
        ];
        base[ScriptCodelineType.Variable] =
        [
            new SCMDVariable()
        ];
        base[ScriptCodelineType.SpecialCommand] =
        [
            new SCMDFiller(),
            new SCMDStatement(),
            new SCMDList()
        ];
    }
    #endregion

    #region Fields
    public new List<ScriptCommand> this[ScriptCodelineType key]
    {
        get => base[key];
    }

    public List<ScriptCommand> this[int key]
    {
        get => base[(ScriptCodelineType)key];
    }

    public ScriptCommand this[ScriptCodelineType key, int index]
    {
        get => base[key][index];
    }

    public ScriptCommand this[int key, int index]
    {
        get => base[(ScriptCodelineType)key][index];
    }

    public ScriptCommand? this[ScriptCodelineType key, string id]
    {
        get => base[key].Find(command => command.ID == id);
    }

    public ScriptCommand? this[int key, string id]
    {
        get => base[(ScriptCodelineType)key].Find(command => command.ID == id);
    }

    public ScriptCodelineType this[ScriptCommand command]
    {
        get => Keys.FirstOrDefault(type => this[type].Find(cmd => cmd.ID == command.ID) != null);
    }
    #endregion

    #region Methods
    public void Add(ScriptCodelineType key, ScriptCommand value)
    {
        this[key].Add(value);
    }

    public void Add(int key, ScriptCommand value)
    {
        this[(ScriptCodelineType)key].Add(value);
    }

    public bool Remove(ScriptCodelineType key, ScriptCommand value)
    {
        return this[key].Remove(value);
    }

    public bool Remove(int key, ScriptCommand value)
    {
        return this[(ScriptCodelineType)key].Remove(value);
    }
    #endregion

}
