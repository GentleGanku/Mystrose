using Mystrose.ScriptMachine.Enumerations;
using Mystrose.ScriptMachine.Inputs;
using Mystrose.ScriptMachine.Objects;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Mystrose.ScriptMachine.Commands.Action;

public class ACMDRest : SCMDAction
{

    #region Constructor
    public ACMDRest() : base(ScriptCommandType.Action, "ACMD07", "Rest", "A script command that forces an in-game rest, depending on the Mode value.")
    {
        Parameters = new()
        {
            ["Mode"] = new ScriptOptions("Regular Rest / Conditional Rest", "The mode of resting to execute"),
            ["Safe"] = new ScriptParameter(false, "Whether to move to a safe cell before resting")
        };
        SecondaryParameters = [];
    }
    #endregion

    #region Methods: Override
    public override ScriptCommand Clone()
    {
        return new ACMDRest()
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
            "Regular Rest" => [],
            "Conditional Rest" => new()
            {
                ["Minimum HP (%)"] = new ScriptParameter(-1.0, "Minimum HP percentage to start resting at"),
                ["Minimum MP (%)"] = new ScriptParameter(-1.0, "Minimum MP percentage to start resting at"),
                ["Maximum HP (%)"] = new ScriptParameter(-1.0, "Maximum HP percentage to stop resting at"),
                ["Maximum MP (%)"] = new ScriptParameter(-1.0, "Maximum MP percentage to stop resting at"),
                ["Skill Indexes"] = new ScriptParameter("0, 1", "Indexes of skills to wait for while resting (0, ..., 5)")
            }
        };

        return SecondaryParameters[key];
    }

    public override async Task Execute(ScriptEngine engine)
    {
        if (Parameters["Safe"].GetVar(engine).Boolean)
        {
            engine.Flash.CallGameFunction("world.exitCombat");
            engine.Flash.CallGameFunction("world.moveToCell", engine.Master.Cell, engine.Master.Pad);
        }

        switch (Parameters["Mode"].String)
        {
            case "Regular Rest":
                engine.Flash.CallGameFunction("world.rest");
                break;
            case "Conditional Rest":
                int restChecks = 0;
                
                if (SecondaryParameters["Conditional Rest"]["Minimum HP (%)"].Double >= 0.0)
                {
                    restChecks -= engine.Master.HP <= engine.Master.MaxHP * (SecondaryParameters["Conditional Rest"]["Minimum HP (%)"].Double / 100.0) ? 0 : 1;
                }

                if (SecondaryParameters["Conditional Rest"]["Minimum MP (%)"].Double >= 0.0)
                {
                    restChecks -= engine.Master.MP <= engine.Master.MaxMP * (SecondaryParameters["Conditional Rest"]["Minimum MP (%)"].Double / 100.0) ? 0 : 1;
                }

                if (restChecks < 0)
                {
                    EndResult = ScriptResultType.Success;
                    return;
                }

                engine.Flash.CallGameFunction("world.rest");

                await engine.WaitForCondition(
                    () =>
                    {
                        if (SecondaryParameters["Conditional Rest"]["Skill Indexes"].String != "")
                        {
                            int[] indexes = Array.ConvertAll(SecondaryParameters["Conditional Rest"]["Skill Indexes"].String.Split(", "), int.Parse);

                            foreach (int index in indexes)
                            {
                                if (engine.Skills[index].Cooldown > 0)
                                {
                                    EndResult = ScriptResultType.Success;
                                    return false;
                                }
                            }
                        }

                        int restChecks = 0;

                        if (SecondaryParameters["Conditional Rest"]["Maximum HP (%)"].Double >= 0.0)
                        {
                            restChecks -= engine.Master.HP >= engine.Master.MaxHP * (SecondaryParameters["Conditional Rest"]["Maximum HP (%)"].Double / 100.0) ? 0 : 1;
                        }

                        if (SecondaryParameters["Conditional Rest"]["Maximum MP (%)"].Double >= 0.0)
                        {
                            restChecks -= engine.Master.MP >= engine.Master.MaxMP * (SecondaryParameters["Conditional Rest"]["Maximum MP (%)"].Double / 100.0) ? 0 : 1;
                        }

                        if (restChecks < 0)
                        {
                            EndResult = ScriptResultType.Success;
                            return false;
                        }

                        return true;
                    }, 60);
                break;
        }

        EndResult = ScriptResultType.Success;
    }

    public override string ToString()
    {
        return Parameters["Mode"].String switch
        {
            "Regular Rest" => "Rest",
            "Conditional Rest" => "Rest Conditionally"
        };
    }
    #endregion

}
