namespace Mystrose.Core.ScriptMachine.Codelines.Action;

public class ACLSkillUse : SCLAction
{

    #region Properties: Attributes
    public override string ID
    {
        get => "SCL.006";
    }

    public override ScriptCodelineType Type
    {
        get => ScriptCodelineType.Action;
    }

    public override string Name
    {
        get => "Skill Use";
    }

    public override string Description
    {
        get => "Script codeline that uses an active skill in-game based on their index.";
    }
    #endregion

    #region Methods: Override
    public override ScriptCodeline Clone()
    {
        return new ACLSkillUse()
        {
            Parameters = ScriptMachineParser.CloneToParameters(Parameters)
        };
    }

    public override void LoadRegulars()
    {
        Dictionary<string, ScriptParameter> regulars = new()
        {
            ["Skill Index"] = new ScriptParameter(0, "Index of the skill to use"),
            ["Rule Type"] = new ScriptOptions("Non-Ruling/Ruling", "Type of rule to be applied when using the skill"),
        };
        Parameters = Parameters.Concat(regulars)
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value);
    }

    public override Dictionary<string, ScriptParameter> LoadAdditionals()
    {
        Dictionary<string, ScriptParameter> additionals = Regulars["Rule Type"].String switch
        {
            "Non-Ruling" => [],
            "Ruling" => new()
            {
                ["Minimum HP (%)"] = new ScriptParameter(-1.0, "Minimum HP percentage to use the skill at"),
                ["Minimum MP (%)"] = new ScriptParameter(-1.0, "Minimum MP percentage to use the skill at"),
                ["Wait"] = new ScriptParameter(false, "Indicator of whether to wait for the skill to go off cooldown")
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

        int index = Parameters["Skill Index"].GetVariable(engine).Integer;

        switch (Parameters["Rule Type"].String)
        {
            case "Non-Ruling":
                if (!engine.Skills[index].IsSafeToUse)
                {
                    engine.StateCodelineToBe(ScriptCodelineStatusType.Failed, this);
                    return;
                }
                break;
            case "Ruling":
                int safeChecks = 0;

                if (Additionals["Minimum HP (%)"].Double >= 0.0)
                {
                    safeChecks -= engine.Player.HP <= engine.Player.MaxHP * (Additionals["Minimum HP (%)"].Double / 100.0) ? 0 : 1;
                }

                if (Additionals["Minimum MP (%)"].Double >= 0.0)
                {
                    safeChecks -= engine.Player.MP <= engine.Player.MaxMP * (Additionals["Minimum MP (%)"].Double / 100.0) ? 0 : 1;
                }

                if (safeChecks < 0 || !engine.Skills[index].IsSafeToUse)
                {
                    engine.StateCodelineToBe(ScriptCodelineStatusType.Failed, this);
                    return;
                }

                if (Additionals["Wait"].GetVariable(engine).Boolean is true)
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
            engine.FlashAPI.CallGameFunction("world.approachTarget");
        }
        else
        {
            engine.FlashAPI.CallGameFunctionOnFunc("world.testAction", "world.getActionByRef", reference);
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
        return Parameters["Rule Type"].String switch
        {
            "Non-Ruling" => "Use skill at index " + Parameters["Skill Index"],
            "Ruling" => "(Ruled) Use skill at index " + Parameters["Skill Index"]
        };
    }
    #endregion

}
