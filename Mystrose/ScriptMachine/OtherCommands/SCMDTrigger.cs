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

namespace Mystrose.ScriptMachine.Objects;

public class SCMDTrigger : ScriptCommand, ITriggerCommand
{

    #region Constructor
    public SCMDTrigger() : base(ScriptCommandType.Trigger, "SCMD04", "Trigger", "A script command that executes its internal commands, when its prerequisites are met on a trigger. It executes otherwise if the Reverse Check is set to True.")
    {
        Parameters = new()
        {
            ["Trigger Type"] = new ScriptOptions("Variable / Self / Player / Monster / Skill / Aura / Map / Faction / Quest / Item / Drop / Combat Message / Event Message", "The type of the trigger to be checked."),
            ["Trigger Name"] = new ScriptParameter("", "The name of the trigger to be checked."),
            ["Reverse Check"] = new ScriptParameter(false, "If True, the command will execute if the trigger is False."),
            ["Active"] = new ScriptParameter(true, "If False, the trigger will be disabled for event invoke. Otherwise, it's enabled.")
        };
        SecondaryParameters = [];
        InternalCommands = [];
    }
    #endregion

    #region Properties
    public ScriptTriggerType? TriggerType
    {
        get => Enum.TryParse(Parameters["Trigger Type"].String.Replace(" ", ""), out ScriptTriggerType type) ? type : null;
    }

    public List<ScriptCommand> InternalCommands
    {
        get;
        set;
    }

    public bool IsEnabled
    {
        get => Parameters["Active"].Boolean ?? true;
    }
    #endregion

    #region Methods: Interface
    public bool IsValid(ScriptEngine engine, Dictionary<string, ScriptParameter> parameters)
    {
        bool isConditionTrue = false;
        ScriptParameter reverseParameter = Parameters["Reverse Check"].RealValue(engine);

        foreach (KeyValuePair<string, ScriptParameter> parameter in SecondaryParameters[Parameters["Trigger Type"].String])
        {
            ScriptConditional condition = (ScriptConditional)parameter.Value;
            isConditionTrue = condition.IsTrue(parameters[parameter.Key].Object, condition.RealValue(engine));

            if (isConditionTrue != !reverseParameter.Boolean)
            {
                break;
            }
        }

        return reverseParameter.Boolean == true ? (isConditionTrue == false) : (isConditionTrue == true);
    }
    #endregion

    #region Methods: Override
    public override ScriptCommand Clone()
    {
        return new SCMDTrigger()
        {
            InternalCommands = new(InternalCommands),
            Parameters = new(Parameters),
            SecondaryParameters = new(SecondaryParameters),
            EndResult = EndResult
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
        Dictionary<string, ScriptParameter> mandatoryParameters = ScriptRepository.ConvertToConditionals(targetModel.MandatorySearchProperties);

        SecondaryParameters.Clear();
        SecondaryParameters[key] = ScriptRepository.ConvertToConditionals(targetStatement).Where(k => !mandatoryParameters.ContainsKey(k.Key)).ToDictionary();

        return mandatoryParameters;
    }

    public override async Task Execute(ScriptEngine engine)
    {
        bool canExecuteNext = true;
        foreach (ScriptCommand cmd in InternalCommands)
        {
            if (!canExecuteNext)
            {
                canExecuteNext = true;
                continue;
            }

            try
            {
                await cmd.Execute(engine);
            }
            catch (Exception e)
            {
                // TODO: Handle exception
                EndResult = ScriptResultType.Error;
                return;
            }
            // TODO: Handle the command's result

            if (cmd.EndResult == ScriptResultType.Failure)
            {
                canExecuteNext = false;
            }
        }

        EndResult = ScriptResultType.Success;
    }

    public override string ToString()
    {
        return $"<{Parameters["Name"]}>";
    }
    #endregion

}

