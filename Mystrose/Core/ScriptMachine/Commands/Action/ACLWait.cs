namespace Mystrose.Core.ScriptMachine.Codelines.Action;

public class ACLWait : SCLAction
{

    #region Properties: Attributes
    public override string ID
    {
        get => "SCL.005";
    }

    public override ScriptCodelineType Type
    {
        get => ScriptCodelineType.Action;
    }

    public override string Name
    {
        get => "Wait";
    }

    public override string Description
    {
        get => "Script codeline that forces the script's next execution to be delayed for a time frame.";
    }
    #endregion

    #region Methods: Override
    public override ScriptCodeline Clone()
    {
        return new ACLWait()
        {
            Parameters = ScriptMachineParser.CloneToParameters(Parameters)
        };
    }

    public override void LoadRegulars()
    {
        Dictionary<string, ScriptParameter> regulars = new()
        {
            ["Wait Type"] = new ScriptOptions("Timeout"/*"Timeout / Condition"*/, "Type of wait to be executed"),
            ["Delay Time"] = new ScriptParameter(1000, "Time to wait for, in milliseconds (1 second equals 1000 milliseconds)")
        };
        Parameters = Parameters.Concat(regulars)
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value);
    }

    public override Dictionary<string, ScriptParameter> LoadAdditionals()
    {
        Dictionary<string, ScriptParameter> additionals = Regulars["Wait Type"].String switch
        {
            "Timeout" => [],
            "Condition" => new()
            {
                ["Condition Type"] = new ScriptOptions("Variable/Rule", "Type of condition to be checked"),
                ["Variable Name"] = new ScriptParameter("", "Name of the variable to be checked"),
                ["Rule Name"] = new ScriptParameter("", "Name of the rule to be checked")
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

        int delayTime = Parameters["Delay Time"].GetVariable(engine).Integer;

        switch (Parameters["Wait Type"].String)
        {
            case "Timeout":
                await Task.Delay(delayTime);
                break;
            case "Condition":
                // WIP
                break;
        }

        engine.StateCodelineToBe(ScriptCodelineStatusType.Succeed, this);
    }

    public override async Task Cancel(ScriptEngine engine)
    {
        // TODO: Implement cancellation logic if needed
        return;
    }

    public override string ToString()
    {
        return Parameters["Wait Type"].String switch
        {
            "Timeout" => "Wait for " + Parameters["Delay Time"] + " millisecond(s) of Timeout",
            "Condition" => "Wait for the Condition in " + Parameters["Delay Time"] + " millisecond(s) interval"
        };
    }
    #endregion

}
