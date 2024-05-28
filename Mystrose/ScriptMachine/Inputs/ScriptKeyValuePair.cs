using Mystrose.ScriptMachine.Enumerations;

namespace Mystrose.ScriptMachine.Inputs;

/// <summary>
/// An inheritor class that represents a key value pair object.
/// </summary>
public class ScriptKeyValuePair : ScriptParameter
{

    #region Constructors
    public ScriptKeyValuePair()
    {
        // Empty constructor for serialization and deserialization.
    }

    public ScriptKeyValuePair(string key) : base()
    {
        InputType = ScriptParameterInputType.KeyValuePair;
        SetKey(key);
    }

    public ScriptKeyValuePair(string key, object value, string? hint = null) : base(value, hint)
    {
        InputType = ScriptParameterInputType.KeyValuePair;
        SetKey(key);
        SetValue(value);
    }
    #endregion

    #region Private Fields
    private string _key;
    private object _value;
    #endregion

    #region Properties
    /// <summary>
    /// The parameter's key.
    /// </summary>
    /// <returns>
    /// A string representing the parameter's specific key.
    /// </returns>
    public string Key
    {
        get => _key;
        set
        {
            _key = value;
        }
    }

    /// <summary>
    /// The parameter's value.
    /// </summary>
    /// <returns>
    /// An object representing the parameter's value.
    /// </returns>
    public object Value
    {
        get => _value;
        set
        {
            base.SetValue(value);

            _value = Type switch
            {
                ScriptValueType.String => String,
                ScriptValueType.Integer => Integer,
                ScriptValueType.Double => Double,
                ScriptValueType.Boolean => Boolean,
                _ => Object
            };
        }
    }
    #endregion

    #region Methods
    public void SetKey(string key)
    {
        Key = key;
    }

    public void SetValue(object value)
    {
        Value = value;
    }

    public void Empty()
    {
        Key = string.Empty;
        Value = string.Empty;

        base.Empty();
    }
    #endregion

    #region Overrides
    public override string ToString()
    {
        return base.ToString();
    }
    #endregion

}
