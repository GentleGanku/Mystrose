using Mystrose.ScriptMachine.Enumerations;
using System.Text.Json.Serialization;

namespace Mystrose.ScriptMachine.Inputs;

/// <summary>
/// A base class that represents a conditional statement object.
/// </summary>
public class ScriptKeyValuePair : ScriptParameter
{

    #region Constructors
    public ScriptKeyValuePair(string key) : base()
    {
        SetKey(key);
    }

    public ScriptKeyValuePair(string key, object value, string? hint = null) : base(value, hint)
    {
        SetKey(key);
        SetValue(value);
    }
    #endregion

    #region Properties
    /// <summary>
    /// The parameter's key.
    /// </summary>
    /// <returns>
    /// A string representing the parameter's specific key.
    /// </returns>
    public string? Key
    {
        get;
        private set;
    }

    /// <summary>
    /// The parameter's value.
    /// </summary>
    /// <returns>
    /// An object representing the parameter's value.
    /// </returns>
    public object? Value
    {
        get;
        private set;
    }
    #endregion

    #region Methods
    public void SetKey(string key)
    {
        Key = key;
    }

    public void SetValue(object? value)
    {
        base.SetValue(value);
        Value = Type switch
        {
            ScriptValueType.String => String,
            ScriptValueType.Integer => Integer,
            ScriptValueType.Double => Double,
            ScriptValueType.Boolean => Boolean,
            _ => Object
        };
    }

    public void Empty()
    {
        base.Empty();

        Key = null;
        Value = null;
    }
    #endregion

    #region Overrides
    public override string ToString()
    {
        return base.ToString();
    }
    #endregion

}
