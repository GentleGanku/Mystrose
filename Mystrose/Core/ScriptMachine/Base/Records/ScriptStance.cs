namespace Mystrose.Core.ScriptMachine.Base.Records;

public class ScriptStance(string name)
{

    #region Fields
    public virtual string Name
    {
        get => name;
    }
    #endregion

    #region Properties: Attributes
    public virtual List<ScriptCodeline> Commands
    {
        get;
        protected set;
    } = [];

    [JsonIgnore]
    public virtual int Index
    {
        get;
        protected set;
    } = 0;
    #endregion

    #region Methods: Actions
    public virtual void SetIndex(int index)
    {
        Index = index;
    }

    public virtual void JumpIndex(int index)
    {
        Index += index;
    }

    public virtual void AddCommand(ScriptCodeline command)
    {
        Commands.Add(command);
    }

    public virtual void AddCommand(ScriptCodeline command, int index)
    {
        Commands.Insert(index, command);
    }

    public virtual void RemoveCommand(ScriptCodeline command)
    {
        Commands.Remove(command);
    }

    public virtual void RemoveCommand(int index)
    {
        Commands.RemoveAt(index);
    }

    public virtual void ClearCommands()
    {
        Commands.Clear();
    }
    #endregion

    #region Methods: Override
    public override string ToString()
    {
        return Name;
    }
    #endregion

}
