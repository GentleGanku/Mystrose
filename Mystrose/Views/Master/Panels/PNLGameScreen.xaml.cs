namespace Mystrose.Views.Master.Panels;

public partial class PNLGameScreen : MystPanel
{

    #region Constructor
    public PNLGameScreen(MystWindow parentWindow) : base(parentWindow)
    {
        InitializeComponent();
        DataContext = this;

        Initialize();
    }
    #endregion

    #region Methods: Setup
    protected override void Initialize()
    {
        MSVCGame.Instance.ActivatedGameEvent += ActivateIncomingGame;
        MSVCGame.Instance.SelectedGameEvent += SelectIncomingGame;

        HSVCLogger.Instance.LogOnConsole("PNLGameScreen is pre-initialized.", $"PNLGameScreen", "InitializeComponent");
    }

    public override void Destruct()
    {
        MSVCGame.Instance.ActivatedGameEvent -= ActivateIncomingGame;
        MSVCGame.Instance.SelectedGameEvent -= SelectIncomingGame;

        HSVCLogger.Instance.LogOnConsole("PNLGameScreen is destructed.", $"PNLGameScreen", "Destruct");
    }
    #endregion

    #region Methods: Action
    public void ActivateGame(string codename, HSTGame? game)
    {
        Response<Action> response = ParentWindow.Invoke(() =>
        {
            if (GRD_Buffer.Visibility is Visibility.Collapsed)
            {
                GRD_Buffer.Visibility = Visibility.Visible;
                GRD_GameScreen.Visibility = Visibility.Collapsed;
            }
        });
    }

    public void SelectGame(string codename, HSTGame? game)
    {
        if (game is null)
        {
            return;
        }

        Response<Action> response = ParentWindow.Invoke(() =>
        {
            GRD_GameScreen.Children.Clear();
            GRD_GameScreen.Children.Add(game);

            if (GRD_Buffer.Visibility is Visibility.Visible)
            {
                GRD_Buffer.Visibility = Visibility.Collapsed;
                GRD_GameScreen.Visibility = Visibility.Visible;
            }
        });
    }
    #endregion

    #region Methods: Service Handlers
    private void ActivateIncomingGame(string codename, HSTGame? game)
    {
        ActivateGame(codename, game);
    }

    private void SelectIncomingGame(string codename, HSTGame? game)
    {
        SelectGame(codename, game);
    }
    #endregion

    #region Handlers: Events
    protected override void OnLoaded(object sender, RoutedEventArgs e)
    {
        HSVCLogger.Instance.LogOnConsole("PNLGameScreen is ready to go.", "PNLGameScreen", "OnLoaded");
    }

    protected override void OnUnloaded(object sender, RoutedEventArgs e)
    {
        HSVCLogger.Instance.LogOnConsole("PNLGameScreen is removed.", "PNLGameScreen", "OnUnloaded");
    }
    #endregion

}
