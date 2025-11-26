namespace Mystrose.Core.ScriptMachine.Codelines.Others;

public class SCLStatement : ScriptCodeline, IConditional, IStackable
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
        get => "SCL.301";
    }

    public override ScriptCodelineType Type
    {
        get => ScriptCodelineType.Statement;
    }

    public override string Name
    {
        get => "Statement";
    }

    public override string Description
    {
        get => "Script codeline that executes a set of internal commands within its scope " +
            "when conditions are met. " +
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
        string sourceTypeString = JSONParser.Serialize(sourceType);
        Dictionary<string, ScriptParameter> targetParameters = ScriptMachineParser.GetConditionalsByEngine(sourceTypeString, engine, sourceParameters);

        if (!Validate(engine))
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
            isConditionTrue = cdt.IsTrue(targetParameters[prm.Key].Value, Conditionals["Reversal"].Boolean);

            if (!isConditionTrue)
            {
                return false;
            }
        }

        return true;
    }
    #endregion

    #region Methods: Override
    public override ScriptCodeline Clone()
    {
        return new SCLStatement()
        {
            Parameters = ScriptMachineParser.CloneToParameters(Parameters)
        };
    }

    public override void LoadRegulars()
    {
        string[] modelTypes = [.. Enum.GetValues<ScriptEntityModelType>()
            .Where(mt => mt < ScriptEntityModelType.CombatMessage &&
                mt is not ScriptEntityModelType.Cell &&
                mt is not ScriptEntityModelType.ShopItem)
            .Select(mt => JSONParser.Serialize(mt))];
        Dictionary<string, ScriptParameter> regulars = new()
        {
            ["Target Type"] = new ScriptParameter(string.Join('/', modelTypes), "Type of entity model to be used as the statement source")
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

        ScriptEntityModelType sourceType = JSONParser.Deserialize<ScriptEntityModelType>(Regulars["Target Type"].String);
        
        if (!ValidateCondition(engine, sourceType, Mandatories))
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
        string boolean = Conditionals["Reversal"].Boolean ? "false" : "true";

        return string.IsNullOrEmpty(label) ? ("If statement is " + boolean + ", then...") : label;
    }
    #endregion

}

