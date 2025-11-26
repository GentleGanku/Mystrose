namespace Mystrose.Core.ScriptMachine.Codelines.Action;

public class ACLIndexJump : SCLAction
{

    #region Properties: Attributes
    public override string ID
    {
        get => "SCL.001";
    }

    public override ScriptCodelineType Type
    {
        get => ScriptCodelineType.Action;
    }

    public override string Name
    {
        get => "Index Jump";
    }

    public override string Description
    {
        get => "Script codeline that changes current stance index in the running script.";
    }
    #endregion

    #region Methods: Override
    public override ScriptCodeline Clone()
    {
        return new ACLIndexJump()
        {
            Parameters = ScriptMachineParser.CloneToParameters(Parameters)
        };
    }

    public override void LoadRegulars()
    {
        Dictionary<string, ScriptParameter> regulars = new()
        {
            ["Jump Type"] = new ScriptOptions("Go To/Up/Down", "Type of jump to be performed"),
            ["Index"] = new ScriptParameter(0, "Index to be used")
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

        int indexValue = Parameters["Index"].GetVariable(engine).Integer;
        int newIndex = Parameters["Jump Type"].String switch
        {
            "Go To" => indexValue,
            "Up" => engine.ActiveLoadout.ActiveStance.Index + indexValue,
            "Down" => engine.ActiveLoadout.ActiveStance.Index - indexValue
        };

        if (newIndex < 0 || newIndex >= engine.ActiveLoadout.ActiveStance.Commands.Count)
        {
            engine.StateCodelineToBe(ScriptCodelineStatusType.Failed, this);
            return;
        }

        engine.ActiveLoadout.ActiveStance.SetIndex(newIndex);

        engine.StateCodelineToBe(ScriptCodelineStatusType.Succeed, this);
    }

    public override async Task Cancel(ScriptEngine engine)
    {
        // TODO: Implement cancellation logic if needed
        return;
    }

    public override string ToString()
    {
        return Parameters["Jump Type"].String switch
        {
            "Go To" => "Go to index: " + Parameters["Index"].String,
            "Up" => "Jump up " + Parameters["Index"].String + " index(es)",
            "Down" => "Jump down " + Parameters["Index"].String + " index(es)"
        };
    }
    #endregion

}
