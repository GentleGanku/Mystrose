namespace Mystrose.Services.Helper;

public class HSVCSettings() : HelperService(nameof(HSVCSettings))
{

    #region Delegates & Handlers
    public delegate void SettingsHandler(SettingOption key, Option option);
    public event SettingsHandler SettingsEvent;
    #endregion

    #region (Static) Fields
    public static HSVCSettings Instance
    {
        get
        {
            if (_instance is null)
            {
                _instance = new HSVCSettings();
                _instance.Construct();
            }
            
            return _instance;
        }
    }
    #endregion

    #region (Private) Fields
    private static HSVCSettings? _instance;
    #endregion

    #region Fields
    private readonly string _pathToSettings = "settings.json";
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        WriteIndented = true
    };
    private readonly Dictionary<SettingOption, Option> _options = new()
    {
        [SettingOption.FirstTime] = new(JsonSerializer.Serialize(SettingOption.FirstTime), 
            "Brings the user to the app's introduction board.", 
            false, 
            true),
        [SettingOption.MaximizedMainWindow] = new(JsonSerializer.Serialize(SettingOption.MaximizedMainWindow), 
            "Maximizes the main window after the app opens.", 
            true, 
            false),
        [SettingOption.SkippableHome] = new(JsonSerializer.Serialize(SettingOption.SkippableHome), 
            "Immediately redirects to the game screen after the app opens.", 
            true, 
            false),
        [SettingOption.DebugNetwork] = new(JsonSerializer.Serialize(SettingOption.DebugNetwork), 
            "Enables network debugging.", 
            false, 
            false)
    };
    #endregion

    #region Methods: Builder
    public override void Construct()
    {
        try
        {
            if (!File.Exists(_pathToSettings))
            {
                Reset();
                return;
            }

            Refresh();

            Log("Settings constructed successfully.", "Construct");
        }
        catch (Exception ex)
        {
            Log(ex.ToString(), "Construct");
        }
    }

    public override void Deconstruct()
    {
        try
        {
            _options.Clear();

            Log("Settings deconstructed successfully.", "Deconstruct");
        }
        catch (Exception ex)
        {
            Log(ex.ToString(), "Deconstruct");
        }
    }

    public void Reset()
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

            Log("Settings reset successfully.", "Reset");
        }
        catch (Exception ex)
        {
            Log(ex.ToString(), "Reset");
        }
    }

    public void Refresh()
    {
        try
        {
            string jsonDictionary = File.ReadAllText(_pathToSettings);
            var settings = JsonSerializer.Deserialize<Dictionary<SettingOption, Option>>(jsonDictionary, _serializerOptions)!;

            foreach (var setting in settings)
            {
                if (!_options.TryGetValue(setting.Key, out Option? option))
                {
                    continue;
                }

                option.Set(setting.Value.Value.ToString()!);

                SettingsEvent?.Invoke(setting.Key, option);
            }

            Log("Settings refreshed successfully.", "Refresh");
        }
        catch (Exception ex)
        {
            Log(ex.ToString(), "Refresh");
        }
    }
    #endregion

    #region Methods: Read/Write
    public Response<Option?> Read(SettingOption key)
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

    public Response<Option?> Write(SettingOption key, object value)
    {
        if (!_options.TryGetValue(key, out Option? option))
        {
            return new(false,
                $"Option not found on the key {key}.",
                null);
        }

        option.Set(value);

        SettingsEvent?.Invoke(key, option);

        Save(key);

        return new(true,
            $"Option written with {value} on the key {key}.",
            option);
    }

    public Response<int> ReadAll()
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

    public Response<Dictionary<SettingOption, Option>> Save(SettingOption key)
    {
        if (!_options.TryGetValue(key, out Option? option))
        {
            return new(false,
                $"Option not found on the key {key}.",
                _options);
        }

        string jsonDictionary = File.ReadAllText(_pathToSettings);
        var settings = JsonSerializer.Deserialize<Dictionary<SettingOption, Option>>(jsonDictionary, _serializerOptions)!;

        settings[key] = option;

        jsonDictionary = JsonSerializer.Serialize(settings, _serializerOptions);
        File.WriteAllText(_pathToSettings, jsonDictionary);

        return new(true,
            "All options saved.",
            _options);
    }

    public Response<Dictionary<SettingOption, Option>> SaveAll()
    {
        string jsonDictionary = JsonSerializer.Serialize(_options, _serializerOptions);
        File.WriteAllText(_pathToSettings, jsonDictionary);

        return new(true, 
            "All options saved.",
            _options);
    }
    #endregion

    #region Methods: Getter/Setter
    public Response<Option?> Get(SettingOption key)
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