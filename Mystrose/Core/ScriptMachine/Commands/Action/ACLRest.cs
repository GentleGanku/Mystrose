namespace Mystrose.Core.ScriptMachine.Codelines.Action;

public class ACLRest : SCLAction
{

    #region Properties: Attributes
    public override string ID
    {
        get => "SCL.007";
    }

    public override ScriptCodelineType Type
    {
        get => ScriptCodelineType.Action;
    }

    public override string Name
    {
        get => "Rest";
    }

    public override string Description
    {
        get => "Script codeline that forces an in-game rest.";
    }
    #endregion

    #region Methods: Override
    public override ScriptCodeline Clone()
    {
        return new ACLRest()
        {
            Parameters = ScriptMachineParser.CloneToParameters(Parameters)
        };
    }

    public override void LoadRegulars()
    {
        Dictionary<string, ScriptParameter> regulars = new()
        {
            ["Rest Type"] = new ScriptOptions("Regular Rest/Conditional Rest", "Type of resting to be executed"),
            ["Safe Cell"] = new ScriptParameter(false, "Indicator of Whether to move to a safe cell before resting")
        };
        Parameters = Parameters.Concat(regulars)
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value);
    }

    public override Dictionary<string, ScriptParameter> LoadAdditionals()
    {
        Dictionary<string, ScriptParameter> additionals = Regulars["Rest Type"].String switch
        {
            "Regular Rest" => [],
            "Conditional Rest" => new()
            {
                ["Minimum HP (%)"] = new ScriptParameter(-1.0, "Minimum HP percentage to start resting at"),
                ["Minimum MP (%)"] = new ScriptParameter(-1.0, "Minimum MP percentage to start resting at"),
                ["Maximum HP (%)"] = new ScriptParameter(-1.0, "Maximum HP percentage to stop resting at"),
                ["Maximum MP (%)"] = new ScriptParameter(-1.0, "Maximum MP percentage to stop resting at"),
                ["Skill Indexes"] = new ScriptParameter("0, 1", "Indexes of skills to wait for while resting (0, ..., 5)")
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

        if (Parameters["Safe Cell"].GetVariable(engine).Boolean)
        {
            engine.FlashAPI.CallGameFunction("world.exitCombat");
            engine.FlashAPI.CallGameFunction("world.moveToCell", engine.Player.Cell, engine.Player.Pad);
        }

        switch (Parameters["Rest Type"].String)
        {
            case "Regular Rest":
                engine.FlashAPI.CallGameFunction("world.rest");
                break;
            case "Conditional Rest":
                int restChecks = 0;

                if (Additionals["Minimum HP (%)"].Double >= 0.0)
                {
                    restChecks -= engine.Player.HP <= engine.Player.MaxHP * (Additionals["Minimum HP (%)"].Double / 100.0) ? 0 : 1;
                }

                if (Additionals["Minimum MP (%)"].Double >= 0.0)
                {
                    restChecks -= engine.Player.MP <= engine.Player.MaxMP * (Additionals["Minimum MP (%)"].Double / 100.0) ? 0 : 1;
                }

                if (restChecks < 0)
                {
                    engine.StateCodelineToBe(ScriptCodelineStatusType.Failed, this);
                    return;
                }

                engine.FlashAPI.CallGameFunction("world.rest");

                await engine.WaitForCondition(
                    () =>
                    {
                        if (Additionals["Skill Indexes"].String != "")
                        {
                            int[] indexes = Array.ConvertAll(Additionals["Skill Indexes"].String.Split(", "), int.Parse);

                            foreach (int index in indexes)
                            {
                                if (engine.Skills[index].Cooldown > 0)
                                {
                                    return false;
                                }
                            }
                        }

                        int restChecks = 0;

                        if (Additionals["Maximum HP (%)"].Double >= 0.0)
                        {
                            restChecks -= engine.Player.HP >= engine.Player.MaxHP * (Additionals["Maximum HP (%)"].Double / 100.0) ? 0 : 1;
                        }

                        if (Additionals["Maximum MP (%)"].Double >= 0.0)
                        {
                            restChecks -= engine.Player.MP >= engine.Player.MaxMP * (Additionals["Maximum MP (%)"].Double / 100.0) ? 0 : 1;
                        }

                        if (restChecks < 0)
                        {
                            engine.StateCodelineToBe(ScriptCodelineStatusType.Failed, this);
                            return false;
                        }

                        return true;
                    }, 60);
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
        return Parameters["Rest Type"].String switch
        {
            "Regular Rest" => "Rest",
            "Conditional Rest" => "Rest conditionally"
        };
    }
    #endregion

}
