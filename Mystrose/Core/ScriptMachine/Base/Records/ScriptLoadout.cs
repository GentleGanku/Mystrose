namespace Mystrose.Core.ScriptMachine.Base.Records;

public class ScriptLoadout : ScriptLoadoutDetails
{

    #region Fields
    private ScriptStance _activeStance;
    #endregion

    #region Properties: Collections
    public TypeList<ScriptStance> Stances
    {
        get;
        private set;
    } = 
    [
        new ScriptStance("Main")
    ];

    public TypeList<SCLTrigger> Triggers
    {
        get;
        private set;
    } = [];

    public TypeList<SCLVariable> PresetVariables
    {
        get;
        private set;
    } = [];

    public ScriptVariableCollection Variables
    {
        get;
        private set;
    } = [];
    #endregion

    #region Properties: States
    public ScriptStance ActiveStance
    {
        get => _activeStance ??= Stances[0]!;
        set => _activeStance = value;
    }
    #endregion

    #region Methods: Details
    public void SetLoadoutDetails(string name, string author, string documentation)
    {
        bool isNewlyModified = false;

        if (!Name.Equals(name))
        {
            Name = name;
            isNewlyModified = true;
        }

        if (!Author.Equals(author))
        {
            Author = author;
            isNewlyModified = true;
        }

        if (!Documentation.Equals(documentation))
        {
            Documentation = documentation;
            isNewlyModified = true;
        }

        if (isNewlyModified)
        {
            WriteToFile();
        }
    }
    #endregion

    #region Methods: Read
    public bool ReadFromFile()
    {
        try
        {
            // TO DO: Implement file reading logic

            return true;
        }
        catch
        {
            return false;
        }
    }
    #endregion

    #region Methods: Write
    public bool WriteToFile()
    {
        try
        {
            // TO DO: Implement file writing logic

            LastModifiedDate = DateTime.Now;

            return true;
        }
        catch
        {
            return false;
        }
    }
    #endregion

}
