namespace Mystrose.Core.ScriptMachine.Base.Parameters;

/// <summary>
/// A base class that represents an object containing one specific value.
/// </summary>
public class ScriptParameter
{

    #region Constructors
    public ScriptParameter()
    {
        SetValue("");
        SetHint(null);
    }

    public ScriptParameter(object value, string? hint = null)
    {
        SetValue(value);
        SetHint(hint);
    }
    #endregion

    #region Properties
    /// <summary>
    /// The parameter's input type.
    /// </summary>
    /// <returns>
    /// An enumeration representing the parameter's input type.
    /// </returns>
    public ScriptParameterInputType? InputType
    {
        get;
        set;
    } = ScriptParameterInputType.Parameter;

    /// <summary>
    /// The parameter's value type.
    /// </summary>
    /// <returns>
    /// An enumeration representing the parameter's value type.
    /// </returns>
    public ScriptValueType? Type
    {
        get;
        set;
    }

    /// <summary>
    /// The parameter's object value.
    /// </summary>
    /// <returns>
    /// An object representing the parameter's value.
    /// </returns>
    [JsonIgnore]
    public object Object
    {
        get => Type switch
        {
            ScriptValueType.String => String,
            ScriptValueType.Integer => Integer,
            ScriptValueType.Double => Double,
            ScriptValueType.Boolean => Boolean,
            _ => string.Empty
        };
    }

    /// <summary>
    /// The parameter's string value.
    /// </summary>
    /// <returns>
    /// A string representing the parameter's value.
    /// </returns>
    public string String
    {
        get;
        set;
    }

    /// <summary>
    /// The parameter's integer value.
    /// </summary>
    /// <returns>
    /// An integer representing the parameter's value.
    /// </returns>
    public int Integer
    {
        get;
        set;
    }

    /// <summary>
    /// The parameter's double value.
    /// </summary>
    /// <returns>
    /// A double representing the parameter's value.
    /// </returns>
    public double Double
    {
        get;
        set;
    }

    /// <summary>
    /// The parameter's boolean value.
    /// </summary>
    /// <returns>
    /// A boolean representing the parameter's value.
    /// </returns>
    public bool Boolean
    {
        get;
        set;
    }

    /// <summary>
    /// The parameter's hint.
    /// </summary>
    /// <returns>
    /// A string representing the parameter's input hint.
    /// </returns>
    public string Hint
    {
        get;
        set;
    }
    #endregion

    #region Methods
    public ScriptParameter GetVar(ScriptEngine engine)
    {
        return engine.GetVariableValue(this);
    }

    public void SetValue(object value)
    {
        Empty();

        switch (value)
        {
            case int intValue:
                Integer = intValue;
                Type = ScriptValueType.Integer;
                break;
            case double doubleValue:
                Double = doubleValue;
                Type = ScriptValueType.Double;
                break;
            case bool boolValue:
                Boolean = boolValue;
                Type = ScriptValueType.Boolean;
                break;

            case string stringValue:
                if (int.TryParse(value.ToString(), out int parsedInt))
                {
                    Integer = parsedInt;
                    Type = ScriptValueType.Integer;
                }
                else if (double.TryParse(value.ToString(), out double parsedDouble))
                {
                    Double = parsedDouble;
                    Type = ScriptValueType.Double;
                }
                else if (bool.TryParse(value.ToString(), out bool parsedBool))
                {
                    Boolean = parsedBool;
                    Type = ScriptValueType.Boolean;
                }
                else
                {
                    String = stringValue;
                    Type = ScriptValueType.String;
                }
                break;
        }
    }

    public void SetHint(string? hint)
    {
        Hint = hint ?? Type switch
        {
            ScriptValueType.String => "Input a text string.\r\nExample: Text",
            ScriptValueType.Integer => "Input a round number.\r\nExample: 1",
            ScriptValueType.Double => "Input a decimal number.\r\nExample: 1.0",
            ScriptValueType.Boolean => "Input True or False.\r\nExample: True",
            _ => "Input a corresponding value for the parameter."
        };
    }

    public void Empty()
    {
        switch (Type)
        {
            case ScriptValueType.String:
                String = string.Empty;
                break;
            case ScriptValueType.Integer:
                Integer = -1;
                break;
            case ScriptValueType.Double:
                Double = -1.0;
                break;
            case ScriptValueType.Boolean:
                Boolean = false;
                break;
        }

        Type = null;
    }

    public override string? ToString()
    {
        return Type switch
        {
            ScriptValueType.String => String,
            ScriptValueType.Integer => Integer.ToString(),
            ScriptValueType.Double => Double.ToString(),
            ScriptValueType.Boolean => Boolean.ToString(),
            _ => string.Empty
        };
    }
    #endregion

}
