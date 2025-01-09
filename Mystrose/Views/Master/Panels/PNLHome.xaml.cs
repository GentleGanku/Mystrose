using CheckBox = System.Windows.Controls.CheckBox;
using Button = System.Windows.Controls.Button;

namespace Mystrose.Views.Master.Panels;

public partial class PNLHome : MystPanel
{

    #region Constructor
    public PNLHome(MystWindow parentWindow) : base(parentWindow)
    {
        InitializeComponent();
        DataContext = this;

        Initialize();
    }
    #endregion

    #region Fields
    public string AppVersion
    {
        get => $"v{Assembly.GetExecutingAssembly().GetName().Version}";
    }
    #endregion

    #region Methods: Setup
    protected override void Initialize()
    {
        HSVCSettings.Instance.SettingsEvent += SetUpcomingOption;

        HSVCSettings.Instance.ReadAll();

        CHBBTN_IsHomeSkippable.Checked += CheckBox_Checked;
        CHBBTN_IsHomeSkippable.Unchecked += CheckBox_Checked;

        MBTN_NewTab.Button.Click += MenuButton_Click;
        MBTN_JoinDiscord.Button.Click += MenuButton_Click;
        MBTN_Donate.Button.Click += MenuButton_Click;

        HSVCLogger.Instance.LogOnConsole("PNLHome is pre-initialized.", $"PNLHome", "InitializeComponent");
    }

    public override void Destruct()
    {
        MBTN_NewTab.Button.Click -= MenuButton_Click;
        MBTN_JoinDiscord.Button.Click -= MenuButton_Click;
        MBTN_Donate.Button.Click -= MenuButton_Click;

        CHBBTN_IsHomeSkippable.Checked -= CheckBox_Checked;
        CHBBTN_IsHomeSkippable.Unchecked -= CheckBox_Checked;

        HSVCSettings.Instance.SettingsEvent -= SetUpcomingOption;

        HSVCLogger.Instance.LogOnConsole("PNLHome is destructed.", $"PNLHome", "Destruct");
    }
    #endregion

    #region Methods: Actions
    private void OpenGameTab()
    {
        Response<Action> response = ParentWindow.Invoke(() =>
        {
            MSVCGame.Instance.Activate();
        });
    }

    private void OpenDiscordLink()
    {
        Response<Action> response = ParentWindow.Invoke(() =>
        {
            ParentWindow.ShowActionMessageBox("Are you sure?",
                "Join and chat with others in the Discord server to get the latest updates!",
                "Yes",
                "No",
                () => ParentWindow.OpenHyperlink("https://bit.ly/MystroseDiscord"),
                () => { return; });
        });
    }

    private void OpenDonateLink()
    {
        Response<Action> response = ParentWindow.Invoke(() =>
        {
            ParentWindow.ShowActionMessageBox("Are you sure?",
                "Donations are greatly appreciated and help keep the project development running!",
                "Yes",
                "No",
                () => ParentWindow.OpenHyperlink("https://paypal.me/GentleGanku"),
                () => { return; });
        });
    }
    #endregion

    #region Methods: Event Handlers
    private void SetUpcomingOption(SettingOption key, Option option)
    {
        switch (key)
        {
            case SettingOption.SkippableHome:
                CHBBTN_IsHomeSkippable.IsChecked = option.Get<bool>();
                break;
        }
    }
    #endregion

    #region Handlers: Events
    protected override void OnLoaded(object sender, RoutedEventArgs e)
    {
        HSVCLogger.Instance.LogOnConsole("PNLHome is ready to go.", "PNLHome", "OnLoaded");
    }

    protected override void OnUnloaded(object sender, RoutedEventArgs e)
    {
        HSVCLogger.Instance.LogOnConsole("PNLHome is removed.", "PNLHome", "OnUnloaded");
    }

    private void CheckBox_Checked(object sender, RoutedEventArgs e)
    {
        CheckBox checkBox = (CheckBox)sender;

        switch (checkBox.Name)
        {
            case "CHBBTN_IsHomeSkippable":
                Response<Option?> responseIsHomeSkippable = HSVCSettings.Instance.Write(SettingOption.SkippableHome, checkBox.IsChecked!);
                HSVCLogger.Instance.LogOnTrace(responseIsHomeSkippable.Message);
                break;
        }
    }

    private void MenuButton_Click(object sender, RoutedEventArgs e)
    {
        MenuButton button = ((sender as Button)!.Parent as MenuButton)!;

        switch (button.Name)
        {
            case "MBTN_NewTab":
                OpenGameTab();
                break;

            case "MBTN_JoinDiscord":
                OpenDiscordLink();
                break;

            case "MBTN_Donate":
                OpenDonateLink();
                break;
        }
    }
    #endregion

}
