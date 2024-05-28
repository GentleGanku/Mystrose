using Mystrose.ScriptMachine.Enumerations;
using Mystrose.ScriptMachine.Inputs;
using Mystrose.ScriptMachine.Objects;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Mystrose.ScriptMachine.Commands.Action;

public class ACMDVariableSetter : SCMDAction
{

    #region Constructor
    public ACMDVariableSetter() : base(ScriptCommandType.Action, "ACMD04", "Variable Setter", "A script command that sets a Script Variable up, depending on the Setting type.")
    {
        Parameters = new()
        {
            ["Setting Type"] = new ScriptOptions("Add / Remove / Update", "The type of setting to execute"),
            ["Variable Name"] = new ScriptParameter("", "The name of the variable to set"),
        };
        SecondaryParameters = [];
    }
    #endregion

    #region Methods: Override
    public override ScriptCommand Clone()
    {
        return new ACMDVariableSetter()
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
            "Add" => new()
            {
                ["Variable Value"] = new ScriptParameter("", "The value to set the variable to")
            },
            "Remove" => [],
            "Update" => new()
            {
                ["Operator Type"] = new ScriptOptions("= / + / - / * / : / %", "The type of operator to use"),
                ["Variable Value"] = new ScriptParameter("", "The value to set the variable to")
            }
        };

        return SecondaryParameters[key];
    }

    public override async Task Execute(ScriptEngine engine)
    {
        bool isSuccess = Parameters["Setting Type"].String switch
        {
            "Add" => engine.CurrentLoadout.Variables.Add(engine, Parameters["Variable Name"], SecondaryParameters["Add"]["Variable Value"]),
            "Remove" => engine.CurrentLoadout.Variables.Remove(engine, Parameters["Variable Name"].String),
            "Update" => engine.CurrentLoadout.Variables.Update(engine, Parameters["Variable Name"], SecondaryParameters["Update"]["Variable Value"], (ScriptOptions)SecondaryParameters["Update"]["Operator Type"])
        };

        EndResult = isSuccess ? ScriptResultType.Success : ScriptResultType.Failure;
    }

    public override string ToString()
    {
        return Parameters["Setting Type"].String switch
        {
            "Add" => $"Add Variable {Parameters["Variable Name"].String} with Value {SecondaryParameters["Add"]["Variable Value"]}",
            "Remove" => $"Remove Variable {Parameters["Variable Name"].String}",
            "Update" => $"Update Variable {Parameters["Variable Name"].String} with Value {SecondaryParameters["Update"]["Variable Value"]} ({SecondaryParameters["Update"]["Operator Type"]})"
        };
    }
    #endregion

}
