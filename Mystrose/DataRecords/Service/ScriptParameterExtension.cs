namespace Mystrose.DataRecords.Service;

public struct ScriptParameterExtension
{

    #region Constructor
    public ScriptParameterExtension(ScriptParameterInputType inputType, string value, string hint)
    {
        InputType = inputType;
        Value = value;
        Hint = hint;
    }
    #endregion

    #region Properties
    public ScriptParameterInputType InputType
    {
        get;
        init;
    }

    public string Value
    {
        get;
        init;
    }

    public string Hint
    {
        get;
        init;
    }
    #endregion

}
