using Mystrose.ScriptMachine.Enumerations;
using Mystrose.ScriptMachine.Inputs;
using Mystrose.ScriptMachine.Objects;
using System.Collections.Generic;

namespace Mystrose.ScriptMachine.Interfaces;

public interface ITriggerCommand
{

    #region Inputs & Outputs
    ScriptTriggerType? TriggerType
    {
        get;
    }

    bool IsReverseChecked
    {
        get;
    }

    bool IsEnabled
    {
        get;
    }
    #endregion

    #region Methods
    bool IsValid(ScriptEngine engine, Dictionary<string, ScriptParameter> parameters);
    #endregion

}
