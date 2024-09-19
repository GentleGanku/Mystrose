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
        ["firstTime"] = new("First-Time User", "Brings the user to the app's introduction board.", false, true),
        ["maximizedMainWindow"] = new("Maximized App Window on Startup", "Maximizes the main window after the app opens.", true, false),
        ["skippableHome"] = new("Home Skip", "Immediately redirects to the game screen after the app opens.", true, false),
        ["debugNetwork"] = new("Network Debugging", "Enables network debugging.", false, false)
    };
    #endregion

    #region Methods: Filing
    public static void Checkup()
    {
        try
        {
            if (!File.Exists(_pathToSettings))
            {
                Reset();
                return;
            }

            Refresh();

            SVCLogger.LogOnConsole("Settings checkup completed.", "SVCSettings", "Checkup");
        }
        catch (Exception ex)
        {
            SVCLogger.LogOnConsole(ex.ToString(), "SVCSettings", "Checkup");
        }
    }

    public static void Reset()
    {
        try
        {
            string jsonDictionary = JsonSerializer.Serialize(new Dictionary<string, Option>()
            {
                ["firstTime"] = new("First-Time User", "Brings the user to the app's introduction board.", false, true),
                ["maximizedMainWindow"] = new("Maximized App Window on Startup", "Maximizes the main window after the app opens.", true, false),
                ["skippableHome"] = new("Home Skip", "Immediately redirects to the game screen after the app opens.", true, false),
                ["debugNetwork"] = new("Network Debugging", "Enables network debugging.", false, false)
            }, _serializerOptions);
            File.WriteAllText(_pathToSettings, jsonDictionary);

            SVCLogger.LogOnConsole("Settings reset completed.", "SVCSettings", "Reset");
        }
        catch (Exception ex)
        {
            SVCLogger.LogOnConsole(ex.ToString(), "SVCSettings", "Reset");
        }
    }

    public static void Refresh()
    {
        try
        {
            string jsonDictionary = File.ReadAllText(_pathToSettings);
            var settings = JsonSerializer.Deserialize<Dictionary<string, Option>>(jsonDictionary, _serializerOptions)!;

            foreach (var setting in settings)
            {
                if (!_options.TryGetValue(setting.Key, out Option? option))
                {
                    continue;
                }

                option.Set(setting.Value.Value.ToString()!);

                SettingsEvent?.Invoke(setting.Key, option);
            }

            SVCLogger.LogOnConsole("Settings refresh completed.", "SVCSettings", "Refresh");
        }
        catch (Exception ex)
        {
            SVCLogger.LogOnConsole(ex.ToString(), "SVCSettings", "Refresh");
        }
    }
    #endregion

    #region Methods: Read/Write
    public static Response<Option?> Read(string key)
    {
        if (!_options.TryGetValue(key, out Option? option))
        {
            return new(false, 
                $"Option not found on the key {key}.", 
                null);
        }

        SettingsEvent?.Invoke(key, option);

        return new(true, 
            $"Option read on the key {key}.", 
            option);
    }

    public static Response<Option?> Write(string key, object value)
    {
        if (!_options.TryGetValue(key, out Option? option))
        {
            return new(false,
                $"Option not found on the key {key}.",
                null);
        }

        option.Set(value);

        SettingsEvent?.Invoke(key, option);

        SaveAll();

        return new(true,
            $"Option written with {value} on the key {key}.",
            option);
    }

    public static Response<int> ReadAll()
    {
        if (_options.Count <= 0)
        {
            return new(false, 
                "No options found.", 
                0);
        }

        foreach (var key in _options.Keys)
        {
            Read(key);
        }

        return new(true, 
            "All options read.", 
            _options.Count);
    }

    public static Response<Dictionary<string, Option>> SaveAll()
    {
        string jsonDictionary = JsonSerializer.Serialize(_options, _serializerOptions);
        File.WriteAllText(_pathToSettings, jsonDictionary);

        return new(true, 
            "All options saved.",
            _options);
    }
    #endregion

    #region Methods: Getter/Setter
    public static Response<Option?> Get(string key)
    {
        if (!_options.TryGetValue(key, out Option? option))
        {
            return new(false,
                $"Option not found on the key {key}.",
                null);
        }

        return new(true,
            $"Option found on the key {key}.",
            option);
    }
    #endregion

}