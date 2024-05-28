using Mystrose.ScriptMachine.Enumerations;
using Mystrose.ScriptMachine.Inputs;
using Mystrose.ScriptMachine.Interfaces;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Mystrose.ScriptMachine.Objects;

public class SCMDList : ScriptCommand, IListCommand
{

    #region Constructor
    public SCMDList() : base(ScriptCommandType.List, "SCMD05", "List", "A script command that executes a set of one-type commands.")
    {
        Parameters = new()
        {
            ["List Type"] = new ScriptOptions("Action / Statement", "The type of the list.")
        };
        SecondaryParameters = [];
        InternalCommands = [];
    }
    #endregion

    #region Properties
    [JsonIgnore]
    public ScriptListType? ListType
    {
        get => Enum.TryParse(Parameters["List Type"].String, out ScriptListType type) ? type : null;
    }

    public List<ScriptCommand> InternalCommands
    {
        get;
        set;
    }
    #endregion

    #region Methods: Override
    public override ScriptCommand Clone()
    {
        return new SCMDList()
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
                // TODO: Handle exception
                EndResult = ScriptResultType.Error;
                return;
            }
            // TODO: Handle the command's result

            if (cmd.EndResult == ScriptResultType.Failure)
            {
                EndResult = ScriptResultType.Failure;
                return;
            }
        }

        EndResult = ScriptResultType.Success;
    }

    public override string ToString()
    {
        return ListType switch
        {
            ScriptListType.Action => $"Execute {InternalCommands.Count} listed action commands",
            ScriptListType.Statement => $"Execute {InternalCommands.Count} listed statement commands",
            //ScriptListType.Statement => $"If (not) such {InternalCommands.Count} targets have properties in-game",
            _ => "" 
        };
    }
    #endregion

}

