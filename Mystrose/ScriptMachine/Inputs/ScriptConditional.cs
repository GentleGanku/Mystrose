using Mystrose.ScriptMachine.Enumerations;
using System;

namespace Mystrose.ScriptMachine.Inputs;

/// <summary>
/// An inheritor class that represents a conditional statement object.
/// </summary>
public class ScriptConditional : ScriptParameter
{

    #region Constructors
    public ScriptConditional()
    {
        // Empty constructor for serialization and deserialization.
    }

    public ScriptConditional(ScriptConditionalType condType) : base()
    {
        InputType = ScriptParameterInputType.Conditional;
        SetCondition(condType);
    }

    public ScriptConditional(ScriptConditionalType condType, object value, string? hint = null) : base(value, hint)
    {
        InputType = ScriptParameterInputType.Conditional;
        SetCondition(condType);
    }
    #endregion

    #region Private Fields
    private ScriptConditionalType? _condition;
    #endregion

    #region Properties
    /// <summary>
    /// The parameter's conditional type.
    /// </summary>
    /// <returns>
    /// An enumeration representing the parameter's conditional type.
    /// </returns>
    public ScriptConditionalType? Condition
    {
        get => _condition;
        set
        {
            _condition = value;
        }
    }
    #endregion

    #region Private Functions
    private bool EvaluateString(string value, ScriptParameter target)
    {
        return Condition switch
        {
            ScriptConditionalType.Equal => value.Equals(target.String, StringComparison.OrdinalIgnoreCase),
            ScriptConditionalType.NotEqual => !value.Equals(target.String, StringComparison.OrdinalIgnoreCase),
            ScriptConditionalType.Include => value.Contains(target.String, StringComparison.OrdinalIgnoreCase),
            ScriptConditionalType.Exclude => !value.Contains(target.String, StringComparison.OrdinalIgnoreCase),
            _ => false
        };
    }

    private bool EvaluateInteger(int value, ScriptParameter target)
    {
        return Condition switch
        {
            ScriptConditionalType.Equal => value == target.Integer,
            ScriptConditionalType.NotEqual => value != target.Integer,
            ScriptConditionalType.LessThanOrEqual => value <= target.Integer,
            ScriptConditionalType.LessThan => value < target.Integer,
            ScriptConditionalType.MoreThanOrEqual => value >= target.Integer,
            ScriptConditionalType.MoreThan => value > target.Integer,
            _ => false
        };
    }

    private bool EvaluateDouble(double value, ScriptParameter target)
    {
        return Condition switch
        {
            ScriptConditionalType.Equal => value == target.Double,
            ScriptConditionalType.NotEqual => value != target.Double,
            ScriptConditionalType.LessThanOrEqual => value <= target.Double,
            ScriptConditionalType.LessThan => value < target.Double,
            ScriptConditionalType.MoreThanOrEqual => value >= target.Double,
            ScriptConditionalType.MoreThan => value > target.Double,
            _ => false
        };
    }

    private bool EvaluateBool(bool value, ScriptParameter target)
    {
        return Condition switch
        {
            ScriptConditionalType.Equal => value == target.Boolean,
            ScriptConditionalType.NotEqual => value != target.Boolean,
            _ => false
        };
    }
    #endregion

    #region Methods
    public void SetCondition(ScriptConditionalType condType)
    {
        Condition = condType;
    }

    public void SetCondition(string condType)
    {
        Condition = ScriptRepository.GetCondition(condType);
    }

    public void Empty()
    {
        Condition = null;

        base.Empty();
    }

    public bool IsTrue(object value, ScriptParameter? alternative = null)
    {
        ScriptParameter target = alternative ?? this;

        switch (value)
        {
            case int intValue:
                return EvaluateInteger(intValue, target);
            case double doubleValue:
                return EvaluateDouble(doubleValue, target);
            case bool boolValue:
                return EvaluateBool(boolValue, target);

            case string stringValue:
                if (int.TryParse(value.ToString(), out int parsedInt))
                {
                    return EvaluateInteger(parsedInt, target);
                }
                else if (double.TryParse(value.ToString(), out double parsedDouble))
                {
                    return EvaluateDouble(parsedDouble, target);
                }
                else if (bool.TryParse(value.ToString(), out bool parsedBool))
                {
                    return EvaluateBool(parsedBool, target);
                }
                return EvaluateString(stringValue, target);

            default:
                return false;
        }
    }

    public bool IsTrue(object value, bool reverse, ScriptParameter? alternative = null)
    {
        return reverse ? !IsTrue(value, alternative) : IsTrue(value, alternative);
    }
    #endregion

}
