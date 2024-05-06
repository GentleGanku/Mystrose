using Mystrose.ScriptMachine.Enumerations;
using Mystrose.ScriptMachine.Inputs;
using Mystrose.ScriptMachine.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mystrose.ScriptMachine.Objects;

public class SCMDAction : ScriptCommand, IActionCommand
{

    #region Constructor
    public SCMDAction(ScriptCommandType type = ScriptCommandType.Action, string id = "SCMD02", string commandName = "Action", string commandDescription = "A script command that performs a single in-game action, accompanied either with a ruleset or without one.") : base(type, id, commandName, commandDescription)
    {
        Parameters = [];
        SecondaryParameters = [];
    }
    #endregion

    #region Methods: Override
    public override ScriptCommand Clone()
    {
        return new SCMDAction(Type, ID, CommandName, CommandDescription)
        {
            Parameters = new(Parameters),
            SecondaryParameters = new(SecondaryParameters),
            EndResult = EndResult
        };
    }

    public override Dictionary<string, ScriptParameter> PassSecondaryParameters(string key)
    {
        SecondaryParameters[key] = [];
        return SecondaryParameters[key];
    }

    public override async Task Execute(ScriptEngine engine)
    {
        // No execution

        EndResult = ScriptResultType.Success;
    }

    public override string ToString()
    {
        return "Performs an action";
    }
    #endregion

}

