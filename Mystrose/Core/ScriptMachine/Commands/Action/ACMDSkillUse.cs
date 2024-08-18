namespace Mystrose.Core.ScriptMachine.Commands.Action;

public class ACMDSkillUse : SCMDAction
{

    #region Constructor
    public ACMDSkillUse() : base(ScriptCommandType.Action, "ACMD06", "Skill Use", "A script command that uses an active skill in-game based on the Index value, depending on the Rules parameters.")
    {
        Parameters = new()
        {
            ["Skill Index"] = new ScriptParameter(0, "Index of the skill to use"),
            ["Rule Type"] = new ScriptOptions("Non-Ruling / Ruling", "The type of rule to use when using the skill"),
        };
        SecondaryParameters = [];
    }
    #endregion

    #region Methods: Override
    public override ScriptCommand Clone()
    {
        return new ACMDSkillUse()
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
            "Non-Ruling" => [],
            "Ruling" => new()
            {
                ["Minimum HP (%)"] = new ScriptParameter(-1.0, "Minimum HP percentage to use the skill at"),
                ["Minimum MP (%)"] = new ScriptParameter(-1.0, "Minimum MP percentage to use the skill at"),
                ["Wait"] = new ScriptParameter(false, "Whether to wait for the skill to go off cooldown")
            },
        };

        return SecondaryParameters[key];
    }

    public override async Task Execute(ScriptEngine engine)
    {
        int index = Parameters["Skill Index"].GetVar(engine).Integer;

        switch (Parameters["Rule Type"].String)
        {
            case "Non-Ruling":
                if (!engine.Skills[index].IsSafeToUse)
                {
                    EndResult = ScriptResultType.Failure;
                    return;
                }
                break;
            case "Ruling":
                int safeChecks = 0;

                if (SecondaryParameters["Ruling"]["Minimum HP (%)"].Double >= 0.0)
                {
                    safeChecks -= engine.Master.HP <= engine.Master.MaxHP * (SecondaryParameters["Ruling"]["Minimum HP (%)"].Double / 100.0) ? 0 : 1;
                }

                if (SecondaryParameters["Ruling"]["Minimum MP (%)"].Double >= 0.0)
                {
                    safeChecks -= engine.Master.MP <= engine.Master.MaxMP * (SecondaryParameters["Ruling"]["Minimum MP (%)"].Double / 100.0) ? 0 : 1;
                }

                if (safeChecks < 0 || !engine.Skills[index].IsSafeToUse)
                {
                    EndResult = ScriptResultType.Failure;
                    return;
                }

                if (SecondaryParameters["Ruling"]["Wait"].GetVar(engine).Boolean == true)
                {
                    await engine.WaitForCondition(
                        () =>
                        {
                            return engine.Skills[index].Cooldown == 0;
                        }, 40);
                }
                break;
        }

        string reference = index switch
        {
            0 - 4 => "a" + index,
            >= 5 => "i" + (index - 4),
        };

        if (reference == "a0")
        {
            engine.Flash.CallGameFunction("world.approachTarget");
        }
        else
        {
            engine.Flash.CallGameFunctionOnFunc("world.testAction", "world.getActionByRef", reference);
        }

        EndResult = ScriptResultType.Success;
    }

    public override string ToString()
    {
        return Parameters["Rule Type"].String switch
        {
            "Non-Ruling" => "Use skill at index " + Parameters["Skill Index"],
            "Ruling" => "(Ruled) Use skill at index " + Parameters["Skill Index"]
        };
    }
    #endregion

}
