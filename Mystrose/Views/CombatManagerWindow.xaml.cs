using Button = System.Windows.Controls.Button;

namespace Mystrose.Views;

/// <summary>
/// Interaction logic for CombatManagerWindow.xaml
/// </summary>
public partial class CombatManagerWindow : FluentWindow
{

    #region Constructor
    public CombatManagerWindow(Client client, GameHost host)
    {
        InitializeComponent(); 

        ParentClient = client;
        ParentHost = host;
        EnginePanel = new(Engine);

        Loaded += OnLoaded;
    }
    #endregion

    #region Private Fields
    private ScriptEnginePanel _enginePanel;
    #endregion

    #region Fields
    public CombatEngine Engine
    {
        get => ParentHost.ScriptManager.CombatEngine;
    }
    #endregion

    #region Properties
    public Client ParentClient
    {
        get;
        set;
    }

    public GameHost ParentHost
    {
        get;
        set;
    }

    public ScriptEnginePanel EnginePanel
    {
        get => _enginePanel;
        set
        {
            _enginePanel = value;
            EngineCtl.Content = value;
        }
    }
    #endregion

    #region Methods: Initialization
    private void Initialize()
    {
        WindowState = WindowState.Maximized;
    }
    #endregion

    #region Events
    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        Initialize();
    }

    private void OnButtonClick(object sender, RoutedEventArgs e)
    {
        switch ((sender as Button).Name)
        {
            case "AttackBtn":

                break;
        }
    }
    #endregion

}
