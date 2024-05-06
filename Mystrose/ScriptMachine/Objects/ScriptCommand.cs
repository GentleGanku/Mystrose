using Mystrose.ScriptMachine.Enumerations;
using Mystrose.ScriptMachine.Inputs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mystrose.ScriptMachine.Objects;

public abstract class ScriptCommand
{

    #region Constructor
    public ScriptCommand(ScriptCommandType type, string id, string commandName, string commandDescription)
    {
        Type = type;
        ID = id;
        CommandName = commandName;
        CommandDescription = commandDescription;

        EndResult = ScriptResultType.Failure;
    }
    #endregion

    #region Fields
    public ScriptParameter? this[string key]
    {
        get => Parameters[key];
    }
    #endregion

    #region Properties
    public ScriptCommandType Type
    {
        get;
        private set;
    }

    public string ID
    {
        get;
        private set;
    }

    public string CommandName
    {
        get;
        private set;
    }

    public string CommandDescription
    {
        get;
        private set;
    }
    #endregion

    #region Inputs & Outputs
    public Dictionary<string, ScriptParameter> Parameters
    {
        get;
        set;
    }

    public Dictionary<string, Dictionary<string, ScriptParameter>> SecondaryParameters
    {
        get;
        set;
    }

    public ScriptResultType EndResult
    {
        get;
        set;
    }
    #endregion

    #region Methods
    public abstract ScriptCommand Clone();

    public abstract Dictionary<string, ScriptParameter> PassSecondaryParameters(string key);

    public abstract Task Execute(ScriptEngine engine);

    public abstract new string ToString();
    #endregion

}
