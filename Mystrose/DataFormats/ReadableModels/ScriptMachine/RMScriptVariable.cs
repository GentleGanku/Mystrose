namespace Mystrose.DataFormats.ReadableModels.ScriptMachine;

public class RMScriptVariable : ReadableModel<ScriptVariable>
{

    #region Constructor
    public RMScriptVariable(ScriptVariable? model = null, World? world = null)
        : base(model ?? new ScriptVariable(), world ?? new World())
    {
        KeyProperties = new()
        {
            [nameof(Key)] = Key
        };
    }
    #endregion

    #region Properties
    public string Key => Model.Key;
    public string Value => Model.Value;
    #endregion

    #region Methods: Overrides
    public override string ToString()
    {
        return $"{Key} = {Value}";
    }
    #endregion

}