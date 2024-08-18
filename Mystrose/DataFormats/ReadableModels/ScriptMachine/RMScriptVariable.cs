namespace Mystrose.DataFormats.ReadableModels.ScriptMachine;

public class RMScriptVariable : ReadableModel
{

    #region Constructor
    public RMScriptVariable(ScriptVariable? model = null, World? world = null) : base(model, world)
    {
        Model = model ?? new ScriptVariable();
        MandatorySearchProperties = new()
        {
            [nameof(Key)] = Key
        };
    }
    #endregion

    #region Private Fields
    [JsonIgnore]
    private ScriptVariable ScriptVariable
    {
        get => (ScriptVariable)Model;
    }
    #endregion

    #region Properties
    public string Key
    {
        get => ScriptVariable.Key;
    }

    public string Value
    {
        get => ScriptVariable.Value;
    }
    #endregion

}
