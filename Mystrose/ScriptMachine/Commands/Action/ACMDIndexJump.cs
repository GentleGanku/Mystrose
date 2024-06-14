using Mystrose.ScriptMachine.Enumerations;
using Mystrose.ScriptMachine.Inputs;
using Mystrose.ScriptMachine.Objects;
using System.Text.Json;
using System.Threading.Tasks;

namespace Mystrose.ScriptMachine.Commands.Action;

public class ACMDIndexJump : SCMDAction
{

    #region Constructor
    public ACMDIndexJump() : base(ScriptCommandType.Action, "ACMD01", "Index Jump", "A script command that changes the current index of the script.")
    {
        Parameters = new()
        {
            ["Jump Type"] = new ScriptOptions("Go To / Up / Down", "The type of jump to perform."),
            ["Index"] = new ScriptParameter(0, "The index to jump to.")
        };
        SecondaryParameters = [];
    }
    #endregion

    #region Methods: Override
    public override ScriptCommand Clone()
    {
        return new ACMDIndexJump()
        {
            Parameters = ScriptRepository.CloneToParameters(Parameters),
            SecondaryParameters = ScriptRepository.CloneToSecondaryParameters(SecondaryParameters),
            EndResult = JsonSerializer.Deserialize<ScriptResultType>(JsonSerializer.Serialize(EndResult))
        };
    }

    public override async Task Execute(ScriptEngine engine)
    {
        int indexValue = Parameters["Index"].GetVar(engine).Integer;
        int newIndex = Parameters["Jump Type"].String switch
        {
            "Go To" => indexValue,
            "Up" => engine.CurrentIndex + indexValue,
            "Down" => engine.CurrentIndex - indexValue
        };

        if (newIndex < 0 || newIndex >= engine.CurrentStance.Commands.Count)
        {
            EndResult = ScriptResultType.Success;
            return;
        }

        engine.CurrentIndex = newIndex;
        engine.CurrentStance.SetIndex(newIndex);

        EndResult = ScriptResultType.Success;
    }

    public override string ToString()
    {
        return Parameters["Jump Type"].String switch
        {
            "Go To" => "Go to index: " + Parameters["Index"],
            "Up" => "Jump up " + Parameters["Index"] + " index(es)",
            "Down" => "Jump down " + Parameters["Index"] + " index(es)"
        };
    }
    #endregion

}
