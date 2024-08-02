using Mystrose.ScriptMachine.Enumerations;
using Mystrose.ScriptMachine.Interfaces;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Mystrose.ScriptMachine.Inputs;
using Mystrose.ReadableModels.General;
using Mystrose.ReadableModels.ScriptMachine;
using Mystrose.ReadableModels.Environment;
using Mystrose.ReadableModels.Base;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mystrose.ScriptMachine.Objects;

public class SCMDTrigger : ScriptCommand, ITriggerCommand, IStackable
{

    #region Constructor
    public SCMDTrigger() : base(ScriptCommandType.Trigger, "SCMD04", "Trigger", "A script command that executes a set of internal commands within its scope, when its prerequisites are met on an event trigger. It executes otherwise if the Reverse Check is set to True. Any kinds of commands, other than Trigger and Variable ones, are executable in this scope. Stacks up to 20 internal commands.")
    {
        Parameters = new()
        {
            ["Label Name"] = new ScriptParameter("Label", "The label name of the trigger to be used."),
            ["Trigger Type"] = new ScriptOptions("Variable / Self / Player / Monster / Skill / Aura / Map / Faction / Quest / Item / Drop / Combat Message / Event Message", "The type of the trigger to be checked."),
            ["Reverse Check"] = new ScriptParameter(false, "If True, the command will execute with the trigger being False."),
            ["Active"] = new ScriptParameter(true, "If False, the command will be disabled for event trigger. Otherwise, it's enabled.")
        };
        SecondaryParameters = [];
        InternalCommands = [];
    }
    #endregion

    #region Properties
    [JsonIgnore]
    public string LabelName
    {
        get => Parameters["Label Name"].ToString()!;
    }

    [JsonIgnore]
    public int StackLimit
    {
        get => 20;
    }

    public List<ScriptCommand> InternalCommands
    {
        get;
        set;
    }

    [JsonIgnore]
    public ScriptTriggerType? TriggerType
    {
        get => Enum.TryParse(Parameters["Trigger Type"].String.Replace(" ", ""), out ScriptTriggerType type) ? type : null;
    }

    [JsonIgnore]
    public bool IsReverseChecked
    {
        get => Parameters["Reverse Check"].Boolean;
    }

    [JsonIgnore]
    public bool IsEnabled
    {
        get => Parameters["Active"].Boolean;
    }
    #endregion

    #region Methods: Interface
    public bool IsInputValid(ScriptCommand cmd)
    {
        return cmd.Type != ScriptCommandType.Trigger && cmd.Type != ScriptCommandType.Variable && InternalCommands.Count <= StackLimit;
    }

    public bool IsValid(ScriptEngine engine, Dictionary<string, ScriptParameter> parameters)
    {
        bool isConditionTrue = false;

        foreach (KeyValuePair<string, ScriptParameter> parameter in SecondaryParameters[Parameters["Trigger Type"].String])
        {
            ScriptConditional condition = (ScriptConditional)parameter.Value;
            isConditionTrue = condition.IsTrue(parameters[parameter.Key].Object, condition.GetVar(engine));

            if (isConditionTrue != !IsReverseChecked)
            {
                break;
            }
        }

        return IsReverseChecked == true ? (isConditionTrue == false) : (isConditionTrue == true);
    }
    #endregion

    #region Methods: Override
    public override ScriptCommand Clone()
    {
        return new SCMDTrigger()
        {
            InternalCommands = ScriptRepository.CloneToCommandsList(InternalCommands),
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

        object? targetStatement = TriggerType switch
        {
            ScriptTriggerType.Variable => new RMScriptVariable(),
            ScriptTriggerType.Self => new RMSelf(),
            ScriptTriggerType.Player => new RMAvatar(),
            ScriptTriggerType.Monster => new RMMonster(),
            ScriptTriggerType.Skill => new RMActiveSkill(),
            ScriptTriggerType.Aura => new RMAura(),
            ScriptTriggerType.Map => new RMArea(),
            ScriptTriggerType.Faction => new RMFaction(),
            ScriptTriggerType.Quest => new RMQuest(),
            ScriptTriggerType.Item => new RMInventoryItem(),
            ScriptTriggerType.Drop => new RMItemDrop(),
            ScriptTriggerType.CombatMessage => new RMCombatMessage(),
            ScriptTriggerType.EventMessage => new RMEventMessage()
        };

        ReadableModel targetModel = (ReadableModel)targetStatement;

        SecondaryParameters.Clear();
        SecondaryParameters[key] = ScriptRepository.ConvertToConditionals(targetModel.MandatorySearchProperties);
        SecondaryParameters["Optional"] = ScriptRepository.ConvertToConditionals(targetStatement).Where(k => !SecondaryParameters[key].ContainsKey(k.Key)).ToDictionary();

        return SecondaryParameters[key];
    }

    public override async Task Execute(ScriptEngine engine)
    {
        foreach (ScriptCommand cmd in InternalCommands)
        {
            try
            {
                await cmd.Execute(engine);
            }
            catch (Exception e)
            {
                EndResult = ScriptResultType.Error;
                return;
            }
        }

        EndResult = ScriptResultType.Success;
    }

    public override string ToString()
    {
        return $"<{LabelName}>";
    }
    #endregion

}

