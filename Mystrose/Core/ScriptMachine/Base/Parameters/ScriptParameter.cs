namespace Mystrose.Core.ScriptMachine.Base.Parameters;

/// <summary>
/// A base class that represents an object containing one specific value.
/// </summary>
public class ScriptParameter
{

    #region Constructors
    public ScriptParameter(object value, string hint = "")
    {
        Value = value;
        Hint = hint;
    }
    #endregion

    #region Private Fields
    private object _value = string.Empty;
    private string _hint = string.Empty;
    #endregion

    #region Fields
    /// <summary>
    /// The parameter's value in string form.
    /// </summary>
    /// <returns>
    /// A string representing the parameter's value.
    /// </returns>
    [JsonIgnore]
    public string String
    {
        get => ValueType is ScriptValueType.String ? (string)Value : Value.ToString() ?? "";
    }

    /// <summary>
    /// The parameter's value in integer form.
    /// </summary>
    /// <returns>
    /// An integer representing the parameter's value.
    /// </returns>
    [JsonIgnore]
    public int Integer
    {
        get => ValueType is ScriptValueType.Integer ? (int)Value : int.Parse(Value.ToString() ?? "0");
    }

    /// <summary>
    /// The parameter's value in double form.
    /// </summary>
    /// <returns>
    /// A double representing the parameter's value.
    /// </returns>
    [JsonIgnore]
    public double Double
    {
        get => ValueType is ScriptValueType.Double ? (double)Value : double.Parse(Value.ToString() ?? "0.0");
    }

    /// <summary>
    /// The parameter's value in boolean form.
    /// </summary>
    /// <returns>
    /// A boolean representing the parameter's value.
    /// </returns>
    [JsonIgnore]
    public bool Boolean
    {
        get => ValueType is ScriptValueType.Boolean ? (bool)Value : bool.Parse(Value.ToString() ?? "False");
    }
    #endregion

    #region Properties
    /// <summary>
    /// The parameter's input type.
    /// </summary>
    /// <returns>
    /// An enumeration representing the parameter's input type.
    /// </returns>
    public virtual ScriptParameterInputType InputType
    {
        get => ScriptParameterInputType.Parameter;
    }

    /// <summary>
    /// The parameter's value type.
    /// </summary>
    /// <returns>
    /// An enumeration representing the parameter's value type.
    /// </returns>
    public ScriptValueType? ValueType
    {
        get;
        protected set;
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
        protected set => Set(value);
    }

    /// <summary>
    /// The parameter's hint.
    /// </summary>
    /// <returns>
    /// A string representing the parameter's input hint.
    /// </returns>
    public string Hint
    {
        get => _hint;
        protected set => SetHint(value);
    }
    #endregion

    #region Methods
    public ScriptParameter GetVariable(ScriptEngine engine)
    {
        return engine.GetVariableValue(this);
    }

    public void Set(object value)
    {
        Empty();

        switch (value)
        {
            case int intValue:
                _value = intValue;
                ValueType = ScriptValueType.Integer;
                break;
            case double doubleValue:
                _value = doubleValue;
                ValueType = ScriptValueType.Double;
                break;
            case bool boolValue:
                _value = boolValue;
                ValueType = ScriptValueType.Boolean;
                break;

            case string stringValue:
                if (int.TryParse(stringValue, out int parsedInt))
                {
                    _value = parsedInt;
                    ValueType = ScriptValueType.Integer;
                }
                else if (double.TryParse(stringValue, out double parsedDouble))
                {
                    _value = parsedDouble;
                    ValueType = ScriptValueType.Double;
                }
                else if (bool.TryParse(stringValue, out bool parsedBool))
                {
                    _value = parsedBool;
                    ValueType = ScriptValueType.Boolean;
                }
                else
                {
                    _value = stringValue;
                    ValueType = ScriptValueType.String;
                }
                break;
        }
    }

    public void SetHint(string? hint)
    {
        _hint = !string.IsNullOrEmpty(hint) ? hint : ValueType switch
        {
            ScriptValueType.String => "Input text",
            ScriptValueType.Integer => "Round number",
            ScriptValueType.Double => "Decimal number",
            ScriptValueType.Boolean => "True / False"
        };
    }

    public virtual void Empty()
    {
        _value = string.Empty;
        ValueType = null;
    }

    public override string? ToString()
    {
        return Value.ToString();
    }
    #endregion

}
