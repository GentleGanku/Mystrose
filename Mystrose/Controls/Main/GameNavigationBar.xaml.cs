using Mystrose.Systems;
using System.Windows;
using System.Windows.Controls;

namespace Mystrose.Controls.Main;

public partial class GameNavigationBar : UserControl
{

    #region Constructor
    public GameNavigationBar()
    {
        InitializeComponent();

        Loaded += OnLoaded;
    }
    #endregion

    #region Properties
    protected internal MainWindow Window
    {
        get;
        set;
    }
    #endregion

    #region Events
    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        NotifsFlyoutContent.Parent = NotifsFlyout;
        NotifsFlyoutContent.NoticeIcon = NotifsNoticeIcon;
    }

    private void OnButtonClick(object sender, RoutedEventArgs e)
    {
        switch (((Wpf.Ui.Controls.Button)sender).Name)
        {
            case "ScriptMngrBtn":
                ClientMaster.Clients[Window.TitleBar.SelectedItem].ScriptManager.Show();
                break;
            case "CombatMngrBtn":
                ClientMaster.Clients[Window.TitleBar.SelectedItem].CombatManager.Show();
                break;

            case "NotifsBtn":
                NotifsFlyoutContent.Open();
                break;
        }
    }
    #endregion

}