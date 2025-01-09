using UserControl = System.Windows.Controls.UserControl;
using Button = Wpf.Ui.Controls.Button;
using MenuItem = System.Windows.Controls.MenuItem;

namespace Mystrose.Views.Master.Controls;

public partial class MasterTabBar : UserControl
{

    #region Constructor
    public MasterTabBar()
    {
        InitializeComponent();
    }
    #endregion

    #region Fields
    public MystWindow ParentWindow
    {
        get => (MystWindow)Window.GetWindow(this);
    }
    #endregion

    #region Methods: Actions
    public void SelectTab(string codename)
    {
        Response<Action> response = ParentWindow.Invoke(() =>
        {
            if (string.IsNullOrEmpty(codename))
            {
                TBTN_Home.Button.Appearance = ControlAppearance.Primary;
            }
            else
            {
                TBTN_Home.Button.Appearance = ControlAppearance.Transparent;
            }
        });
    }
    #endregion

    #region Methods: Event Handlers
    private void SelectIncomingTab(string codename, HSTGame? game)
    {
        SelectTab(codename);
    }
    #endregion

    #region Events: Read/Write
    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        TBTN_Home.Button.Click += Button_Click;

        MSVCGame.Instance.ActivatedGameEvent += SelectIncomingTab;
        MSVCGame.Instance.SelectedGameEvent += SelectIncomingTab;
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        TBTN_Home.Button.Click -= Button_Click;

        MSVCGame.Instance.ActivatedGameEvent -= SelectIncomingTab;
        MSVCGame.Instance.SelectedGameEvent -= SelectIncomingTab;
    }
    #endregion

    #region Events: Interface
    private void Button_Click(object sender, RoutedEventArgs e)
    {
        TabButton tabButton = ((sender as Button)!.Parent as TabButton)!;

        switch (tabButton.Name)
        {
            case "TBTN_Home":
                if (tabButton.Button.Appearance is ControlAppearance.Transparent)
                {
                    MSVCGame.Instance.Deselect();
                }
                break;
        }
    }

    private void MenuItem_Click(object sender, RoutedEventArgs e)
    {
        MenuItem menuItem = (MenuItem)sender;

        switch ((string)menuItem.Tag)
        {
            case "MI_OpenNewTab":
                MSVCGame.Instance.Activate();
                break;

            case "MI_CloseAll":
                MSVCGame.Instance.DeactivateAll();
                break;

            case "MI_CloseOthers":
                InstanceButton? exceptedButton = IPSTG_List.Instances.Find(i => i.Button.Appearance is ControlAppearance.Primary);

                if (exceptedButton is null)
                {
                    break;
                }

                string exceptedCodename = exceptedButton.NameText;
                MSVCGame.Instance.DeactivateAll(exceptedCodename);
                break;

            case "MI_Reload":
                InstanceButton? reloadingButton = IPSTG_List.Instances.Find(i => i.Button.Appearance is ControlAppearance.Primary);

                if (reloadingButton is null)
                {
                    break;
                }

                string reloadingCodename = reloadingButton.NameText;
                MSVCGame.Instance.Render(reloadingCodename);
                break;

            case "MI_MultiScreen":
                //
                break;
        }
    }
    #endregion

}
