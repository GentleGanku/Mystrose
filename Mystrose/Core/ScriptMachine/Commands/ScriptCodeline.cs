namespace Mystrose.Core.ScriptMachine.Codelines;

public abstract class ScriptCodeline
{

    #region Fields
    public ScriptParameter? this[string key]
    {
        get => Parameters.TryGetValue(key, out ScriptParameter? prm) ? prm : null;
    }

    public virtual Dictionary<string, ScriptParameter> Regulars
    {
        get => Parameters
            .Where(kvp => !kvp.Key.StartsWith(ScriptMachineParser.INNATE_PREFIX)
                && !kvp.Key.StartsWith(ScriptMachineParser.ADDITIONAL_PREFIX)
                && !kvp.Key.StartsWith(ScriptMachineParser.CONDITIONAL_PREFIX))
            .ToDictionary();
    }

    public virtual Dictionary<string, ScriptParameter> Innates
    {
        get => Parameters
            .Where(kvp => kvp.Key.StartsWith(ScriptMachineParser.INNATE_PREFIX))
            .ToDictionary(
                kvp => ScriptMachineParser.SanitizeLabel(kvp.Key),
                kvp => kvp.Value
            );
    }

    public virtual Dictionary<string, ScriptParameter> Additionals
    {
        get => Parameters
            .Where(kvp => kvp.Key.StartsWith(ScriptMachineParser.ADDITIONAL_PREFIX))
            .ToDictionary(
                kvp => ScriptMachineParser.SanitizeLabel(kvp.Key),
                kvp => kvp.Value
            );
    }

    public virtual Dictionary<string, ScriptParameter> Conditionals
    {
        get => Parameters
            .Where(kvp => kvp.Key.StartsWith(ScriptMachineParser.CONDITIONAL_PREFIX))
            .ToDictionary(
                kvp => ScriptMachineParser.SanitizeLabel(kvp.Key),
                kvp => kvp.Value
            );
    }
    #endregion

    #region Properties: Attributes
    public virtual string ID
    {
        get;
    }

    public virtual ScriptCodelineType Type
    {
        get;
    }

    public virtual string Name
    {
        get;
    }

    public virtual string Description
    {
        get;
    }
    #endregion

    #region Properties: I/O
    public virtual Dictionary<string, ScriptParameter> Parameters
    {
        get;
        protected set;
    } = HSVCScriptMachineExtensions.Instance.RetrieveInnateParameters().Output!;

    public virtual ScriptCodelineStatusType Status
    {
        get;
        set;
    } = ScriptCodelineStatusType.Idle;
    #endregion

    #region Methods: Validations
    public virtual bool Validate(ScriptEngine engine)
    {
        return Innates["Is Enabled"].Boolean is true &&
            Status is not ScriptCodelineStatusType.Canceled &&
            engine.Status < ScriptEngineStatusType.Paused;
    }
    #endregion

    #region Methods: Overrides
    public abstract ScriptCodeline Clone();

    public abstract void LoadRegulars();

    public abstract Dictionary<string, ScriptParameter> LoadAdditionals();

    public abstract Task Execute(ScriptEngine engine);

    public abstract Task Cancel(ScriptEngine engine);

    public abstract new string ToString();
    #endregion

}
