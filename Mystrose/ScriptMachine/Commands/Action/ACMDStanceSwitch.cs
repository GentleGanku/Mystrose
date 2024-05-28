using Mystrose.ScriptMachine.Enumerations;
using Mystrose.ScriptMachine.Inputs;
using Mystrose.ScriptMachine.Objects;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Mystrose.ScriptMachine.Commands.Action;

public class ACMDStanceSwitch : SCMDAction
{

    #region Constructor
    public ACMDStanceSwitch() : base(ScriptCommandType.Action, "ACMD03", "Stance Switch", "A script command that changes the current stance of the script. The new current stance will start from a position depending on the Index value.")
    {
        Parameters = new()
        {
            ["Stance Name"] = new ScriptParameter("", "Name of the stance to switch to"),
            ["Index"] = new ScriptParameter(0, "Index of the stance to start from")
        };
        SecondaryParameters = [];
    }
    #endregion

    #region Methods: Override
    public override ScriptCommand Clone()
    {
        return new ACMDStanceSwitch()
        {
            Parameters = ScriptRepository.CloneToParameters(Parameters),
            SecondaryParameters = ScriptRepository.CloneToSecondaryParameters(SecondaryParameters),
            EndResult = JsonSerializer.Deserialize<ScriptResultType>(JsonSerializer.Serialize(EndResult))
        };
    }

    public override async Task Execute(ScriptEngine engine)
    {
        ScriptStance? scriptStance = engine.CurrentLoadout.Stances.Find(
            (s) =>
            {
                return s.Name.Equals(Parameters["Stance Name"].GetVar(engine).String, StringComparison.OrdinalIgnoreCase);
            });
        int index = Parameters["Index"].GetVar(engine).Integer;

        if (scriptStance is null || index < 0 || index >= engine.CurrentStance.Commands.Count)
        {
            EndResult = ScriptResultType.Failure;
            return;
        }

        engine.CurrentStance = scriptStance;

        if (index <= -1)
        {
            engine.CurrentIndex = engine.CurrentStance.Index;
        }
        else
        {
            engine.CurrentIndex = index;
            engine.CurrentStance.SetIndex(index);
        }

        EndResult = ScriptResultType.Success;
    }

    public override string ToString()
    {
        return "Switch to Stance " + Parameters["Stance Name"] + " on index " + Parameters["Index"];
    }
    #endregion

}
