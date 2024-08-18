using TabItem = Wpf.Ui.Controls.TabItem;

namespace Mystrose.Master.Temporary;

public class Profile
{

    #region Constructor
    public Profile()
    {
        UpdateEvent += OnUpdate;
    }
    #endregion

    #region Destructor
    ~Profile()
    {
        UpdateEvent -= OnUpdate;
    }
    #endregion

    #region Delegates
    private delegate void UpdateHandler(string cmd, params object[] args);
    #endregion

    #region Events
    private event UpdateHandler UpdateEvent;
    #endregion

    #region Properties
    protected internal TabItem InstanceTab
    {
        get;
        set;
    }
    #endregion

    #region Methods
    private void OnUpdate(string cmd, params object[] args)
    {
        switch (cmd)
        {
            //case "onLogin":
            //    Name = Flash.GetGameObject("mcLogin.ni.text");
            //    Password = Flash.GetGameObject("mcLogin.pi.text");
            //    break;
        }
    }
    #endregion

}
