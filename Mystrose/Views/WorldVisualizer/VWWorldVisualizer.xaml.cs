using Button = Wpf.Ui.Controls.Button;
using TreeViewItem = System.Windows.Controls.TreeViewItem;

namespace Mystrose.Views.WorldVisualizer;

public partial class VWWorldVisualizer : MystWindow
{

    #region Constructor
    public VWWorldVisualizer()
    {
        InitializeComponent();

        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
        ContentRendered += OnContentRendered;
    }
    #endregion

    #region (Private) Fields
    private readonly string[] _grabberPresets =
    [
        "Auras",
        "Cells",
        "Combat Messages",
        "Event Messages",
        "Factions",
        "Inventory Items",
        "Item Drops",
        "Monsters",
        "Players",
        "Quests",
        "Shop Items",
        "Skills"
    ];
    private readonly Dictionary<string, string> _loaderPresets = new()
    {
        ["Cell"] = "(Cell Name) (Pad Name)",
        ["Map"] = "(Map SWF File Path)",
        ["Player Profile"] = "(Player Name)",
        ["Quest"] = "(Quest ID)",
        ["Shop"] = "(Shop ID)"
    };
    #endregion

    #region Fields
    private MSVCVisualizer VisualizerService => MSVCVisualizer.Instance;
    private ClientSwitchButton SwitchButton => (ClientSwitchButton)TTLB_Main.AdditionalContent;
    #endregion

    #region Properties
    public Dictionary<string, IReadableModel[]> CurrentGrabberData
    {
        get;
        set;
    }
    #endregion

    #region Methods: Setup
    private void Initialize()
    {
        CurrentGrabberData = [];

        CB_VisualType.Items.Clear();
        foreach (string preset in _grabberPresets)
        {
            CB_VisualType.Items.Add(preset);
        }
        CB_VisualType.SelectedIndex = -1;

        CB_ModelType.Items.Clear();
        foreach (KeyValuePair<string, string> preset in _loaderPresets)
        {
            CB_ModelType.Items.Add(preset.Key);
        }
        CB_ModelType.SelectedIndex = -1;
    }

    private void Destruct()
    {
        CB_VisualType.Items.Clear();
        CB_ModelType.Items.Clear();

        CurrentGrabberData.Clear();
    }
    #endregion

    #region Methods: Actions
    private void ShowGrabberInfo()
    {
        Response<Action> response = Invoke(() =>
        {
            ShowMessageBox("How the Grabber Works",
                "While you're currently in-game, World Visualizer records the game world's state in real-time and stores its data to the Grabber." +
                " This data is a set of game objects including their details that you can look up for." +
                "\r\n\r\nDo note that Combat/Event Messages and Auras are stored only for the next 3 minutes after its occurrence.");
        });
    }

    private void ShowLoaderInfo()
    {
        Response<Action> response = Invoke(() =>
        {
            ShowMessageBox("How the Loader Works",
                "You can choose an object type and enter its specific parameter(s) in order to load it in-game." +
                " Each parameter that you enter must be separated by a space (e.g. '1 2 3'). If you want to enter a string for one parameter, then you can enclose it with quotations (e.g. \"This is a string.\")." +
                "\r\n\r\nLoader only works while you're currently in-game.");
        });
    }
    #endregion

    #region Methods: Grabber
    private void ClearGrabberView()
    {
        Response<Action> response = Invoke(() =>
        {
            TV_Models.Items.Clear();
            TV_Models.Visibility = Visibility.Collapsed;
            TB_EmptyView.Visibility = Visibility.Visible;
            SPNL_Attributes.Children.Clear();
        });
    }

