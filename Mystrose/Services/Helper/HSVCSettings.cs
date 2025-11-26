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
    private string _pathToFolder => "configs";
    private string _pathToSettings => _pathToFolder + "\\settings.json";
    private readonly Dictionary<SettingOption, Option> _options = new()
    {
        [SettingOption.FirstTime] = new(JSONParser.Serialize(SettingOption.FirstTime), 
            "Brings the user to the app's introduction board.", 
            false, 
            true),
        [SettingOption.MaximizedMainWindow] = new(JSONParser.Serialize(SettingOption.MaximizedMainWindow), 
            "Maximizes the main window after the app opens.", 
            true, 
            false),
        [SettingOption.SkippableHomeScreen] = new(JSONParser.Serialize(SettingOption.SkippableHomeScreen), 
            "Immediately redirects to the game screen after the app opens.", 
            true, 
            false),
        [SettingOption.DebugNetwork] = new(JSONParser.Serialize(SettingOption.DebugNetwork), 
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
            if (!Directory.Exists(_pathToFolder))
            {
                Directory.CreateDirectory(_pathToFolder);
            }

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
            string jsonDictionary = JSONParser.Serialize(new Dictionary<SettingOption, Option>()
            {
                [SettingOption.FirstTime] = new(JSONParser.Serialize(SettingOption.FirstTime), 
                    "Brings the user to the app's introduction board.", 
                    false, 
                    true),
                [SettingOption.MaximizedMainWindow] = new(JSONParser.Serialize(SettingOption.MaximizedMainWindow), 
                    "Maximizes the main window after the app opens.", 
                    true, 
                    false),
                [SettingOption.SkippableHomeScreen] = new(JSONParser.Serialize(SettingOption.SkippableHomeScreen), 
                    "Immediately redirects to the game screen after the app opens.", 
                    true, 
                    false),
                [SettingOption.DebugNetwork] = new(JSONParser.Serialize(SettingOption.DebugNetwork), 
                    "Enables network debugging.", 
                    false, 
                    false)
            });
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
            var settings = JSONParser.Deserialize<Dictionary<SettingOption, Option>>(jsonDictionary)!;

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
        var settings = JSONParser.Deserialize<Dictionary<SettingOption, Option>>(jsonDictionary)!;

        settings[key] = option;

        jsonDictionary = JSONParser.Serialize(settings);
        File.WriteAllText(_pathToSettings, jsonDictionary);

        return new(true,
            "All options saved.",
            _options);
    }

    public Response<Dictionary<SettingOption, Option>> SaveAll()
    {
        string jsonDictionary = JSONParser.Serialize(_options);
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