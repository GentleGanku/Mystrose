using Mystrose.Controls.Main;
using Mystrose.Windows;

namespace Mystrose.Systems;

public class Client
{

    #region Constructor
    public Client()
    {
        GameHost = new();
        ScriptManager = new(this, GameHost);
        CombatManager = new(this, GameHost);
        Profile = new();
    }
    #endregion

    #region Destructor
    ~Client()
    {
        GameHost.Dispose();
        ScriptManager.Dispatcher.Invoke(() => ScriptManager.Close());
        CombatManager.Dispatcher.Invoke(() => CombatManager.Close());
    }
    #endregion

    #region Properties
    public GameHost GameHost
    {
        get;
        set;
    }

    public ScriptManagerWindow ScriptManager
    {
        get;
        set;
    }
    
    public CombatManagerWindow CombatManager
    {
        get;
        set;
    }

    public Profile Profile
    {
        get;
        set;
    }
    #endregion

}
