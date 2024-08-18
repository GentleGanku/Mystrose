namespace Mystrose.Core.ScriptMachine.Base.Dictionaries;

public class ScriptVariableDictionary : Dictionary<string, ScriptVariable>
{

    #region Constructor
    public ScriptVariableDictionary() : base()
    {
        // TODO: Nothing to do here.
    }
    #endregion

    #region Fields
    public new ScriptVariable? this[string key]
    {
        get => base[key];
    }

    public ScriptVariable? this[ScriptVariable scriptVariable]
    {
        get => base[scriptVariable.KeyValuePair.Key];
    }
    #endregion

    #region Methods: Add
    public bool Add(ScriptVariable variable)
    {
        if (ContainsKey(variable.KeyValuePair.Key))
        {
            return false;
        }

        Add(variable.KeyValuePair.Key, variable);
        return true;
    }

    public bool Add(ScriptParameter keyPrm, ScriptParameter valuePrm)
    {
        if (ContainsKey(keyPrm.String))
        {
            return false;
        }

        Add(keyPrm.String, new(keyPrm.String, valuePrm.Object));
        return true;
    }

    public bool Add(ScriptEngine engine, ScriptParameter keyPrm, ScriptParameter valuePrm)
    {
        if (ContainsKey(keyPrm.String))
        {
            return false;
        }

        ScriptVariable var = new(keyPrm.String, valuePrm.Object);

        Add(keyPrm.String, var);
        engine.InvokeTrigger(ScriptTriggerType.Variable, var.KeyValuePair);
        return true;
    }
    #endregion

    #region Methods: Remove
    public bool Remove(string key)
    {
        if (!ContainsKey(key))
        {
            return false;
        }

        base.Remove(key);
        return true;
    }

    public bool Remove(ScriptEngine engine, string key)
    {
        if (!ContainsKey(key))
        {
            return false;
        }

        ScriptVariable var = this[key];
        var.KeyValuePair.SetValue(null);

        base.Remove(key);
        engine.InvokeTrigger(ScriptTriggerType.Variable, var.KeyValuePair);
        return true;
    }
    #endregion

    #region Methods: Update
    public bool Update(ScriptParameter keyPrm, ScriptParameter valuePrm, ScriptOptions opt)
    {
        string key = keyPrm.String;

        if (!ContainsKey(key))
        {
            return false;
        }

        ScriptOperatorType optType = JsonSerializer.Deserialize<ScriptOperatorType>(opt.String);

        return valuePrm.Type switch
        {
            ScriptValueType.String => UpdateString(key, valuePrm.String, optType),
            ScriptValueType.Integer => UpdateInteger(key, valuePrm.Integer, optType),
            ScriptValueType.Double => UpdateDouble(key, valuePrm.Double, optType),
            ScriptValueType.Boolean => UpdateBool(key, valuePrm.Boolean, optType),
            _ => false,
        };
    }

    public bool Update(ScriptEngine engine, ScriptParameter keyPrm, ScriptParameter valuePrm, ScriptOptions opt)
    {
        string key = keyPrm.String;

        if (!ContainsKey(key))
        {
            return false;
        }

        ScriptOperatorType optType = JsonSerializer.Deserialize<ScriptOperatorType>(opt.String);

        switch (valuePrm.Type)
        {
            case ScriptValueType.String:
                UpdateString(key, valuePrm.String, optType);
                break;
            case ScriptValueType.Integer:
                UpdateInteger(key, valuePrm.Integer, optType);
                break;
            case ScriptValueType.Double:
                UpdateDouble(key, valuePrm.Double, optType);
                break;
            case ScriptValueType.Boolean:
                UpdateBool(key, valuePrm.Boolean, optType);
                break;
            default:
                return false;
        }

        ScriptVariable var = this[key];

        engine.InvokeTrigger(ScriptTriggerType.Variable, var.KeyValuePair);
        return true;
    }
    #endregion

    #region Private Methods: Update
    private bool UpdateString(string key, object value, ScriptOperatorType opt)
    {
        string str = value.ToString();
        switch (opt)
        {
            case ScriptOperatorType.Assignation:
                this[key].KeyValuePair.SetValue(str);
                break;
            default:
                return false;
        }
        return true;
    }

    private bool UpdateInteger(string key, object value, ScriptOperatorType opt)
    {
        if (!int.TryParse(value.ToString(), out int intValue))
        {
            return false;
        }

        switch (opt)
        {
            case ScriptOperatorType.Assignation:
                this[key].KeyValuePair.SetValue(intValue);
                break;
            case ScriptOperatorType.Addition:
                this[key].KeyValuePair.SetValue(this[key].KeyValuePair.Integer + intValue);
                break;
            case ScriptOperatorType.Subtraction:
                this[key].KeyValuePair.SetValue(this[key].KeyValuePair.Integer - intValue);
                break;
            case ScriptOperatorType.Multiplication:
                this[key].KeyValuePair.SetValue(this[key].KeyValuePair.Integer * intValue);
                break;
            case ScriptOperatorType.Division:
                this[key].KeyValuePair.SetValue(this[key].KeyValuePair.Integer / intValue);
                break;
            case ScriptOperatorType.Modulo:
                this[key].KeyValuePair.SetValue(this[key].KeyValuePair.Integer % intValue);
                break;
            default:
                return false;
        }
        return true;
    }

    private bool UpdateDouble(string key, object value, ScriptOperatorType opt)
    {
        if (!double.TryParse(value.ToString(), out double doubleValue))
        {
            return false;
        }

        switch (opt)
        {
            case ScriptOperatorType.Assignation:
                this[key].KeyValuePair.SetValue(doubleValue);
                break;
            case ScriptOperatorType.Addition:
                this[key].KeyValuePair.SetValue(this[key].KeyValuePair.Double + doubleValue);
                break;
            case ScriptOperatorType.Subtraction:
                this[key].KeyValuePair.SetValue(this[key].KeyValuePair.Double - doubleValue);
                break;
            case ScriptOperatorType.Multiplication:
                this[key].KeyValuePair.SetValue(this[key].KeyValuePair.Double * doubleValue);
                break;
            case ScriptOperatorType.Division:
                this[key].KeyValuePair.SetValue(this[key].KeyValuePair.Double / doubleValue);
                break;
            case ScriptOperatorType.Modulo:
                this[key].KeyValuePair.SetValue(this[key].KeyValuePair.Double % doubleValue);
                break;
            default:
                return false;
        }
        return true;
    }

    private bool UpdateBool(string key, object value, ScriptOperatorType opt)
    {
        if (!bool.TryParse(value.ToString(), out bool boolValue))
        {
            return false;
        }

        switch (opt)
        {
            case ScriptOperatorType.Assignation:
                this[key].KeyValuePair.SetValue(boolValue);
                break;
            default:
                return false;
        }
        return true;
    }
    #endregion

}