    private void RefreshGrabberView()
    {
        if (CB_VisualType.SelectedIndex == -1)
        {
            NotifyInfo("Grabber", "Please select a game object type.");
            return;
        }

        Response<Action> response = Invoke(() =>
        {
            TV_Models.Items.Clear();
            TV_Models.Visibility = Visibility.Visible;
            TB_EmptyView.Visibility = Visibility.Collapsed;
            SPNL_Attributes.Children.Clear();

            string type = CB_VisualType.Text;
            CurrentGrabberData = ConvertToModels(type);

            if (CurrentGrabberData.Count == 1)
            {
                if (CurrentGrabberData.First().Value.Length == 0)
                {
                    TreeViewItem emptyItem = new()
                    {
                        Header = "No data available."
                    };

                    TV_Models.Items.Add(emptyItem);
                    return;
                }

                foreach (IReadableModel model in CurrentGrabberData.First().Value)
                {
                    TreeViewItem item = new()
                    {
                        Header = model.ToString(),
                        IsExpanded = true
                    };

                    TV_Models.Items.Add(item);
                }
            }
            else
            {
                foreach (KeyValuePair<string, IReadableModel[]> models in CurrentGrabberData)
                {
                    if (models.Value.Length == 0)
                    {
                        TreeViewItem emptyItem = new()
                        {
                            Header = "No data available."
                        };

                        TV_Models.Items.Add(emptyItem);
                        continue;
                    }

                    TreeViewItem item = new()
                    {
                        Header = models.Key,
                        IsExpanded = true
                    };

                    foreach (IReadableModel model in models.Value)
                    {
                        TreeViewItem subItem = new()
                        {
                            Header = model.ToString()
                        };

                        item.Items.Add(subItem);
                    }

                    TV_Models.Items.Add(item);
                }
            }
        });
    }

    private void RefreshAttributesView(string key, int index)
    {
        if (index == -1)
        {
            NotifyInfo("Grabber", "Please select a game object.");
            return;
        }

        Response<Action> response = Invoke(() =>
        {
            SPNL_Attributes.Children.Clear();

            IReadableModel model = CurrentGrabberData[key][index];
            Dictionary<string, string> attributes = ConvertToAttributes(model);

            foreach (KeyValuePair<string, string> att in attributes)
            {
                AttributeItem item = new(att.Key, att.Value);
                SPNL_Attributes.Children.Add(item);
            }
        });
    }

    private void SearchAttribute(string keyword)
    {
        Response<Action> response = Invoke(() =>
        {
            foreach (AttributeItem item in SPNL_Attributes.Children)
            {
                if (item.AttributeName.ToLower().Contains(keyword.ToLower()))
                {
                    item.Visibility = Visibility.Visible;
                }
                else
                {
                    item.Visibility = Visibility.Collapsed;
                }
            }
        });
    }
    #endregion

    #region Methods: Loader
    private void RenderPlaceholderParameters()
    {
        if (CB_ModelType.SelectedIndex == -1)
        {
            NotifyInfo("Loader", "Please select a game object type.");
            return;
        }

        Response<Action> response = Invoke(() =>
        {
            SPNL_Attributes.Children.Clear();

            int index = CB_ModelType.SelectedIndex;
            string placeholder = _loaderPresets.Values.ElementAt(index);

            TBX_Parameters.PlaceholderText = placeholder;
        });
    }

    private void LoadGameObject()
    {
        if (CB_ModelType.SelectedIndex == -1)
        {
            NotifyInfo("Loader", "Please select a game object type.");
            return;
        }

        if (TBX_Parameters.Text == _loaderPresets[CB_ModelType.Text])
        {
            NotifyInfo("Loader", "Please enter the specific parameter(s) for the game object.");
            return;
        }

        Response<Action> response = Invoke(() =>
        {
            string type = CB_ModelType.Text;
            string[] parameters = TBX_Parameters.Text.Split(' ');

            string codename = SwitchButton.SelectedCodename;

            switch (type)
            {
                case "Shop":
                    break;
                case "Quest":
                    break;
                case "Map":
                    break;
                case "Cell":
                    break;
                case "Player Profile":
                    break;
            }
        });
    }
    #endregion

