using Mystrose.GameModels.Master;
using Mystrose.Panels.MainWindow;
using Mystrose.Systems;
using Mystrose.Utilities.Enumerations;
using System;
using System.Windows.Forms.Integration;

namespace Mystrose.Controls.Main;

public class GameHost : WindowsFormsHost
{

    #region Constructor
    public GameHost()
    {
        State = GameStateType.Idle;
        Buffer = new FrozenPanel();
        ScriptManager = new(this);

        Loaded += OnLoaded;
    }
    #endregion

    #region Destructor
    ~GameHost()
    {
        Flash = null;
        Network = null;
        Buffer = null;

        Child.Dispose();
    }
    #endregion

    #region Properties
    protected internal GameStateType State
    {
        get;
        set;
    }

    public World World
    {
        get;
        set;
    }

    public ScriptManager ScriptManager
    {
        get;
        set;
    }

    public FlashPlayer Flash
    {
        get;
        set;
    }

    public NetworkMonitor Network
    {
        get;
        set;
    }

    public FrozenPanel? Buffer
    {
        get;
        set;
    }

    public int GroupIndex
    {
        get;
        set;
    }
    #endregion

    #region Events
    private void OnLoaded(object sender, EventArgs e)
    {
        Flash ??= new(this);
        Network ??= new();
    }
    #endregion

}
