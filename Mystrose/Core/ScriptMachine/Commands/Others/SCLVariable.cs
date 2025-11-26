namespace Mystrose.Core.ScriptMachine.Codelines.Others;

public class SCLVariable : ScriptCodeline
{

    #region Properties: Attributes
    public override string ID
    {
        get => "SCL.501";
    }

    public override ScriptCodelineType Type
    {
        get => ScriptCodelineType.Variable;
    }

    public override string Name
    {
        get => "Variable";
    }

    public override string Description
    {
        get => "Script codeline that reserves a Variable on execution, " +
            "making it to be available for use in any parameters. " +
            "Using a Variable can be done by wrapping its keyword with a pair of curly brackets, like {KEYWORD}.";
    }
    #endregion

    #region Methods: Override
    public override ScriptCodeline Clone()
    {
        return new SCLVariable()
        {
            Parameters = ScriptMachineParser.CloneToParameters(Parameters)
        };
    }

    public override void LoadRegulars()
    {
        Dictionary<string, ScriptParameter> regulars = new()
        {
            ["Variable Name"] = new ScriptParameter("", "Name of the variable to be set"),
            ["Variable Value"] = new ScriptParameter("", "Value of the variable to be used")
        };
        Parameters = Parameters.Concat(regulars)
            .ToDictionary(
                kvp => kvp.Key, 
                kvp => kvp.Value);
    }

    public override Dictionary<string, ScriptParameter> LoadAdditionals()
    {
        return [];
    }

    public override async Task Execute(ScriptEngine engine)
    {
        if (!Validate(engine))
        {
            return;
        }

        engine.StateCodelineToBe(ScriptCodelineStatusType.Executing, this);

        ScriptKeyValuePair keyValuePair = new(Parameters["Variable Name"].String, Parameters["Variable Value"].Value);
        bool isSuccess = engine.ActiveLoadout.Variables.Add(keyValuePair) is not null;

        engine.StateCodelineToBe(
            isSuccess ? ScriptCodelineStatusType.Succeed : ScriptCodelineStatusType.Failed,
            this);
    }

    public override async Task Cancel(ScriptEngine engine)
    {
        engine.ActiveLoadout.Variables.Remove(Parameters["Variable Name"].String);
    }

    public override string ToString()
    {
        return Parameters["Variable Name"].String + " = " + Parameters["Variable Value"].String;
    }
    #endregion

}

