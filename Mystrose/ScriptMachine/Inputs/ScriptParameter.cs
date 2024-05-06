using Mystrose.ScriptMachine.Enumerations;

namespace Mystrose.ScriptMachine.Inputs;

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

    #region Private Fields
    private object? _object;
    private string? _string;
    private int? _integer;
    private double? _double;
    private bool? _boolean;
    #endregion

    #region Properties
    /// <summary>
    /// The parameter's value type.
    /// </summary>
    /// <returns>
    /// An enumeration representing the parameter's value type.
    /// </returns>
    public ScriptValueType? Type
    {
        get;
        private set;
    }

    /// <summary>
    /// The parameter's object value.
    /// </summary>
    /// <returns>
    /// An object representing the parameter's value.
    /// </returns>
    public object? Object
    {
        get => _object;
        private set
        {
            _object = value;
            Type = ScriptValueType.Object;
        }
    }

    /// <summary>
    /// The parameter's string value.
    /// </summary>
    /// <returns>
    /// A string representing the parameter's value.
    /// </returns>
    public string? String
    {
        get => _string;
        private set
        {
            _string = value;
            Type = ScriptValueType.String;
        }
    }

    /// <summary>
    /// The parameter's integer value.
    /// </summary>
    /// <returns>
    /// An integer representing the parameter's value.
    /// </returns>
    public int? Integer
    {
        get => _integer;
        private set
        {
            _integer = value;
            Type = ScriptValueType.Integer;
        }
    }

    /// <summary>
    /// The parameter's double value.
    /// </summary>
    /// <returns>
    /// A double representing the parameter's value.
    /// </returns>
    public double? Double
    {
        get => _double;
        private set
        {
            _double = value;
            Type = ScriptValueType.Double;
        }
    }

    /// <summary>
    /// The parameter's boolean value.
    /// </summary>
    /// <returns>
    /// A boolean representing the parameter's value.
    /// </returns>
    public bool? Boolean
    {
        get => _boolean;
        private set
        {
            _boolean = value;
            Type = ScriptValueType.Boolean;
        }
    }

    /// <summary>
    /// The parameter's hint.
    /// </summary>
    /// <returns>
    /// A string representing the parameter's input hint.
    /// </returns>
    public string? Hint
    {
        get;
        private set;
    }

    /// <summary>
    /// The parameter's placeholder text.
    /// </summary>
    /// <returns>
    /// A string representing the parameter's placeholder text.
    /// </returns>
    public string? PlaceholderText
    {
        get;
        private set;
    }
    #endregion

    #region Methods
    public ScriptParameter RealValue(ScriptEngine engine)
    {
        return engine.GetVariableValue(this);
    }

    public void SetValue(object value)
    {
        Empty();
        Object = value;

        switch (value)
        {
            case int intValue:
                Integer = intValue;
                break;
            case double doubleValue:
                Double = doubleValue;
                break;
            case bool boolValue:
                Boolean = boolValue;
                break;

            case string stringValue:
                if (int.TryParse(value.ToString(), out int parsedInt))
                {
                    Integer = parsedInt;
                }
                else if (double.TryParse(value.ToString(), out double parsedDouble))
                {
                    Double = parsedDouble;
                }
                else if (bool.TryParse(value.ToString(), out bool parsedBool))
                {
                    Boolean = parsedBool;
                }
                else
                {
                    String = stringValue;
                }
                break;
        }

        SetPlaceholderText();
    }

    public void SetHint(string? hint)
    {
        Hint = hint ?? Type switch
        {
            ScriptValueType.String => "Input a text string.\r\nExample: Text123",
            ScriptValueType.Integer => "Input a round number.\r\nExample: 1",
            ScriptValueType.Double => "Input a decimal number.\r\nExample: 1.0",
            ScriptValueType.Boolean => "Input True or False.\r\nExample: True",
            _ => "Input a corresponding value for the parameter."
        };
    }

    public void SetPlaceholderText()
    {
        PlaceholderText = Type switch
        {
            ScriptValueType.String => "Text string",
            ScriptValueType.Integer => "Round number",
            ScriptValueType.Double => "Decimal number",
            ScriptValueType.Boolean => "True or False",
            ScriptValueType.Object => "Input value",
            _ => null
        };
    }

    public void Empty()
    {
        switch (Type)
        {
            case ScriptValueType.String:
                String = null;
                break;
            case ScriptValueType.Integer:
                Integer = null;
                break;
            case ScriptValueType.Double:
                Double = null;
                break;
            case ScriptValueType.Boolean:
                Boolean = null;
                break;
            case ScriptValueType.Object:
                Object = null;
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
            ScriptValueType.Object => Object?.ToString(),
            _ => null
        };
    }
    #endregion

}
