namespace Mystrose.Core.ScriptMachine.Commands.Others;

public class SCMDVariable : ScriptCommand, IVariableCommand
{

    #region Constructor
    public SCMDVariable() : base(ScriptCommandType.Variable, "SCMD06", "Variable", "A script command that preserves a script variable on execution, rendering it to be available for use in the parameters. A variable's value can be used by wrapping its keyword with a pair of curly brackets (ex. {keyword} for a value of 1).")
    {
        Parameters = new()
        {
            ["Variable Name"] = new ScriptParameter("", "The name of the variable to set"),
            ["Variable Value"] = new ScriptParameter("", "The value to set the variable to")
        };
        SecondaryParameters = [];
    }
    #endregion

    #region Inputs & Outputs
    [JsonIgnore]
    public ScriptVariable Variable
    {
        get;
        set;
    }
    #endregion

    #region Methods: Override
    public override ScriptCommand Clone()
    {
        return new SCMDVariable()
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
        Variable = new(Parameters["Variable Name"].String, Parameters["Variable Value"].Object);

        bool isSuccess = engine.CurrentLoadout.Variables.Add(Variable);

        EndResult = isSuccess ? ScriptResultType.Success : ScriptResultType.Failure;
    }

    public override string ToString()
    {
        return Parameters["Variable Name"] + " = " + Parameters["Variable Value"];
    }
    #endregion

}

