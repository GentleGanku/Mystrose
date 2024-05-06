using Mystrose.ScriptMachine.Enumerations;
using System;
using System.Text.Json;

namespace Mystrose.ScriptMachine.Inputs;

/// <summary>
/// A base class that represents a conditional statement object.
/// </summary>
public class ScriptConditional : ScriptParameter
{

    #region Constructors
    public ScriptConditional(ScriptConditionalType condType) : base()
    {
        SetCondition(condType);
    }

    public ScriptConditional(ScriptConditionalType condType, object value, string? hint = null) : base(value, hint)
    {
        SetCondition(condType);
    }
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
        get;
        private set;
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
        base.Empty();

        Condition = null;
    }

    public bool IsTrue(object value, ScriptParameter? alternative = null)
    {
        ScriptParameter target = alternative ?? this;
        return value switch
        {
            string stringValue => EvaluateString(stringValue, target),
            int intValue => EvaluateInteger(intValue, target),
            double doubleValue => EvaluateDouble(doubleValue, target),
            bool boolValue => EvaluateBool(boolValue, target),
            _ => false
        };
    }

    public bool IsTrue(object value, bool reverse, ScriptParameter? alternative = null)
    {
        return reverse ? !IsTrue(value, alternative) : IsTrue(value, alternative);
    }

    //public bool IsTrue(object value, ScriptParameter? alternative = null)
    //{
    //    bool boolean = false;

    //    ScriptParameter target = alternative ?? this;

    //    switch (value)
    //    {
    //        case string stringValue:
    //            switch (Condition)
    //            {
    //                case ScriptConditionalType.Equal:
    //                    if (stringValue.Equals(target.String, StringComparison.OrdinalIgnoreCase))
    //                    {
    //                        boolean = true;
    //                    }    
    //                    break;
    //                case ScriptConditionalType.NotEqual:
    //                    if (!stringValue.Equals(target.String, StringComparison.OrdinalIgnoreCase))
    //                    {
    //                        boolean = true;
    //                    }
    //                    break;
    //            }
    //            break;
    //        case int intValue:
    //            switch (Condition)
    //            {
    //                case ScriptConditionalType.Equal:
    //                    if (intValue == target.Integer)
    //                    {
    //                        boolean = true;
    //                    }
    //                    break;
    //                case ScriptConditionalType.NotEqual:
    //                    if (intValue != target.Integer)
    //                    {
    //                        boolean = true;
    //                    }
    //                    break;
    //                case ScriptConditionalType.LessThan:
    //                    if (intValue < target.Integer)
    //                    {
    //                        boolean = true;
    //                    }
    //                    break;
    //                case ScriptConditionalType.MoreThan:
    //                    if (intValue > target.Integer)
    //                    {
    //                        boolean = true;
    //                    }
    //                    break;
    //            }
    //            break;
    //        case double doubleValue:
    //            switch (Condition)
    //            {
    //                case ScriptConditionalType.Equal:
    //                    if (doubleValue == target.Double)
    //                    {
    //                        boolean = true;
    //                    }
    //                    break;
    //                case ScriptConditionalType.NotEqual:
    //                    if (doubleValue != target.Double)
    //                    {
    //                        boolean = true;
    //                    }
    //                    break;
    //                case ScriptConditionalType.LessThan:
    //                    if (doubleValue < target.Double)
    //                    {
    //                        boolean = true;
    //                    }
    //                    break;
    //                case ScriptConditionalType.MoreThan:
    //                    if (doubleValue > target.Double)
    //                    {
    //                        boolean = true;
    //                    }
    //                    break;
    //            }
    //            break;
    //        case bool boolValue:
    //            switch (Condition)
    //            {
    //                case ScriptConditionalType.Equal:
    //                    if (boolValue == target.Boolean)
    //                    {
    //                        boolean = true;
    //                    }
    //                    break;
    //                case ScriptConditionalType.NotEqual:
    //                    if (boolValue != target.Boolean)
    //                    {
    //                        boolean = true;
    //                    }
    //                    break;
    //            }
    //            break;  
    //    }

    //    return boolean;
    //}
    #endregion

}