    #region Methods: Utility
    public Dictionary<string, IReadableModel[]> ConvertToModels(string type)
    {
        Dictionary<string, IReadableModel[]> models = [];
        string codename = SwitchButton.SelectedCodename;

        switch (type)
        {
            case "Auras":
                models[type] = [.. VisualizerService.GetAuraModels(codename).Output];
                break;
            case "Cells":
                models[type] = [.. VisualizerService.GetCellModels(codename).Output];
                break;
            case "Combat Messages":
                models[type] = [.. VisualizerService.GetCombatMessageModels(codename).Output];
                break;
            case "Event Messages":
                models[type] = [.. VisualizerService.GetEventMessageModels(codename).Output];
                break;
            case "Factions":
                models[type] = [.. VisualizerService.GetFactionModels(codename).Output];
                break;
            case "Inventory Items":
                var inventories = VisualizerService.GetInventoryItemModels(codename).Output;
                foreach (var inv in inventories)
                {
                    models[inv.Key.ToString()] = [.. inv.Value];
                }
                break;
            case "Item Drops":
                models[type] = [.. VisualizerService.GetItemDropModels(codename).Output];
                break;
            case "Monsters":
                models[type] = [.. VisualizerService.GetMonsterModels(codename).Output];
                break;
            case "Players":
                models[type] = [.. VisualizerService.GetAvatarModels(codename).Output];
                break;
            case "Quests":
                models[type] = [.. VisualizerService.GetQuestModels(codename).Output];
                break;
            case "Shop Items":
                models[type] = [.. VisualizerService.GetShopItemModels(codename).Output];
                break;
            case "Skills":
                models[type] = [.. VisualizerService.GetActiveSkillModels(codename).Output];
                break;
        }

        return models;
    }

    public Dictionary<string, string> ConvertToAttributes(object obj)
    {
        JsonObject? jsonTarget = JsonSerializer.Deserialize<JsonObject>(JsonSerializer.Serialize(obj));
        Dictionary<string, string> attributes = [];

        foreach (KeyValuePair<string, JsonNode> property in jsonTarget)
        {
            string propertyKey = property.Key.Replace("_", " ");
            string propertyValue = property.Value.ToString();

            attributes.Add(propertyKey, propertyValue);
        }

        return attributes;
    }
    #endregion

    #region Events: Read/Write
    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        MBTN_GrabberInfo.Button.Click += MenuButton_Click;
        MBTN_LoaderInfo.Button.Click += MenuButton_Click;
        
        MBTN_Refresh.Button.Click += MenuButton_Click;
        MBTN_Load.Button.Click += MenuButton_Click;
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        MBTN_GrabberInfo.Button.Click -= MenuButton_Click;
        MBTN_LoaderInfo.Button.Click -= MenuButton_Click;

        MBTN_Refresh.Button.Click -= MenuButton_Click;
        MBTN_Load.Button.Click -= MenuButton_Click;

        Destruct();
    }

    private void OnContentRendered(object? sender, EventArgs e)
    {
        Initialize();
    }
    #endregion

    #region Events: Interface
    private void MenuButton_Click(object sender, RoutedEventArgs e)
    {
        MenuButton button = ((sender as Button)!.Parent as MenuButton)!;

        switch (button.Name)
        {
            case "MBTN_GrabberInfo":
                ShowGrabberInfo();
                break;
            case "MBTN_LoaderInfo":
                ShowLoaderInfo();
                break;

            case "MBTN_Refresh":
                RefreshGrabberView();
                break;

            case "MBTN_Load":
                LoadGameObject();
                break;
        }
    }

    private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        SearchAttribute(TBX_FindLabel.Text);
    }

    private void ObjectTypesBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender == CB_VisualType)
        {
            ClearGrabberView();
        }
        else if (sender == CB_ModelType)
        {
            RenderPlaceholderParameters();
        }
    }
    #endregion

}
