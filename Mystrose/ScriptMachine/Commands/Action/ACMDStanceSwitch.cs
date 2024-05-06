using Mystrose.ScriptMachine.Enumerations;
using Mystrose.ScriptMachine.Inputs;
using Mystrose.ScriptMachine.Objects;
using System;
using System.Threading.Tasks;

namespace Mystrose.ScriptMachine.Commands.Action;

public class ACMDStanceSwitch : SCMDAction
{

    #region Constructor
    public ACMDStanceSwitch() : base(ScriptCommandType.Action, "ACMD03", "Stance Switch", "A script command that changes the current stance of the script. If the Index value is set (not -1), the new current stance will start from that. Otherwise, it will start from its current index instead.")
    {
        Parameters = new()
        {
            ["Stance Name"] = new ScriptParameter("", "Name of the stance to switch to"),
            ["Index"] = new ScriptParameter(-1, "Index of the stance to start from")
        };
        SecondaryParameters = [];
    }
    #endregion

    #region Methods: Override
    public override ScriptCommand Clone()
    {
        return new ACMDStanceSwitch()
        {
            Parameters = new(Parameters),
            SecondaryParameters = new(SecondaryParameters),
            EndResult = EndResult
        };
    }

    public override async Task Execute(ScriptEngine engine)
    {
        ScriptStance? scriptStance = engine.CurrentLoadout.Stances.Find(
            (s) =>
            {
                return s.Name.Equals(Parameters["Stance Name"].RealValue(engine).String, StringComparison.OrdinalIgnoreCase);
            });
        int index = (int)Parameters["Index"].RealValue(engine).Integer;

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
