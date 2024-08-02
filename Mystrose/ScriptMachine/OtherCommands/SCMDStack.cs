using Mystrose.ScriptMachine.Enumerations;
using Mystrose.ScriptMachine.Inputs;
using Mystrose.ScriptMachine.Interfaces;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Mystrose.ScriptMachine.Objects;

public class SCMDStack : ScriptCommand, IStackCommand, IStackable
{

    #region Constructor
    public SCMDStack() : base(ScriptCommandType.Stack, "SCMD05", "List", "A script command that executes a set of internal commands within its scope. Any kinds of commands, other than Trigger and Variable ones, are executable in this scope. Stacks up to 20 internal commands.")
    {
        Parameters = new()
        {
            ["Label Name"] = new ScriptParameter("Label", "The label name of the stack to be used.")
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
    #endregion

    #region Methods: Interface
    public bool IsInputValid(ScriptCommand cmd)
    {
        return cmd.Type != ScriptCommandType.Trigger && cmd.Type != ScriptCommandType.Variable && InternalCommands.Count <= StackLimit;
    }
    #endregion

    #region Methods: Override
    public override ScriptCommand Clone()
    {
        return new SCMDStack()
        {
            InternalCommands = ScriptRepository.CloneToCommandsList(InternalCommands),
            Parameters = ScriptRepository.CloneToParameters(Parameters),
            SecondaryParameters = ScriptRepository.CloneToSecondaryParameters(SecondaryParameters),
            EndResult = JsonSerializer.Deserialize<ScriptResultType>(JsonSerializer.Serialize(EndResult))
        };
    }

    public override Dictionary<string, ScriptParameter> PassSecondaryParameters(string key)
    {
        SecondaryParameters[key] = [];
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
        return $"Executes the {LabelName} stack";
    }
    #endregion

}

