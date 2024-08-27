namespace Mystrose.Master;

public static class ClientMaster
{

    #region Properties: Checks
    private static bool IsHandled
    {
        get;
        set;
    }
    #endregion

    #region Properties: Controls
    //public static Dictionary<WpfControls.TabItem, Client> Clients
    //{
    //    get;
    //    set;
    //}

    public static List<Profile> Profiles
    {
        get;
        set;
    }

    public static Settings Settings
    {
        get;
        set;
    }

    public static DataManager DataManager
    {
        get;
        set;
    }
    #endregion

    #region Methods
    public static void Initialize()
    {
        if (IsHandled)
        {
            return;
        }

        //Clients = [];
        Profiles = new List<Profile>(24);
        Settings = new Settings().Load();
        DataManager = new DataManager().Load();

        Update();

        IsHandled = true;
    }

    public static void Update()
    {
        Settings.Save();
        DataManager.SaveAll();
    }
    #endregion

}
