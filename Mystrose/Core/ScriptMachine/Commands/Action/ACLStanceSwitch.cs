namespace Mystrose.Core.ScriptMachine.Codelines.Action;

public class ACLStanceSwitch : SCLAction
{

    #region Properties: Attributes
    public override string ID
    {
        get => "SCL.003";
    }

    public override ScriptCodelineType Type
    {
        get => ScriptCodelineType.Action;
    }

    public override string Name
    {
        get => "Stance Switch";
    }

    public override string Description
    {
        get => "Script codeline that changes current stance in the script. The new current stance will start from a position depending on the target index.";
    }
    #endregion

    #region Methods: Override
    public override ScriptCodeline Clone()
    {
        return new ACLStanceSwitch()
        {
            Parameters = ScriptMachineParser.CloneToParameters(Parameters)
        };
    }

    public override void LoadRegulars()
    {
        Dictionary<string, ScriptParameter> regulars = new()
        {
            ["Stance Name"] = new ScriptParameter("", "Name of the stance to be switched"),
            ["Index"] = new ScriptParameter(0, "Index of the stance to be started at")
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

        ScriptStance? scriptStance = engine.ActiveLoadout.Stances.Find(
            (s) =>
            {
                return s.Name.Equals(Parameters["Stance Name"].GetVariable(engine).String, StringComparison.OrdinalIgnoreCase);
            });
        int index = Parameters["Index"].GetVariable(engine).Integer;

        if (scriptStance is null || index < 0 || index >= engine.ActiveLoadout.ActiveStance.Commands.Count)
        {
            engine.StateCodelineToBe(ScriptCodelineStatusType.Failed, this);
            return;
        }

        engine.ActiveLoadout.ActiveStance = scriptStance;

        if (index >= 0)
        {
            engine.ActiveLoadout.ActiveStance.SetIndex(index);
        }

        engine.StateCodelineToBe(ScriptCodelineStatusType.Succeed, this);
    }

    public override async Task Cancel(ScriptEngine engine)
    {
        // TODO: Implement cancellation logic if needed
        return;
    }

    public override string ToString()
    {
        return "Switch to " + Parameters["Stance Name"] + " stance on index " + Parameters["Index"];
    }
    #endregion

}
