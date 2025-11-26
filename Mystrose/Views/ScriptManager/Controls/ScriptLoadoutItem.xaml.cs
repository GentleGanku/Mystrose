using UserControl = System.Windows.Controls.UserControl;
using Button = Wpf.Ui.Controls.Button;
using Mystrose.Core.ScriptMachine.Base.Records;

namespace Mystrose.Views.ScriptManager.Controls;

public partial class ScriptLoadoutItem : UserControl
{

    #region Constructor
    public ScriptLoadoutItem()
    {
        InitializeComponent();
        DataContext = this;
        
        LoadoutDetails = new();
    }
    
    public ScriptLoadoutItem(ScriptLoadoutDetails loadoutDetails)
    {
        InitializeComponent();
        DataContext = this;
        
        LoadoutDetails = loadoutDetails;
    }
    #endregion

    #region Private Fields
    private ScriptLoadoutDetails _loadoutDetails;
    private string _name;
    private string _author;
    private DateTime _lastModifiedDate;
    private bool _isEnabled;
    #endregion

    #region Fields
    public MystWindow ParentWindow
    {
        get => (MystWindow)Window.GetWindow(this);
    }
    #endregion
    
    #region Properties
    public ScriptLoadoutDetails LoadoutDetails
    {
        get => _loadoutDetails;
        set
        {
            _loadoutDetails = value;
            
            Name = _loadoutDetails.Name;
            Author = _loadoutDetails.Author;
            LastModifiedDate = _loadoutDetails.LastModifiedDate;
            IsEnabled = _loadoutDetails.IsEnabled;
        }
    }

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            TB_Name.Text = _name;
        }
    }
    
    public string Author
    {
        get => _author;
        set
        {
            _author = value;
            TB_Author.Text = _author;
        }
    }
    
    public DateTime LastModifiedDate
    {
        get => _lastModifiedDate;
        set
        {
            _lastModifiedDate = value;
            TB_LastModifiedDate.Text = _lastModifiedDate.ToString("dd MMM yyyy") + ", " + _lastModifiedDate.ToString("hh:mm tt");
        }
    }
    
    public bool IsEnabled
    {
        get => _isEnabled;
        set
        {
            _isEnabled = value;
            TB_IsEnabled.Text = _isEnabled ? "Enabled" : "Disabled";
        }
    }
    #endregion

    #region Methods: Actions
    private void RunScript()
    {
        
    }
    
    private void StopScript()
    {
        
    }
    
    private void EnableScript()
    {
        IsEnabled = true;
        MBTN_ScriptDisable.Visibility = Visibility.Visible;
        MBTN_ScriptEnable.Visibility = Visibility.Collapsed;
    }
    
    private void DisableScript()
    {
        IsEnabled = false;
        MBTN_ScriptDisable.Visibility = Visibility.Collapsed;
        MBTN_ScriptEnable.Visibility = Visibility.Visible;
    }
    #endregion
    
    #region Events: Read/Write
    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        foreach (MenuButton menuButton in PNL_Menu.Children)
        {
            menuButton.Button.Click += MenuButton_Click;
        }
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        foreach (MenuButton menuButton in PNL_Menu.Children)
        {
            menuButton.Button.Click -= MenuButton_Click;
        }
    }
    #endregion

    #region Events: Interface
    private void MenuButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button btn || btn.Parent is not MenuButton menuBtn)
        {
            return;
        }

        switch (menuBtn.Name)
        {
            case "MBTN_ScriptRun":
                RunScript();
                break;
            case "MBTN_ScriptStop":
                StopScript();
                break;
            case "MBTN_ScriptEnable":
                EnableScript();
                break;
            case "MBTN_ScriptDisable":
                DisableScript();
                break;
        }
    }
    #endregion

}

