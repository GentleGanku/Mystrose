namespace Mystrose.DataRecords.ReadableModels;

public class RMScriptVariable(ScriptKeyValuePair? model = null, World? world = null) : ReadableModel(model ?? new ScriptKeyValuePair("", ""), world ?? new World())
{

    #region Properties: I/O
    public new ScriptKeyValuePair Model
    {
        get => (ScriptKeyValuePair)base.Model;
    }

    public override Dictionary<string, object> KeyProperties
    {
        get => new()
        {
            [nameof(Key)] = Key
        };
    }
    #endregion

    #region Properties: Attributes
    public string Key => Model.Key;
    public string Value => Model.String;
    #endregion

    #region Methods: Conversion
    public new ScriptKeyValuePair ToObject()
    {
        return Model;
    }

    public override string ToString()
    {
        return $"{Key} = {Value}";
    }
    #endregion

}