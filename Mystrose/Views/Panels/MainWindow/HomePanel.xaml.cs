using Button = System.Windows.Controls.Button;
using CheckBox = System.Windows.Controls.CheckBox;
using UserControl = System.Windows.Controls.UserControl;

namespace Mystrose.Views.Panels.MainWindow;

public partial class HomePanel : UserControl
{

    #region Constructor
    public HomePanel()
    {
        InitializeComponent();

        SetValue(Grid.RowProperty, 0);
        SetValue(Grid.ColumnProperty, 0);

        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
    }
    #endregion

    #region Destructor
    ~HomePanel()
    {
        Loaded -= OnLoaded;
        Unloaded -= OnUnloaded;
    }
    #endregion

    #region Properties
    protected internal GameTabBar Parent
    {
        get;
        set;
    }
    #endregion

    #region Main Methods
    public void OpenHyperlink(string url)
    {
        try
        {
            Process.Start(new ProcessStartInfo(new Uri(url).AbsoluteUri)
            {
                UseShellExecute = true
            });
        }
        catch (Exception e)
        {
            //
        }
    }
    #endregion

    #region Setter Methods
    private void InitializeStartup()
    {
        SkipHomeBtn.Checked += OnButtonChecked;
        SkipHomeBtn.Unchecked += OnButtonChecked;

        PlayBtn.Click += OnButtonClick;
        DiscordBtn.Click += OnButtonClick;
        WebsiteBtn.Click += OnButtonClick;
    }

    private void UninitializeStartup()
    {
        SkipHomeBtn.Checked -= OnButtonChecked;
        SkipHomeBtn.Unchecked -= OnButtonChecked;

        PlayBtn.Click -= OnButtonClick;
        DiscordBtn.Click -= OnButtonClick;
        WebsiteBtn.Click -= OnButtonClick;
    }

    private void ToggleSettings()
    {
        SkipHomeBtn.IsChecked = ClientMaster.Settings.IsHomeSkip;
    }
    #endregion

    #region Events
    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        InitializeStartup();
        ToggleSettings();
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        UninitializeStartup();
    }

    private void OnButtonChecked(object sender, RoutedEventArgs e)
    {
        switch (((CheckBox)sender).Name)
        {
            case "SkipHomeBtn":
                ClientMaster.Settings.Set("IsHomeSkip", ((CheckBox)sender).IsChecked);
                break;
        }
    }

    private void OnButtonClick(object sender, RoutedEventArgs e)
    {
        switch (((Button)sender).Name)
        {
            case "PlayBtn":
                //Parent.AddNewTab();
                break;
            case "DiscordBtn":
                OpenHyperlink("https://discord.gg/pearlharbor");
                break;
            case "WebsiteBtn":
                OpenHyperlink("https://auqw.tk/");
                break;
            case "PatreonBtn":
                //OpenHyperlink("https://discord.gg/pearlharbor");
                break;
        }
    }
    #endregion

}
