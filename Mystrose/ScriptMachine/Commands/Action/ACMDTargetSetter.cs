using Mystrose.ScriptMachine.Objects;
using Mystrose.ScriptMachine.Enumerations;
using System;
using System.Threading.Tasks;
using Mystrose.ScriptMachine.Inputs;
using System.Collections.Generic;
using Mystrose.GameModels.General;
using Mystrose.GameModels.Environment;
using System.Text.Json;

namespace Mystrose.ScriptMachine.Commands.Action;

public class ACMDTargetSetter : SCMDAction
{

    #region Constructor
    public ACMDTargetSetter() : base(ScriptCommandType.Action, "ACMD02", "Target Setter", "A script command that sets the main target to the corresponding entity.")
    {
        Parameters = new()
        {
            ["Target Type"] = new ScriptOptions("Self / Player / Monster / Random / No Target", "The type of the primary target to set on")  
        };
        SecondaryParameters = [];
    }
    #endregion

    #region Methods: Override
    public override ScriptCommand Clone()
    {
        return new ACMDTargetSetter()
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
            "Self" => [],
            "Player" => new()
            {
                ["Player Name"] = new ScriptParameter("", "The name of the player to set as the target")
            },
            "Monster" => new()
            {
                ["Tag"] = new ScriptParameter("", "The name of the monster (random), or the Monster Map ID of it (specific), to set as the target")
            },
            "Random" => new()
            {
                ["Focus Type"] = new ScriptOptions("Player / Monster", "The type of entity to focus on")
            },
            "No Target" => []
        };

        return SecondaryParameters[key];
    }

    public override async Task Execute(ScriptEngine engine)
    {
        bool isSuccess = true;

        switch (Parameters["Target Type"].String)
        {
            case "Self":
                engine.Flash.CallGameFunctionOnFunc("world.setTarget", "world.getAvatarByUserName", engine.Master.Name);
                break;
            case "Player":
                engine.Flash.CallGameFunctionOnFunc("world.setTarget", "world.getAvatarByUserName", SecondaryParameters["Player"]["Player Name"].GetVar(engine).String);
                break;
            case "Monster":
                if (SecondaryParameters["Monster"]["Tag"].Type == ScriptValueType.String)
                {
                    Random monRandom = new();
                    int monIndex = 0;

                    Monster[] monsters = [.. engine.Area.Monsters.FindAll(
                        (m) =>
                        {
                            MonsterFormat? monsterFormat = engine.Area.Format.MonsterFormats.Find(mf => mf.Name.Equals(SecondaryParameters["Monster"]["Tag"].GetVar(engine).String, StringComparison.OrdinalIgnoreCase));

                            if (monsterFormat is null)
                            {
                                return false;
                            }

                            return monsterFormat.ID == m.ID && m.Cell == engine.Master.Cell;
                        })];

                    if (monsters.Length == 0)
                    {
                        isSuccess = false;
                        break;
                    }

                    monIndex = monRandom.Next(0, monsters.Length);
                    engine.Flash.CallGameFunctionOnFunc("world.setTarget", "world.getMonster", monsters[monIndex].MonMapID);
                }
                else if (SecondaryParameters["Monster"]["Tag"].Type == ScriptValueType.Integer)
                {
                    engine.Flash.CallGameFunctionOnFunc("world.setTarget", "world.getMonster", SecondaryParameters["Monster"]["Tag"].GetVar(engine).Integer);
                }
                break;
            case "Random":
                Random allRandom = new();
                int allIndex = 0;

                if (SecondaryParameters["Random"]["Focus Type"].String == "Player")
                {
                    Avatar[] allPlayers = [.. engine.Area.Players.FindAll(
                        (p) =>
                        {
                            return p.Name != engine.Master.Name && p.Cell == engine.Master.Cell;
                        })];

                    if (allPlayers.Length == 0)
                    {
                        isSuccess = false;
                        break;
                    }

                    allIndex = allRandom.Next(0, allPlayers.Length);
                    engine.Flash.CallGameFunctionOnFunc("world.setTarget", "world.getAvatarByUserName", allPlayers[allIndex].Name);
                }
                else if (SecondaryParameters["Random"]["Focus Type"].String == "Monster")
                {
                    Monster[] allMonsters = [.. engine.Area.Monsters.FindAll(
                        (m) =>
                        {
                            return m.Cell == engine.Master.Cell;
                        })];

                    if (allMonsters.Length == 0)
                    {
                        isSuccess = false;
                        break;
                    }

                    allIndex = allRandom.Next(0, allMonsters.Length);
                    engine.Flash.CallGameFunctionOnFunc("world.setTarget", "world.getMonster", allMonsters[allIndex].MonMapID);
                }
                break;
            case "No Target":
                engine.Flash.CallGameFunctionOnFunc("world.setTarget", "world.getMonster", "");
                break;
        }

        EndResult = ScriptResultType.Success;
    }

    public override string ToString()
    {
        return Parameters["Target Type"].String switch
        {
            "Self" => "Set target to Self",
            "Player" => "Set target to Player " + SecondaryParameters["Player"]["Player Name"].String,
            "Monster" => "Set target to Monster " + SecondaryParameters["Monster"]["Tag"].String,
            "Random" => $"Set target to Random ({SecondaryParameters["Random"]["Focus Type"].String})",
            "No Target" => "Set target to None"
        };
    }
    #endregion

}
