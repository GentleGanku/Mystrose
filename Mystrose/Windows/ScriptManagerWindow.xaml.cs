using Mystrose.Controls.Main;
using Mystrose.ScriptMachine;
using Mystrose.Systems;
using System.Windows;
using Wpf.Ui.Controls;

namespace Mystrose.Windows;


/// <summary>
/// Interaction logic for ScriptManagerWindow.xaml
/// </summary>
public partial class ScriptManagerWindow : FluentWindow
{

    #region Constructor
    public ScriptManagerWindow(Client client, GameHost host)
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
    public ScriptEngine Engine
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
