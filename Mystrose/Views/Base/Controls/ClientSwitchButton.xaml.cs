using UserControl = System.Windows.Controls.UserControl;
using ComboBox = System.Windows.Controls.ComboBox;

namespace Mystrose.Views.Base.Controls;

public partial class ClientSwitchButton : UserControl
{

    #region Constructor
    public ClientSwitchButton()
    {
        InitializeComponent();

        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
    }
    #endregion

    #region Fields
    public string SelectedCodename
    {
        get => CB_Item.SelectedValue.ToString()!.Substring(4);
    }
    #endregion

    #region Properties
    public ComboBox Item
    {
        get => CB_Item;
        private set => CB_Item = value;
    }
    #endregion

    #region Methods: Setup
    private void EnlistCodenames()
    {
        int I = 1;

        CB_Item.Items.Clear();
        foreach (string codename in MSVCGame.Instance.ActiveCollection.Keys)
        {
            string codenameString = $"{I} | {codename}";
            CB_Item.Items.Add(codenameString);

            if (!string.IsNullOrEmpty(MSVCGame.Instance.CurrentCodename) && codename.Equals(MSVCGame.Instance.CurrentCodename))
            {
                CB_Item.SelectedIndex = I - 1;
            }

            I++;
        }

        if (string.IsNullOrEmpty(MSVCGame.Instance.CurrentCodename))
        {
            CB_Item.SelectedIndex = 0;
        }

    }
    #endregion

    #region Methods: Service Handlers
    private void ManageCodename(string codename, HSTGame? game)
    {
        EnlistCodenames();
    }
    #endregion

    #region Handlers: Events
    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        EnlistCodenames();

        MSVCGame.Instance.ActivatedGameEvent += ManageCodename;
        MSVCGame.Instance.DeactivatedGameEvent += ManageCodename;
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        MSVCGame.Instance.ActivatedGameEvent -= ManageCodename;
        MSVCGame.Instance.DeactivatedGameEvent -= ManageCodename;

        CB_Item.Items.Clear();
    }
    #endregion

}
