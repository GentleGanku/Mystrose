using Mystrose.ScriptMachine.Enumerations;
using Mystrose.ScriptMachine.Inputs;
using System.Text.Json.Serialization;

namespace Mystrose.ScriptMachine.Objects;

/// <summary>
/// A base class that represents a script variable.
/// </summary>
public class ScriptVariable
{

    #region Constructor
    public ScriptVariable()
    {
        KeyValuePair = new("", "");
    }

    public ScriptVariable(string key, object value)
    {
        KeyValuePair = new(key, value);
    }
    #endregion

    #region Fields
    public ScriptVariableType Type
    {
        get => KeyValuePair.Type switch
        {
            ScriptValueType.Object => ScriptVariableType.Object,
            ScriptValueType.String => ScriptVariableType.String,
            ScriptValueType.Integer => ScriptVariableType.Integer,
            ScriptValueType.Double => ScriptVariableType.Double,
            ScriptValueType.Boolean => ScriptVariableType.Boolean
        };
    }

    public string Key
    {
        get => KeyValuePair.Key;
    }

    public string Value
    {
        get => KeyValuePair.ToString();
    }
    #endregion

    #region Properties
    [JsonIgnore]
    public ScriptKeyValuePair KeyValuePair
    {
        get;
        private set;
    }
    #endregion

    #region Methods
    public void SetValue(object? value)
    {
        KeyValuePair.SetValue(value);
    }
    #endregion

}
