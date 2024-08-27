namespace Mystrose.Master.Temporary;

public class Settings
{

    #region Constructor
    public Settings()
    {

    }
    #endregion

    #region Destructor
    ~Settings()
    {

    }
    #endregion

    #region Variables
    private string Path
    {
        get
        {
            return AppDomain.CurrentDomain.BaseDirectory + "Settings.json";
        }
    }
    #endregion

    #region Properties
    [JsonInclude]
    public bool IsFirstTime
    {
        get;
        set;
    } = false;

    [JsonInclude]
    public bool IsMainWindowMaximized
    {
        get;
        set;
    } = false;

    [JsonInclude]
    public bool IsHomeSkip
    {
        get;
        set;
    } = false;

    [JsonInclude]
    public GroupType GroupType
    {
        get;
        set;
    } = GroupType.Default;
    #endregion

    #region Methods - Main
    public void Save()
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        string jsonString = JsonSerializer.Serialize(this, options);
        File.WriteAllText(Path, jsonString);
    }

    public Settings Load()
    {
        if (!File.Exists(Path))
        {
            Save();
        }
        
        string jsonString = File.ReadAllText(Path);
        return JsonSerializer.Deserialize<Settings>(jsonString);
    }
    #endregion

    #region Methods - Properties
    public void Set(string key, object value)
    {
        GetType().GetProperty(key).SetValue(this, value);
        Save();
    }
    #endregion

}
