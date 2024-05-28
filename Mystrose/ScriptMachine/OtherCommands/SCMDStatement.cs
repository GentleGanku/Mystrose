using Mystrose.ScriptMachine.Enumerations;
using Mystrose.ScriptMachine.Interfaces;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Collections.Generic;
using Mystrose.ScriptMachine.Inputs;
using System;
using Mystrose.Utilities.Enumerations;
using Mystrose.ReadableModels.ScriptMachine;
using Mystrose.ReadableModels.General;
using Mystrose.ReadableModels.Environment;
using Mystrose.ReadableModels.Base;
using System.Linq;
using System.Text.Json.Serialization;

namespace Mystrose.ScriptMachine.Objects;

public class SCMDStatement : ScriptCommand, IStatementCommand
{

    #region Constructor
    public SCMDStatement() : base(ScriptCommandType.Statement, "SCMD03", "Statement", "A script command that executes an index jump, when its prerequisites are met (True - first next command; False - second next command). It executes otherwise if the Reverse Check is set to True.")
    {
        Parameters = new()
        {
            ["Statement Type"] = new ScriptOptions("Variable / Self / Player / Monster / Skill / Aura / Map / Faction / Quest / Item / Drop", "The type of the statement to be checked."),
            ["Reverse Check"] = new ScriptParameter(false, "If True, the command will execute if the statement is False.")
        };
        SecondaryParameters = [];
    }
    #endregion

    #region Properties
    [JsonIgnore]
    public ScriptStatementType? StatementType
    {
        get => Enum.TryParse(Parameters["Statement Type"].String.Replace(" ", ""), out ScriptStatementType type) ? type : null;
    }
    #endregion

    #region Methods: Override
    public override ScriptCommand Clone()
    {
        return new SCMDStatement()
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

        object? targetStatement = StatementType switch
        {
            ScriptStatementType.Variable => new RMScriptVariable(),
            ScriptStatementType.Self => new RMSelf(),
            ScriptStatementType.Player => new RMAvatar(),
            ScriptStatementType.Monster => new RMMonster(),
            ScriptStatementType.Skill => new RMActiveSkill(),
            ScriptStatementType.Aura => new RMAura(),
            ScriptStatementType.Map => new RMArea(),
            ScriptStatementType.Faction => new RMFaction(),
            ScriptStatementType.Quest => new RMQuest(),
            ScriptStatementType.Item => new RMInventoryItem(),
            ScriptStatementType.Drop => new RMItemDrop()
        };

        ReadableModel targetModel = (ReadableModel)targetStatement;

        SecondaryParameters.Clear();
        SecondaryParameters[key] = ScriptRepository.ConvertToConditionals(targetModel.MandatorySearchProperties);
        SecondaryParameters["Optional"] = ScriptRepository.ConvertToConditionals(targetStatement).Where(k => !SecondaryParameters[key].ContainsKey(k.Key)).ToDictionary();

        return SecondaryParameters[key];
    }   

    public override async Task Execute(ScriptEngine engine)
    {
        Dictionary<string, ScriptParameter> secondaryPrms = SecondaryParameters[Parameters["Statement Type"].String];
        object? target = StatementType switch
        {
            ScriptStatementType.Variable => new RMScriptVariable(engine.CurrentLoadout.Variables[secondaryPrms["Key"].String]),
            ScriptStatementType.Self => new RMSelf(engine.Master),
            ScriptStatementType.Player => new RMAvatar(engine.Area.Players.Find(p => p.Name.Equals(secondaryPrms["Name"].GetVar(engine).String, StringComparison.OrdinalIgnoreCase))),
            ScriptStatementType.Monster => new RMMonster(engine.Area.Monsters.Find(m => m.MonMapID == secondaryPrms["Monster Map ID"].GetVar(engine).Integer)),
            ScriptStatementType.Skill => new RMActiveSkill(engine.Skills.Find(s => s.Index == secondaryPrms["Index"].GetVar(engine).Integer)),
            ScriptStatementType.Aura => new RMAura(engine.World.Auras[JsonSerializer.Deserialize<EntityType>(secondaryPrms["Target Type"].GetVar(engine).String), secondaryPrms["Target ID"].GetVar(engine).String, secondaryPrms["Name"].GetVar(engine).String]),
            ScriptStatementType.Map => new RMArea(engine.Area),
            ScriptStatementType.Faction => new RMFaction(engine.Master.Factions.Find(f => f.Name.Equals(secondaryPrms["Name"].GetVar(engine).String, StringComparison.OrdinalIgnoreCase))),
            ScriptStatementType.Quest => new RMQuest(engine.Quests.Find(q => q.ID == secondaryPrms["ID"].GetVar(engine).Integer)),
            ScriptStatementType.Item => new RMInventoryItem(engine.World.MasterInventory[JsonSerializer.Deserialize<InventoryType>(secondaryPrms["Inventory Type"].GetVar(engine).String)][secondaryPrms["Name"].GetVar(engine).String]),
            ScriptStatementType.Drop => new RMItemDrop(engine.World.Drops.Find(d => d.Name.Equals(secondaryPrms["Name"].GetVar(engine).String, StringComparison.OrdinalIgnoreCase)))
        };

        if (target is null)
        {
            return;
        }

        JsonObject? jsonTarget = JsonSerializer.Deserialize<JsonObject>(JsonSerializer.Serialize(target));
        bool isConditionTrue = false;
        ScriptParameter reverseParameter = Parameters["Reverse Check"].GetVar(engine);

        foreach (KeyValuePair<string, ScriptParameter> parameter in secondaryPrms)
        {
            ScriptConditional condition = (ScriptConditional)parameter.Value;
            isConditionTrue = condition.IsTrue(jsonTarget?[parameter.Key].Deserialize<object>(), condition.GetVar(engine));

            if (isConditionTrue != !reverseParameter.Boolean)
            {
                break;
            }
        }

        if (reverseParameter.Boolean == true)
        {
            EndResult = isConditionTrue == false ? ScriptResultType.Success : ScriptResultType.Failure;
        }
        else
        {
            EndResult = isConditionTrue == true ? ScriptResultType.Success : ScriptResultType.Failure;
        }
    }

    public override string ToString()
    {
        return "If such statement is true: " + JsonSerializer.Serialize(StatementType);
    }
    #endregion

}

