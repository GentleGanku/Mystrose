using Mystrose.DataRecords.ReadableModels;
using Mystrose.Views.WorldVisualizer.Controls;
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
    {
        "Auras", "Cells", "Combat Messages", "Event Messages", "Factions", 
        "Inventory Items", "Item Drops", "Monsters", "Players", "Quests", 
        "Shop Items", "Skills"
    };
    
    private readonly Dictionary<string, string> _loaderPresets = new()
    {
        ["Cell to jump onto"] = "(Cell Name) (Pad Name)",
        ["Map SWF to load in"] = "(Map SWF File Path)",
        ["Quest to load in"] = "(Quest ID)",
        ["Shop to load in"] = "(Shop ID)"
    };
    #endregion

    #region Fields
    private MSVCVisualizer VisualizerService => MSVCVisualizer.Instance;
    private ClientSwitchButton SwitchButton => (ClientSwitchButton)TTLB_Main.AdditionalContent;
    private HSTGame CurrentGameHost => MSVCGame.Instance.Collection[SwitchButton.SelectedCodename]!;
    #endregion

    #region Properties
    public VisualizerItem? SelectedVisualizerItem
    {
        get;
        set;
    }
    #endregion

    #region Methods: Setup
    private void RegisterEvents()
    {
        MSVCGame.Instance.DeactivatedGameEvent += DeactivateInstance;
        SwitchButton.Item.SelectionChanged += SwitchButton_SelectionChanged;

        MBTN_GrabberInfo.Button.Click += MenuButton_Click;
        MBTN_LoaderInfo.Button.Click += MenuButton_Click;
        
        MBTN_Refresh.Button.Click += MenuButton_Click;
        MBTN_Load.Button.Click += MenuButton_Click;
        
        foreach (MenuButton menu in SP_ActionButtons.Children)
        {
            menu.Button.Click += MenuButton_Click;
        }
        
        TV_Models.SelectedItemChanged += TreeView_SelectionChanged;
    }

    private void UnregisterEvents()
    {
        MSVCGame.Instance.DeactivatedGameEvent -= DeactivateInstance;
        SwitchButton.Item.SelectionChanged -= SwitchButton_SelectionChanged;

        MBTN_GrabberInfo.Button.Click -= MenuButton_Click;
        MBTN_LoaderInfo.Button.Click -= MenuButton_Click;
        
        MBTN_Refresh.Button.Click -= MenuButton_Click;
        MBTN_Load.Button.Click -= MenuButton_Click;
        
        foreach (MenuButton menu in SP_ActionButtons.Children)
        {
            menu.Button.Click -= MenuButton_Click;
        }
        
        TV_Models.SelectedItemChanged -= TreeView_SelectionChanged;
    }
    
    private void SetupComboBoxes()
    {
        CB_VisualType.Items.Clear();
        foreach (var preset in _grabberPresets)
        {
            CB_VisualType.Items.Add(preset);
        }
        CB_VisualType.SelectedIndex = -1;

        CB_ModelType.Items.Clear();
        foreach (var kv in _loaderPresets)
        {
            CB_ModelType.Items.Add(kv.Key);
        }
        CB_ModelType.SelectedIndex = -1;
    }

    private void ClearComboBoxes()
    {
        CB_VisualType.Items.Clear();
        CB_ModelType.Items.Clear();
    }
    #endregion

    #region Methods: Actions
    private void ResetView()
    {
        Invoke(() =>
        {         
            ClearGrabberView();
            
            CB_VisualType.SelectedIndex = -1;
            CB_ModelType.SelectedIndex = -1;
            TBX_Parameters.Text = string.Empty;
            TBX_Parameters.PlaceholderText = string.Empty;
        });
    }
    
    private void ShowGrabberInfo()
    {
        Invoke(() =>
            ShowMessageBox("How the Grabber Works",
                "While you are in-game, World Visualizer records the game world's state in real-time and stores its data (consisting of game objects with their detail). " +
                "\r\n\r\nCombat/Event Messages and Auras are stored for 3 minutes after occurring.")
        );
    }

    private void ShowLoaderInfo()
    {
        Invoke(() =>
            ShowMessageBox("How the Loader Works",
                "You can select an object type and enter its parameters to load it in-game. " +
                "Each parameter must be separated with a space.  " +
                "\r\n\r\nLoader works only while in-game.")
        );
    }
    #endregion

    #region Methods: Grabber
    private void ClearGrabberView()
    {
        Response<Action> response = Invoke(() =>
        {
            TB_Results.Visibility = Visibility.Collapsed;
            
            TV_Models.Items.Clear();
            TV_Models.Visibility = Visibility.Collapsed;
            TB_EmptyView.Visibility = Visibility.Visible;
            
            SPNL_Attributes.Children.Clear();
            
            TBX_Search.Clear();
            
            RefreshGrabberMenu(null);
        });
    }

    private void RefreshGrabberView()
    {
        if (CB_VisualType.SelectedIndex == -1)
        {
            NotifyInfo("Grabber", "Please select a game object type.");
            return;
        }

        Invoke(() =>
        {
            ClearGrabberView();
            TV_Models.Visibility = Visibility.Visible;
            TB_EmptyView.Visibility = Visibility.Collapsed;

            string type = CB_VisualType.Text;
            Dictionary<string, IReadableModel[]> modelsMap = ConvertToModels(type);

            RefreshGrabberResult(modelsMap.Select(kvp => kvp.Value.Length).Sum());

            if (modelsMap.First().Key.Equals("All"))
            {
                if (modelsMap["All"].Length == 0)
                {
                    TV_Models.Items.Add(new VisualizerItem(null, type));
                    return;
                }

                foreach (IReadableModel model in modelsMap["All"])
                {
                    TV_Models.Items.Add(new VisualizerItem(model, type));
                }
            }
            else
            {
                foreach (KeyValuePair<string, IReadableModel[]> models in modelsMap)
                {
                    var treeItem = new TreeViewItem
                    {
                        Header = models.Key, 
                        IsExpanded = false
                    };
                    
                    if (models.Value.Length == 0)
                    {
                        treeItem.Items.Add(new VisualizerItem(null, type));
                    }
                    else
                    {
                        foreach (IReadableModel model in models.Value)
                        {
                            treeItem.Items.Add(new VisualizerItem(model, type));
                        }
                    }
                    
                    TV_Models.Items.Add(treeItem);
                }
            }
        });
    }

    private void SearchGrabber(string keyword)
    {
        Invoke(() =>
        {
            foreach (var item in TV_Models.Items)
            {
                if (item is TreeViewItem treeViewItem)
                {
                    foreach (VisualizerItem child in treeViewItem.Items)
                    {
                        child.Visibility = string.IsNullOrEmpty(keyword) ||
                                           child.Label.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                            ? Visibility.Visible
                            : Visibility.Collapsed;
                    }
                }
                else if (item is VisualizerItem visualizerItem)
                {
                    visualizerItem.Visibility = string.IsNullOrEmpty(keyword) ||
                                                visualizerItem.Label.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                        ? Visibility.Visible
                        : Visibility.Collapsed;
                }
            }
        });
    }
    
    private void RefreshGrabberAttributes(IReadableModel model)
    {
        Invoke(() =>
        {
            Dictionary<string, string> attributes = JSONParser.ConvertToAttributes(model);
            
            foreach (var item in attributes.Select(att => new AttributeItem(att.Key, att.Value)))
            {
                SPNL_Attributes.Children.Add(item);
            }
            
            RefreshGrabberMenu(model);
        });
    }
    
    private void RefreshGrabberMenu(IReadableModel? readableModel)
    {
        Invoke(() =>
        {
            BRD_ActionButtons.Visibility = Visibility.Visible;
            foreach (MenuButton menu in SP_ActionButtons.Children)
            {
                menu.Visibility = SP_ActionButtons.Children.IndexOf(menu) > 1
                    ? Visibility.Collapsed
                    : Visibility.Visible;
            }
            
            switch (readableModel)
            {
                case RMInventoryItem:
                case RMShopItem:
                    MBTN_WearItem.Visibility = Visibility.Visible;
                    break;
                
                case RMItemDrop:
                    MBTN_WearItem.Visibility = Visibility.Visible;
                    MBTN_AcceptDrop.Visibility = Visibility.Visible;
                    MBTN_RejectDrop.Visibility = Visibility.Visible;
                    break;
                
                case RMQuest:
                    MBTN_LoadQuest.Visibility = Visibility.Visible;
                    MBTN_AcceptQuest.Visibility = Visibility.Visible;
                    MBTN_AbandonQuest.Visibility = Visibility.Visible;
                    MBTN_CompleteQuest.Visibility = Visibility.Visible;
                    break;
                
                default:
                    BRD_ActionButtons.Visibility = Visibility.Collapsed;
                    return;
            }
        });
    }
    
    private void RefreshGrabberResult(int count)
    {
        Invoke(() =>
        {
            TB_Results.Visibility = Visibility.Visible;
            TB_Results.Text = count == 0 ? "No results found." : $"{count} result{(count > 1 ? "s" : "")} found.";
        });
    }
    #endregion

    #region Methods: Loader
    private void ApplyPlaceholderParameters()
    {
        if (CB_ModelType.SelectedIndex == -1)
        {
            NotifyInfo("Loader", "Please select a game object type.");
            return;
        }

        Invoke(() =>
        {
            TBX_Parameters.Text = string.Empty;
            TBX_Parameters.PlaceholderText = _loaderPresets.Values.ElementAt(CB_ModelType.SelectedIndex);
        });
    }

    private void LoadGameObject()
    {
        if (CB_ModelType.SelectedIndex == -1)
        {
            NotifyInfo("Loader", "Please select a game object type.");
            return;
        }

        if (string.IsNullOrEmpty(TBX_Parameters.Text) || TBX_Parameters.Text.Equals(_loaderPresets[CB_ModelType.Text]))
        {
            NotifyInfo("Loader", "Please enter the parameters for this game object.");
            return;
        }

        Invoke(() =>
        {
            string[] parameters = TBX_Parameters.Text.Split(' ');

            switch (CB_ModelType.SelectedIndex)
            {
                case 0:
                    CurrentGameHost.FlashAPI.Map.MoveTo(parameters[0], parameters[1]);
                    break;
                case 1:
                    CurrentGameHost.FlashAPI.Map.Load(parameters[0]);
                    break;
                case 2:
                    CurrentGameHost.FlashAPI.Quest.ShowQuests(int.Parse(parameters[0]));
                    break;
                case 3:
                    CurrentGameHost.FlashAPI.Shop.Load(int.Parse(parameters[0]));
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
                models = VisualizerService.GetAuraModels(codename).Output
                    .GroupBy(item => item.Target_Type + " - " + item.Target_ID)
                    .ToDictionary(items => items.Key, items => items.ToArray<IReadableModel>());
                break;
            case "Cells":
                models["All"] = [.. VisualizerService.GetCellModels(codename).Output];
                break;
            case "Combat Messages":
                models = VisualizerService.GetCombatMessageModels(codename).Output
                    .GroupBy(item => item.Target_Type + " - " + item.Target_ID)
                    .ToDictionary(items => items.Key, items => items.ToArray<IReadableModel>());
                break;
            case "Event Messages":
                models["All"] = [.. VisualizerService.GetEventMessageModels(codename).Output];
                break;
            case "Factions":
                models["All"] = [.. VisualizerService.GetFactionModels(codename).Output];
                break;
            case "Inventory Items":
                var inventories = VisualizerService.GetInventoryItemModels(codename).Output;
                foreach (var inv in inventories)
                {
                    models[inv.Key.ToString()] = [.. inv.Value];
                }
                break;
            case "Item Drops":
                models["All"] = [.. VisualizerService.GetItemDropModels(codename).Output];
                break;
            case "Monsters":
                models["All"] = [.. VisualizerService.GetMonsterModels(codename).Output];
                break;
            case "Players":
                models["All"] = [.. VisualizerService.GetAvatarModels(codename).Output];
                break;
            case "Quests":
                models = VisualizerService.GetQuestModels(codename).Output
                    .GroupBy(item => item.Status)
                    .ToDictionary(items => items.Key, items => items.ToArray<IReadableModel>());
                break;
            case "Shop Items":
                models = VisualizerService.GetShopItemModels(codename).Output
                    .GroupBy(item => item.Category_Type)
                    .ToDictionary(items => items.Key, items => items.ToArray<IReadableModel>());
                break;
            case "Skills":
                models["All"] = [.. VisualizerService.GetActiveSkillModels(codename).Output];
                break;
        }

        return models;
    }
    
    private VisualizerItem? GetSelectedVisualizerItem()
    {
        return SelectedVisualizerItem;
    }
    
    private AttributeItem? GetAttributeItemByName(string name)
    {
        foreach (var item in SPNL_Attributes.Children)
        {
            if (item is AttributeItem attributeItem && attributeItem.Label.Equals(name, StringComparison.OrdinalIgnoreCase))
            {
                return attributeItem;
            }
        }
        
        return null;
    }
    #endregion

    #region Methods: Service Handlers
    private void DeactivateInstance(string codename, object? args)
    {
        if (!codename.Equals(SwitchButton.SelectedCodename))
        {
            return;
        }

        Invoke(() =>
        {
            SwitchButton.RemoveInstance(codename);
        });
    }
    #endregion
    
    #region Events: Read/Write
    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        RegisterEvents();
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        UnregisterEvents();
        ClearComboBoxes();
    }

    private void OnContentRendered(object sender, EventArgs e)
    {
        SetupComboBoxes();
    }
    #endregion

    #region Events: Interface
    public void SwitchButton_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ResetView();
    }
    
    private void MenuButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button btn || btn.Parent is not MenuButton menuBtn)
        {
            return;
        }

        switch (menuBtn.Name)
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
            case "MBTN_WikiSearch":
                OpenHyperlink("https://aqwwiki.wikidot.com/" + GetSelectedVisualizerItem()!.Label.Replace(" ", "+"));
                break;
            case "MBTN_WikiPage":
                OpenHyperlink("https://aqwwiki.wikidot.com/search:main/fullname/" + GetSelectedVisualizerItem()!.Label.Replace(" ", "+"));
                break;
            case "MBTN_WearItem":
                CurrentGameHost.FlashAPI.Inventory.TryItem((BaseItem)GetSelectedVisualizerItem()!.ReadableModel!.ToObject());
                break;
            case "MBTN_LoadQuest":
                CurrentGameHost.FlashAPI.Quest.LoadQuests(int.Parse(GetAttributeItemByName("ID")!.Value));
                break;
            case "MBTN_AcceptQuest":
                CurrentGameHost.FlashAPI.Quest.AcceptQuest(int.Parse(GetAttributeItemByName("ID")!.Value));
                break;
            case "MBTN_AbandonQuest":
                CurrentGameHost.FlashAPI.Quest.AbandonQuest(int.Parse(GetAttributeItemByName("ID")!.Value));
                break;
            case "MBTN_CompleteQuest":
                CurrentGameHost.FlashAPI.Quest.CompleteQuest(int.Parse(GetAttributeItemByName("ID")!.Value));
                break;
            case "MBTN_AcceptDrop":
                CurrentGameHost.FlashAPI.Drop.AcceptDrop(int.Parse(GetAttributeItemByName("ID")!.Value));
                break;
            case "MBTN_RejectDrop":
                CurrentGameHost.FlashAPI.Drop.RejectDrop(GetAttributeItemByName("Name")!.Value);
                break;
        }
    }

    private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        SearchGrabber(TBX_Search.Text);
    }

    private void ObjectTypesBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender.Equals(CB_VisualType))
        {
            ClearGrabberView();
        }
        else if (sender.Equals(CB_ModelType))
        {
            ApplyPlaceholderParameters();
        }
    }
    
    private void TreeView_SelectionChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        if (e.NewValue is VisualizerItem item && item.ReadableModel is not null)
        {
            SelectedVisualizerItem = item;
            RefreshGrabberAttributes(item.ReadableModel);
        }
    }
    #endregion
    
}
