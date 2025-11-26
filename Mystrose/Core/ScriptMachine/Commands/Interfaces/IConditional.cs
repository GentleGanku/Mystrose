namespace Mystrose.Core.ScriptMachine.Codelines.Interfaces;

public interface IConditional
{

    #region Fields
    Dictionary<string, ScriptParameter> Mandatories
    {
        get;
    }

    Dictionary<string, ScriptParameter> Optionals
    {
        get;
    }
    #endregion

    #region Methods: Validation
    bool ValidateCondition(ScriptEngine engine, ScriptEntityModelType sourceType, Dictionary<string, ScriptParameter> parameters);
    #endregion

}
