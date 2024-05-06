using Mystrose.ScriptMachine.Enumerations;
using Mystrose.ScriptMachine.Inputs;
using Mystrose.ScriptMachine.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mystrose.ScriptMachine.Objects;

public class SCMDFiller : ScriptCommand, IFillerCommand
{

    #region Constructor
    public SCMDFiller() : base(ScriptCommandType.Filler, "SCMD01", "Filler", "A script command that only displays a line containing blank space or text with colors. It does not execute anything.")
    {
        Parameters = new()
        {
            ["Text"] = new ScriptParameter("", "The text to display.")
        };
        SecondaryParameters = [];
    }
    #endregion

    #region Methods: Override
    public override ScriptCommand Clone()
    {
        return new SCMDFiller()
        {
            Parameters = new(Parameters),
            SecondaryParameters = new(SecondaryParameters),
            EndResult = EndResult
        };
    }

    public override Dictionary<string, ScriptParameter> PassSecondaryParameters(string key)
    {
        SecondaryParameters[key] = [];
        return SecondaryParameters[key];
    }

    public override async Task Execute(ScriptEngine engine)
    {
        // No execution

        EndResult = ScriptResultType.Success;
    }

    public override string ToString()
    {
        return Parameters["Text"]?.String?.Length > 0 ? ($"// " + Parameters["Text"]) : "";
    }
    #endregion

}

