using Mystrose.Systems;
using Mystrose.Utilities.Enumerations;
using System.Windows;
using Wpf.Ui.Controls;

namespace Mystrose;

public partial class MainWindow : FluentWindow
{

    #region Constructor
    public MainWindow()
    {
        InitializeComponent();
        Instance = this;

        Loaded += OnLoaded;
    }
    #endregion

    #region Destructor
    ~MainWindow()
    {
        Instance = null;

        Loaded -= OnLoaded;
    }
    #endregion

    #region Static Fields
    public static MainWindow Instance;
    #endregion

    #region Properties
    public GameWindowType GameWindowMode
    {
        get;
        set;
    }
    #endregion

    #region Methods: Initialization
    private void Initialize()
    {
        ClientMaster.Initialize();

        WindowState = ClientMaster.Settings.IsMainWindowMaximized ? WindowState.Maximized : WindowState.Normal;
        GameWindowMode = GameWindowType.Single;
        TitleBar.Window = this;
        NavigationBar.Window = this;
    }
    #endregion

    #region Events
    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        Initialize();
    }
    #endregion

}
