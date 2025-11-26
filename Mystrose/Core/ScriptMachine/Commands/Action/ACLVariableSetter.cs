namespace Mystrose.Core.ScriptMachine.Codelines.Action;

public class ACLVariableSetter : SCLAction
{

    #region Properties: Attributes
    public override string ID
    {
        get => "SCL.004";
    }

    public override ScriptCodelineType Type
    {
        get => ScriptCodelineType.Action;
    }

    public override string Name
    {
        get => "Variable Setter";
    }

    public override string Description
    {
        get => "Script codeline that sets a Script Variable up.";
    }
    #endregion

    #region Methods: Override
    public override ScriptCodeline Clone()
    {
        return new ACLVariableSetter()
        {
            Parameters = ScriptMachineParser.CloneToParameters(Parameters)
        };
    }

    public override void LoadRegulars()
    {
        Dictionary<string, ScriptParameter> regulars = new()
        {
            ["Setting Type"] = new ScriptOptions("Add/Remove/Update", "Type of setting to be executed"),
            ["Variable Name"] = new ScriptParameter("", "Name of the variable to be set"),
        };
        Parameters = Parameters.Concat(regulars)
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value);
    }

    public override Dictionary<string, ScriptParameter> LoadAdditionals()
    {
        Dictionary<string, ScriptParameter> additionals = Regulars["Setting Type"].String switch
        {
            "Add" => new()
            {
                ["Variable Value"] = new ScriptParameter("", "Value to be set on the variable")
            },
            "Remove" => [],
            "Update" => new()
            {
                ["Operator Type"] = new ScriptOptions("=/+/-/*/:/%", "Type of operator to be applied"),
                ["Variable Value"] = new ScriptParameter("", "Value to be set on the variable")
            },
            _ => new()
        };
        Parameters = Parameters.Concat(additionals)
            .ToDictionary(
                kvp => ScriptMachineParser.ADDITIONAL_PREFIX + kvp.Key,
                kvp => kvp.Value);

        return Additionals;
    }

    public override async Task Execute(ScriptEngine engine)
    {
        if (!Validate(engine))
        {
            return;
        }

        engine.StateCodelineToBe(ScriptCodelineStatusType.Executing, this);

        ScriptKeyValuePair? scriptVar = Parameters["Setting Type"].String switch
        {
            "Add" => engine.ActiveLoadout.Variables.Add(Parameters["Variable Name"].String, Additionals["Variable Value"].Value),
            "Remove" => engine.CurrentLoadout.Variables.Remove(Parameters["Variable Name"].String),
            "Update" => engine.CurrentLoadout.Variables.Update((ScriptOptions)Additionals["Operator Type"], Parameters["Variable Name"].String, Additionals["Variable Value"].Value)
        };

        if (scriptVar is not null)
        {
            engine.Trigger(ScriptEntityModelType.ScriptVariable, [..
                    new("Key", scriptVar.Key),
                    new("Value", scriptVar.Value)
                ]);

            engine.StateCodelineToBe(ScriptCodelineStatusType.Succeed, this);
        }
        else
        {
            engine.StateCodelineToBe(ScriptCodelineStatusType.Failed, this);
        }
    }

    public override async Task Cancel(ScriptEngine engine)
    {
        // TODO: Implement cancellation logic if needed
        return;
    }

    public override string ToString()
    {
        return Parameters["Setting Type"].String switch
        {
            "Add" => $"Add variable '{Parameters["Variable Name"].String}' with value '{Additionals["Variable Value"].String}'",
            "Remove" => $"Remove variable '{Parameters["Variable Name"].String}'",
            "Update" => $"Update variable '{Parameters["Variable Name"].String}' with value '{Additionals["Variable Value"].String}' ({Additionals["Operator Type"].String})"
        };
    }
    #endregion

}
