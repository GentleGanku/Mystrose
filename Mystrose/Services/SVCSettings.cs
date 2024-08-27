namespace Mystrose.Services;

public class SVCSettings
{

    #region Delegates & Handlers
    public delegate void SettingsHandler(string key, Option option);
    public static event SettingsHandler SettingsEvent;
    #endregion

    #region Fields
    private static string _pathToSettings => "settings.json";
    private static JsonSerializerOptions _serializerOptions = new()
    {
        WriteIndented = true
    };
    private static readonly Dictionary<string, Option> _options = new()
    {
        ["IsFirstTime"] = new("Brings the user to the app's introduction board.", false, true),
        ["IsMainWindowMaximized"] = new("Maximizes the main window after the app opens.", true, false),
        ["IsHomeSkippable"] = new("Immediately redirects to the game screen after the app opens.", true, false),
    };
    #endregion

    #region Methods: Filing
    public static void Checkup()
    {
        if (!File.Exists(_pathToSettings))
        {
            Reset();
            return;
        }

        LoadAll();
    }

    public static void Reset()
    {
        string jsonDictionary = JsonSerializer.Serialize(new Dictionary<string, Option>()
        {
            ["IsFirstTime"] = new("Brings the user to the app's introduction board.", false, true),
            ["IsMainWindowMaximized"] = new("Maximizes the main window after the app opens.", true, false),
            ["IsHomeSkippable"] = new("Immediately redirects to the game screen after the app opens.", true, false),
        }, _serializerOptions);
        File.WriteAllText(_pathToSettings, jsonDictionary);
    }
    #endregion

    #region Methods: Read/Write
    public static void Save(string key, object value)
    {
        if (!_options.TryGetValue(key, out Option option))
        {
            return;
        }

        option.Set(value);

        SettingsEvent.Invoke(key, option);
    }

    public static void LoadAll()
    {
        string jsonDictionary = File.ReadAllText(_pathToSettings);
        var settings = JsonSerializer.Deserialize<Dictionary<string, Option>>(jsonDictionary, _serializerOptions);

        foreach (var setting in settings!)
        {
            Save(setting.Key, setting.Value.Value);
        }
    }
    #endregion

}