namespace Mystrose.Core.ScriptMachine.Base.Records;

public class ScriptVariableCollection : Dictionary<string, ScriptKeyValuePair>
{

    #region Fields
    public ScriptKeyValuePair? this[ScriptKeyValuePair ScriptKeyValuePair]
    {
        get
        {
            if (TryGetValue(ScriptKeyValuePair.Key, out ScriptKeyValuePair? scriptVar))
            {
                return scriptVar;
            }

            return null;
        }
    }

    public new ScriptKeyValuePair? this[string key]
    {
        get
        {
            if (TryGetValue(key, out ScriptKeyValuePair? scriptVar))
            {
                return scriptVar;
            }

            return null;
        }
    }
    #endregion

    #region Methods: Addition
    public ScriptKeyValuePair Add(ScriptKeyValuePair scriptVar)
    {
        if (TryGetValue(scriptVar.Key, out ScriptKeyValuePair? existingScriptVar))
        {
            return existingScriptVar;
        }

        Add(scriptVar.Key, scriptVar);
        return scriptVar;
    }

    public ScriptKeyValuePair Add(ScriptParameter keyPar, ScriptParameter valuePar)
    {
        if (TryGetValue(keyPar.String, out ScriptKeyValuePair? existingScriptVar))
        {
            return existingScriptVar;
        }

        ScriptKeyValuePair scriptVar = new(keyPar.String, valuePar.Value);
        Add(keyPar.String, scriptVar);

        return scriptVar;
    }
    #endregion
    
    #region Methods: Removal
    public ScriptKeyValuePair? Remove(ScriptKeyValuePair scriptVar)
    {
        if (Remove(scriptVar.Key, out ScriptKeyValuePair? existingScriptVar))
        {
            return existingScriptVar;
        }

        return null;
    }

    public ScriptKeyValuePair? Remove(string key)
    {
        if (Remove(key, out ScriptKeyValuePair? existingScriptVar))
        {
            return existingScriptVar;
        }

        return null;
    }
    #endregion

    #region Methods: Updation
    public ScriptKeyValuePair? Update(ScriptOptions scriptOpts, ScriptParameter keyPar, ScriptParameter valuePar)
    {
        if (!TryGetValue(keyPar.String, out ScriptKeyValuePair? existingScriptVar))
        {
            return null;
        }

        ScriptOperatorType operatorType = JsonSerializer.Deserialize<ScriptOperatorType>(scriptOpts.String);

        switch (valuePar.ValueType)
        {
            case ScriptValueType.String:
                UpdateString(operatorType, keyPar.String, valuePar.String);
                break;
            case ScriptValueType.Integer:
                UpdateInteger(operatorType, keyPar.String, valuePar.Integer);
                break;
            case ScriptValueType.Double:
                UpdateDouble(operatorType, keyPar.String, valuePar.Double);
                break;
            case ScriptValueType.Boolean:
                UpdateBool(operatorType, keyPar.String, valuePar.Boolean);
                break;

            default:
                return null;
        };

        return existingScriptVar;
    }
    #endregion

    #region Methods: Clearance
    public Dictionary<string, ScriptKeyValuePair> Clear()
    {
        if (Count == 0)
        {
            return [];
        }

        Dictionary<string, ScriptKeyValuePair> clearedItems = new(this);
        base.Clear();

        return clearedItems;
    }
    #endregion

    #region Methods: Value Updation
    private bool UpdateString(ScriptOperatorType operatorType, string key, object value)
    {
        if (!TryGetValue(key, out ScriptKeyValuePair? scriptVar))
        {
            return false;
        }

        switch (operatorType)
        {
            case ScriptOperatorType.Assign:
                scriptVar.SetValue(value.ToString()!);
                break;

            default:
                return false;
        }

        return true;
    }

    private bool UpdateInteger(ScriptOperatorType operatorType, string key, object value)
    {
        if (!int.TryParse(value.ToString(), out int intValue) || !TryGetValue(key, out ScriptKeyValuePair? scriptVar))
        {
            return false;
        }

        switch (operatorType)
        {
            case ScriptOperatorType.Assign:
                scriptVar.SetValue(intValue);
                break;
            case ScriptOperatorType.Add:
                scriptVar.SetValue(scriptVar.Integer + intValue);
                break;
            case ScriptOperatorType.Subtract:
                scriptVar.SetValue(scriptVar.Integer - intValue);
                break;
            case ScriptOperatorType.Multiply:
                scriptVar.SetValue(scriptVar.Integer * intValue);
                break;
            case ScriptOperatorType.Divide:
                scriptVar.SetValue(scriptVar.Integer / intValue);
                break;
            case ScriptOperatorType.Modulo:
                scriptVar.SetValue(scriptVar.Integer % intValue);
                break;

            default:
                return false;
        }

        return true;
    }

    private bool UpdateDouble(ScriptOperatorType operatorType, string key, object value)
    {
        if (!double.TryParse(value.ToString(), out double doubleValue) || !TryGetValue(key, out ScriptKeyValuePair? scriptVar))
        {
            return false;
        }

        switch (operatorType)
        {
            case ScriptOperatorType.Assign:
                scriptVar.SetValue(doubleValue);
                break;
            case ScriptOperatorType.Add:
                scriptVar.SetValue(scriptVar.Double + doubleValue);
                break;
            case ScriptOperatorType.Subtract:
                scriptVar.SetValue(scriptVar.Double - doubleValue);
                break;
            case ScriptOperatorType.Multiply:
                scriptVar.SetValue(scriptVar.Double * doubleValue);
                break;
            case ScriptOperatorType.Divide:
                scriptVar.SetValue(scriptVar.Double / doubleValue);
                break;
            case ScriptOperatorType.Modulo:
                scriptVar.SetValue(scriptVar.Double % doubleValue);
                break;

            default:
                return false;
        }

        return true;
    }

    private bool UpdateBool(ScriptOperatorType operatorType, string key, object value)
    {
        if (!bool.TryParse(value.ToString(), out bool boolValue) || !TryGetValue(key, out ScriptKeyValuePair? scriptVar))
        {
            return false;
        }

        switch (operatorType)
        {
            case ScriptOperatorType.Assign:
                scriptVar.SetValue(boolValue);
                break;

            default:
                return false;
        }

        return true;
    }
    #endregion

}
