namespace Mystrose.Core.ScriptMachine.Codelines.Others;

public class SCLStack : ScriptCodeline, IStackable
{

    #region Properties: Attributes
    public override string ID
    {
        get => "SCL.201";
    }

    public override ScriptCodelineType Type
    {
        get => ScriptCodelineType.Stack;
    }

    public override string Name
    {
        get => "Stack";
    }

    public override string Description
    {
        get => "Script codeline that executes a set of internal commands within its scope. " +
            "Any commands, except Triggers, Variables, and Options, can be added and executed in this scope.";
    }
    #endregion

    #region Properties: I/O
    public virtual ScriptCodeline[] InternalCommands
    {
        get;
        init;
    } = [];
    #endregion

    #region Methods: Validation
    public virtual string ValidateCodelineToBeAdded(ScriptCodeline cdl)
    {
        return (cdl.Type is not ScriptCodelineType.Trigger &&
            cdl.Type is not ScriptCodelineType.Variable &&
            cdl.Type is not ScriptCodelineType.Option) ?
            "Failed in adding the codeline to the set. Avoid adding codelines such as Triggers, Variables, and Options." :
            "";
    }
    #endregion

    #region Methods: Override
    public override ScriptCodeline Clone()
    {
        return new SCLStack()
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
        if (!Validate(engine))
        {
            return;
        }

        engine.StateCodelineToBe(ScriptCodelineStatusType.Executing, this);

        foreach (ScriptCodeline cmd in InternalCommands)
        {
            if (!Validate(engine))
            {
                return;
            }

            if (cmd.Innates["Is Enabled"].Boolean is false)
            {
                continue;
            }

            try
            {
                cmd.Status = ScriptCodelineStatusType.Standby;
                await cmd.Execute(engine);
            }
            catch (Exception ex)
            {
                engine.StateCodelineToBe(ScriptCodelineStatusType.Canceled, this);
                engine.StateEngineToBe(ScriptEngineStatusType.Crash, ex);

                return;
            }
        }

        engine.StateCodelineToBe(ScriptCodelineStatusType.Succeed, this);
    }

    public override async Task Cancel(ScriptEngine engine)
    {
        return;
    }

    public override string ToString()
    {
        return "Execute a set of internal commands (" + InternalCommands.Length + ")";
    }
    #endregion

}

