using Mystrose.ScriptMachine.Enumerations;
using Mystrose.ScriptMachine.Inputs;
using Mystrose.ScriptMachine.Interfaces;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Mystrose.ScriptMachine.Objects;

public class SCMDFiller : ScriptCommand, IFillerCommand
{

    #region Constructor
    public SCMDFiller() : base(ScriptCommandType.Filler, "SCMD01", "Filler", "A script command that only displays a line containing text. It does not execute anything.")
    {
        Parameters = new()
        {
            ["Text"] = new ScriptParameter("Hello, World!", "The text to display.")
        };
        SecondaryParameters = [];
    }
    #endregion

    #region Methods: Override
    public override ScriptCommand Clone()
    {
        return new SCMDFiller()
        {
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
        // No execution

        EndResult = ScriptResultType.Success;
    }

    public override string ToString()
    {
        return Parameters["Text"].ToString()!.Length > 0 ? ($"// " + Parameters["Text"].ToString()) : "";
    }
    #endregion

}

