using UserControl = System.Windows.Controls.UserControl;
using Button = Wpf.Ui.Controls.Button;

namespace Mystrose.Views.Master.Controls;

public partial class MasterToolBar : UserControl
{

    #region Constructor
    public MasterToolBar()
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

    #region Events: Read/Write
    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        MBTN_ScriptManager.Button.Click += MenuButton_Click;
        MBTN_Visualizer.Button.Click += MenuButton_Click;
        MBTN_Interceptor.Button.Click += MenuButton_Click;
        MBTN_Logger.Button.Click += MenuButton_Click;
        MBTN_Notifications.Button.Click += MenuButton_Click;
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        MBTN_ScriptManager.Button.Click -= MenuButton_Click;
        MBTN_Visualizer.Button.Click -= MenuButton_Click;
        MBTN_Interceptor.Button.Click -= MenuButton_Click;
        MBTN_Logger.Button.Click -= MenuButton_Click;
        MBTN_Notifications.Button.Click -= MenuButton_Click;
    }
    #endregion

    #region Events: Interface
    private void MenuButton_Click(object sender, RoutedEventArgs e)
    {
        MenuButton button = ((sender as Button)!.Parent as MenuButton)!;
        Response<MystWindow?> response = new(true, 
            "An error occurred while processing the request.", 
            ParentWindow);

        switch (button.Name)
        {
            case "MBTN_ScriptManager":
                // TODO: Open Script Manager
                break;
            case "MBTN_Visualizer":
                response = MSVCView.Instance.OpenForInstances(typeof(VWWorldVisualizer));
                break;
            case "MBTN_Interceptor":
                response = MSVCView.Instance.OpenForInstances(typeof(VWPacketInterceptor));
                break;
            case "MBTN_Logger":
                response = MSVCView.Instance.OpenForInstances(typeof(VWLogger));
                break;

            case "MBTN_Notifications":
                break;
        }

        if (!response.IsSuccess)
        {
            ParentWindow.ShowMessageBox(response.Output!.Title, response.Message);
        }
    }
    #endregion

}
