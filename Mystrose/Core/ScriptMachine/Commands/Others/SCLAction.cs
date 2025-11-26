namespace Mystrose.Core.ScriptMachine.Codelines.Others;

public class SCLAction : ScriptCodeline
{

    #region Properties: Attributes
    public override string ID
    {
        get => "SCL.000";
    }

    public override ScriptCodelineType Type
    {
        get => ScriptCodelineType.Action;
    }

    public override string Name
    {
        get => "Action";
    }

    public override string Description
    {
        get => "Script codeline that performs a single in-game action.";
    }
    #endregion

    #region Methods: Override
    public override ScriptCodeline Clone()
    {
        return new SCLAction()
        {
            Parameters = ScriptMachineParser.CloneToParameters(Parameters)
        };
    }

    public override void LoadRegulars()
    {
        return;
    }

    public override Dictionary<string, ScriptParameter> LoadAdditionals()
    {
        return [];
    }

    public override async Task Execute(ScriptEngine engine)
    {
        if (Innates["Is Enabled"].Boolean is false)
        {
            return;
        }

        engine.StateCodelineToBe(ScriptCodelineStatusType.Succeed, this);
    }

    public override async Task Cancel(ScriptEngine engine)
    {
        return;
    }

    public override string ToString()
    {
        return "Nothing";
    }
    #endregion

}

