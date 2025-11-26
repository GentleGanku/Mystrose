namespace Mystrose.Core.ScriptMachine.Codelines.Others;

public class SCLTrigger : ScriptCodeline, IConditional, IStackable
{

    #region Fields
    public Dictionary<string, ScriptParameter> Mandatories
    {
        get => Parameters
            .Where(kvp => kvp.Key.StartsWith(ScriptMachineParser.ADDITIONAL_PREFIX + ScriptMachineParser.MANDATORY_PREFIX))
            .ToDictionary(
                kvp => ScriptMachineParser.SanitizeLabel(kvp.Key),
                kvp => kvp.Value
            );
    }

    public Dictionary<string, ScriptParameter> Optionals
    {
        get => Parameters
            .Where(kvp => kvp.Key.StartsWith(ScriptMachineParser.ADDITIONAL_PREFIX + ScriptMachineParser.OPTIONAL_PREFIX))
            .ToDictionary(
                kvp => ScriptMachineParser.SanitizeLabel(kvp.Key),
                kvp => kvp.Value
            );
    }
    #endregion

    #region Properties: Attributes
    public override string ID
    {
        get => "SCL.401";
    }

    public override ScriptCodelineType Type
    {
        get => ScriptCodelineType.Trigger;
    }

    public override string Name
    {
        get => "Trigger";
    }

    public override string Description
    {
        get => "Script codeline that executes a set of internal commands within its scope " +
            "when conditions are met on an event trigger. " +
            "Any commands, except Triggers, Variables, and Options, can be added and executed in this scope.";
    }
    #endregion

    #region Properties: I/O
    public virtual ScriptCodeline[] InternalCommands
    {
        get;
        init;
    } = [];
    #endregion

    #region Methods: Validation
    public virtual string ValidateCodelineToBeAdded(ScriptCodeline cdl)
    {
        return (cdl.Type is not ScriptCodelineType.Trigger &&
            cdl.Type is not ScriptCodelineType.Variable &&
            cdl.Type is not ScriptCodelineType.Option) ?
            "Failed in adding the codeline to the set. Avoid adding codelines such as Triggers, Variables, and Options." :
            "";
    }

    public virtual bool ValidateCondition(ScriptEngine engine, ScriptEntityModelType sourceType, Dictionary<string, ScriptParameter> sourceParameters)
    {
        if (!Validate(engine))
        {
            return false;
        }

        string sourceTypeString = JSONParser.Serialize(sourceType);
        string targetType = Regulars["Target Type"].String;

        if (!sourceTypeString.Equals(targetType))
        {
            return false;
        }

        bool isConditionTrue = false;

        foreach (var prm in Additionals)
        {
            if (!Validate(engine))
            {
                return false;
            }

            ScriptParameter var = prm.Value.GetVariable(engine);
            ScriptConditional cdt = new(((ScriptConditional)prm.Value).Condition, var.Value);
            isConditionTrue = cdt.IsTrue(sourceParameters[prm.Key].Value, Conditionals["Reversal"].Boolean);

            if (!isConditionTrue)
            {
                return false;
            }
        }

        Execute(engine);
        return true;
    }
    #endregion

    #region Methods: Override
    public override ScriptCodeline Clone()
    {
        return new SCLTrigger()
        {
            Parameters = ScriptMachineParser.CloneToParameters(Parameters)
        };
    }

    public override void LoadRegulars()
    {
        string[] modelTypes = [.. Enum.GetValues<ScriptEntityModelType>()
            .Select(mt => JSONParser.Serialize(mt))];
        Dictionary<string, ScriptParameter> regulars = new()
        {
            ["Target Type"] = new ScriptParameter(string.Join('/', modelTypes), "Type of entity model to be used as the trigger source")
        };

        Parameters = Parameters.Concat(regulars)
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value);
    }

    public override Dictionary<string, ScriptParameter> LoadAdditionals()
    {
        return ScriptMachineParser.GetConditionalsByModel(Regulars["Target Type"].String);
    }

    public override async Task Execute(ScriptEngine engine)
    {
        if (!Validate(engine))
        {
            return;
        }

        engine.StateCodelineToBe(ScriptCodelineStatusType.Executing, this);

        foreach (ScriptCodeline cmd in InternalCommands)
        {
            if (!Validate(engine))
            {
                return;
            }

            if (cmd.Innates["Is Enabled"].Boolean is false)
            {
                continue;
            }

            try
            {
                cmd.Status = ScriptCodelineStatusType.Standby;
                await cmd.Execute(engine);
            }
            catch (Exception ex)
            {
                engine.StateCodelineToBe(ScriptCodelineStatusType.Canceled, this);
                engine.StateEngineToBe(ScriptEngineStatusType.Crash, ex);

                return;
            }
        }

        engine.StateCodelineToBe(ScriptCodelineStatusType.Succeed, this);
    }

    public override async Task Cancel(ScriptEngine engine)
    {
        return;
    }

    public override string ToString()
    {
        string label = Innates["Label"].String;

        return string.IsNullOrEmpty(label) ? "<Trigger>" : ("<" + label + ">");
    }
    #endregion

}

