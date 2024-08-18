namespace Mystrose.Core.ScriptMachine.Base;

public class ScriptStance
{

    #region Constructor
    public ScriptStance(string name)
    {
        Name = name;
        Commands = [];
    }
    #endregion

    #region Properties
    public string Name
    {
        get;
        set;
    }

    [JsonIgnore]
    public int Index
    {
        get;
        set;
    } = 0;

    public List<ScriptCommand> Commands
    {
        get;
        set;
    }
    #endregion

    #region Methods: Index
    public void SetIndex(int index)
    {
        Index = index;
    }
    #endregion

    #region Methods: Command
    public void AddCommand(ScriptCommand command)
    {
        Commands.Add(command);
    }

    public void AddCommand(ScriptCommand command, int index)
    {
        Commands.Insert(index, command);
    }

    public void RemoveCommand(ScriptCommand command)
    {
        Commands.Remove(command);
    }

    public void RemoveCommand(int index)
    {
        Commands.RemoveAt(index);
    }

    public void ClearCommands()
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
