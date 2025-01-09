namespace Mystrose.Views.Master;

public partial class VWMaster : MystWindow
{

    #region Constructor
    public VWMaster() : base()
    {
        InitializeComponent();
        DataContext = this;

        _panels = new()
        {
            [nameof(PNLHome)] = new PNLHome(this),
            [nameof(PNLGameScreen)] = new PNLGameScreen(this)
        };

        Loaded += OnLoaded;
        ContentRendered += OnContentRendered;
        Closing += OnClosing;
    }
    #endregion

    #region (Private) Fields
    private MystPanel _currentPanel;
    private readonly Dictionary<string, MystPanel> _panels;
    #endregion

    #region Properties
    public MystPanel CurrentPanel
    {
        get => _currentPanel;
        set
        {
            _currentPanel = value;
            CPST_Panel.Content = value;
        }
    }
    #endregion

    #region Methods: Setup
    private void CheckupServices()
    {
        Service[] services =
        [
            HSVCLogger.Instance,
            HSVCRepository.Instance,
            HSVCSettings.Instance,
            MSVCGame.Instance,
            MSVCInterceptor.Instance,
            MSVCScript.Instance, 
            MSVCView.Instance, 
            MSVCVisualizer.Instance, 
            MSVCWorld.Instance, 
        ];
    }
    #endregion

    #region Methods: Utility
    private void DisposePanels()
    {
        foreach (var panel in _panels)
        {
            panel.Value.Destruct();
            panel.Value.Dispose();
        }

        _panels.Clear();
    }
    #endregion

    #region Methods: Actions
    public void ActivateGame(string codename, HSTGame? game)
    {
        Response<Action> response = Invoke(() =>
        {
            CurrentPanel = _panels[nameof(PNLGameScreen)]!;
        });
    }

    public void SelectPanel(string codename, HSTGame? game)
    {
        Response<Action> response = Invoke(() =>
        {
            CurrentPanel = codename switch
            {
                "" => _panels[nameof(PNLHome)]!,
                _ => _panels[nameof(PNLGameScreen)]!
            };
        });
    }
    #endregion

    #region Methods: Screen Mode
    public void SetMultiScreen()
    {
        Response<Action> response = Invoke(() =>
        {
            PNLGameScreen screenPanel = (PNLGameScreen)_panels[nameof(PNLGameScreen)];
            List<HSTGame> games = [.. MSVCGame.Instance.ActiveCollection.Values];
            int rowCount = (int)Math.Ceiling((double)games.Count / 2);

            Grid rowGrid = new();

            screenPanel.GRD_GameScreen.Children.Clear();
            for (int I0 = 0; I0 < rowCount; I0++)
            {
                rowGrid.RowDefinitions.Add(new()
                {
                    Height = new(1, GridUnitType.Star)
                });

                Grid gameGrid = new();

                rowGrid.Children.Add(gameGrid);
                Grid.SetRow(gameGrid, I0);

                for (int I1 = 0; I1 < 2; I1++)
                {
                    int gameIndex = I0 * 2 + I1;
                    if (gameIndex < games.Count)
                    {
                        HSTGame game = games[gameIndex];

                        gameGrid.ColumnDefinitions.Add(new()
                        {
                            Width = new(1, GridUnitType.Star)
                        });

                        gameGrid.Children.Add(game);
                        Grid.SetColumn(game, gameIndex);
                    }
                }
            }

            screenPanel.GRD_GameScreen.Children.Add(rowGrid);
        });

        if (response.IsSuccess)
        {
            HSVCLogger.Instance.LogOnTrace("Multi-screen mode set.");
        }
        else
        {
            HSVCLogger.Instance.LogOnTrace(response.Message);
        }
    }
    #endregion

    #region Methods: Service Handlers
    private void ActivateIncomingGame(string codename, HSTGame? game)
    {
        ActivateGame(codename, game);
    }

    private void SelectIncomingPanel(string codename, HSTGame? game)
    {
        SelectPanel(codename, game);
    }
    #endregion

    #region Events: Read/Write
    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        MSVCGame.Instance.ActivatedGameEvent += ActivateIncomingGame;
        MSVCGame.Instance.SelectedGameEvent += SelectIncomingPanel;
    }

    private void OnContentRendered(object? sender, EventArgs e)
    {
        CheckupServices();
    }

    protected void OnClosing(object? sender, CancelEventArgs e)
    {
        ShowActionMessageBox("Exiting Mystrose",
            "Are you sure you want to exit Mystrose? Doing so will stop any ongoing processes immediately.",
            "Yes",
            "No",
            () =>
            {
                HSVCRepository.Instance.Flush();
                MSVCView.Instance.UnrenderAll();
                e.Cancel = false;
            },
            () => e.Cancel = true,
            "Cancel",
            () => e.Cancel = true);
    }

    private void OnClosed(EventArgs e)
    {
        MSVCGame.Instance.ActivatedGameEvent -= ActivateIncomingGame;
        MSVCGame.Instance.SelectedGameEvent -= SelectIncomingPanel;

        DisposePanels();
    }
    #endregion

}
