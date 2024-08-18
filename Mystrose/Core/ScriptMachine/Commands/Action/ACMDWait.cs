namespace Mystrose.Core.ScriptMachine.Commands.Action;

public class ACMDWait : SCMDAction
{

    #region Constructor
    public ACMDWait() : base(ScriptCommandType.Action, "ACMD05", "Wait", "A script command that forces the script's next execution to be delayed for a time frame, depending on the Wait type.")
    {
        Parameters = new()
        {
            ["Wait Type"] = new ScriptOptions("Timeout"/*"Timeout / Condition"*/, "The type of wait to execute"),
            ["Delay Time"] = new ScriptParameter(1000, "The time to wait for, in milliseconds (1 second equals 1000 milliseconds)")
        };
        SecondaryParameters = [];
    }
    #endregion

    #region Methods: Override
    public override ScriptCommand Clone()
    {
        return new ACMDWait()
        {
            Parameters = ScriptRepository.CloneToParameters(Parameters),
            SecondaryParameters = ScriptRepository.CloneToSecondaryParameters(SecondaryParameters),
            EndResult = JsonSerializer.Deserialize<ScriptResultType>(JsonSerializer.Serialize(EndResult))
        };
    }

    public override Dictionary<string, ScriptParameter> PassSecondaryParameters(string key)
    {
        if (SecondaryParameters.TryGetValue(key, out Dictionary<string, ScriptParameter>? value))
        {
            return value;
        }

        SecondaryParameters.Clear();
        SecondaryParameters[key] = key switch
        {
            "Timeout" => [],
            "Condition" => new()
            {
                ["Condition Type"] = new ScriptOptions("Variable / Rule", "The type of condition to check"),
                ["Variable Name"] = new ScriptParameter("", "The name of the variable to check"),
                ["Rule Name"] = new ScriptParameter("", "The name of the rule to check")
            }
        };

        return SecondaryParameters[key];
    }

    public override async Task Execute(ScriptEngine engine)
    {
        int delayTime = Parameters["Delay Time"].GetVar(engine).Integer;

        switch (Parameters["Wait Type"].String)
        {
            case "Timeout":
                await Task.Delay(delayTime);
                break;
            case "Condition":
                // WIP
                break;
        }

        EndResult = ScriptResultType.Success;
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
