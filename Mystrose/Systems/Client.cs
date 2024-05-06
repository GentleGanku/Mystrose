using Mystrose.Controls.Main;
using Mystrose.Windows;

namespace Mystrose.Systems;

public class Client
{

    #region Constructor
    public Client()
    {
        GameHost = new();
        CombatManager = new(this, GameHost);
        Profile = new();
    }
    #endregion

    #region Properties
    public GameHost GameHost
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
