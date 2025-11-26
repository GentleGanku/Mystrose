namespace Mystrose.Core.ScriptMachine.Base.Parameters;

/// <summary>
/// An inheritor class that represents a conditional statement object.
/// </summary>
public class ScriptConditional : ScriptParameter
{

    #region Constructors
    public ScriptConditional(ScriptConditionType condType, object value, string hint = "") : base(value, hint)
    {
        SetCondition(condType);
    }
    #endregion

    #region Private Fields
    private ScriptConditionType _condition = ScriptConditionType.Equal;
    #endregion

    #region Properties
    /// <summary>
    /// The parameter's input type.
    /// </summary>
    /// <returns>
    /// An enumeration representing the parameter's input type.
    /// </returns>
    public override ScriptParameterInputType InputType 
    {
        get => ScriptParameterInputType.Conditional;
    }

    /// <summary>
    /// The parameter's conditional type.
    /// </summary>
    /// <returns>
    /// An enumeration representing the parameter's conditional type.
    /// </returns>
    public ScriptConditionType Condition
    {
        get => _condition;
        protected set => SetCondition(value);
    }
    #endregion

    #region Methods: Utility
    private bool EvaluateString(string targetValue, string inputValue)
    {
        return Condition switch
        {
            ScriptConditionType.Equal => targetValue.Equals(inputValue, StringComparison.OrdinalIgnoreCase),
            ScriptConditionType.NotEqual => !targetValue.Equals(inputValue, StringComparison.OrdinalIgnoreCase),

            ScriptConditionType.Contains => targetValue.Contains(inputValue, StringComparison.OrdinalIgnoreCase),
            ScriptConditionType.NotContains => !targetValue.Contains(inputValue, StringComparison.OrdinalIgnoreCase),
            ScriptConditionType.StartsWith => targetValue.StartsWith(inputValue, StringComparison.OrdinalIgnoreCase),
            ScriptConditionType.EndsWith => targetValue.EndsWith(inputValue, StringComparison.OrdinalIgnoreCase),

            _ => false
        };
    }

    private bool EvaluateInteger(int targetValue, int inputValue)
    {
        return Condition switch
        {
            ScriptConditionType.Equal => targetValue == inputValue,
            ScriptConditionType.NotEqual => targetValue != inputValue,

            ScriptConditionType.LessThan => targetValue < inputValue,
            ScriptConditionType.LessThanOrEqual => targetValue <= inputValue,
            ScriptConditionType.MoreThan => targetValue > inputValue,
            ScriptConditionType.MoreThanOrEqual => targetValue >= inputValue,

            _ => false
        };
    }

    private bool EvaluateDouble(double targetValue, double inputValue)
    {
        return Condition switch
        {
            ScriptConditionType.Equal => targetValue == inputValue,
            ScriptConditionType.NotEqual => targetValue != inputValue,

            ScriptConditionType.LessThan => targetValue < inputValue,
            ScriptConditionType.LessThanOrEqual => targetValue <= inputValue,
            ScriptConditionType.MoreThan => targetValue > inputValue,
            ScriptConditionType.MoreThanOrEqual => targetValue >= inputValue,

            _ => false
        };
    }

    private bool EvaluateBool(bool targetValue, bool inputValue)
    {
        return Condition switch
        {
            ScriptConditionType.Equal => targetValue == inputValue,
            ScriptConditionType.NotEqual => targetValue != inputValue,

            _ => false
        };
    }
    #endregion

    #region Methods
    public void SetCondition(ScriptConditionType conditionType)
    {
        Condition = conditionType;
    }

    public void SetCondition(string conditionType)
    {
        Condition = ScriptMachineParser.GetConditionType(conditionType);
    }

    public bool IsTrue(object value)
    {
        switch (value)
        {
            case int intValue:
                return EvaluateInteger(intValue, Integer);
            case double doubleValue:
                return EvaluateDouble(doubleValue, Double);
            case bool boolValue:
                return EvaluateBool(boolValue, Boolean);

            case string stringValue:
                if (int.TryParse(stringValue, out int parsedInt))
                {
                    return EvaluateInteger(parsedInt, Integer);
                }
                else if (double.TryParse(stringValue, out double parsedDouble))
                {
                    return EvaluateDouble(parsedDouble, Double);
                }
                else if (bool.TryParse(stringValue, out bool parsedBool))
                {
                    return EvaluateBool(parsedBool, Boolean);
                }
                return EvaluateString(stringValue, String);

            default:
                return false;
        }
    }

    public bool IsTrue(ScriptParameter valueParameter)
    {
        switch (valueParameter.Value)
        {
            case int intValue:
                return EvaluateInteger(intValue, Integer);
            case double doubleValue:
                return EvaluateDouble(doubleValue, Double);
            case bool boolValue:
                return EvaluateBool(boolValue, Boolean);

            case string stringValue:
                if (int.TryParse(stringValue, out int parsedInt))
                {
                    return EvaluateInteger(parsedInt, Integer);
                }
                else if (double.TryParse(stringValue, out double parsedDouble))
                {
                    return EvaluateDouble(parsedDouble, Double);
                }
                else if (bool.TryParse(stringValue, out bool parsedBool))
                {
                    return EvaluateBool(parsedBool, Boolean);
                }
                return EvaluateString(stringValue, String);

            default:
                return false;
        }
    }

    public bool IsTrue(object value, bool reverse)
    {
        return !reverse ? IsTrue(value) : !IsTrue(value);
    }

    public bool IsTrue(ScriptParameter valueParameter, bool reverse)
    {
        return !reverse ? IsTrue(valueParameter.Value) : !IsTrue(valueParameter.Value);
    }
    #endregion

}
