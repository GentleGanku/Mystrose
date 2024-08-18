namespace Mystrose.Core.ScriptMachine.Commands.Interfaces;

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
