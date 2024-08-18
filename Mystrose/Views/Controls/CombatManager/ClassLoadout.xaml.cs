using UserControl = System.Windows.Controls.UserControl;

namespace Mystrose.Views.Controls.CombatManager;

/// <summary>
/// Interaction logic for ClassLoadout.xaml
/// </summary>
public partial class ClassLoadout : UserControl
{
    
    #region Constructor
    public ClassLoadout()
    {
        InitializeComponent();

        DataContext = this;
    }
    #endregion

    #region Private Variables
    private bool _isEnabled;
    private string _class;
    private string _alias;
    #endregion

    #region Properties
    /// <summary>
    /// The condition of whether the loadout is enabled to play or not.
    /// </summary>
    public bool IsEnabled
    {
        get
        {
            return _isEnabled;
        }
        set
        {
            _isEnabled = value;
            LoadoutChk.IsChecked = value;
        }
    }

    /// <summary>
    /// The loadout's class name.
    /// </summary>
    public string Class
    {
        get
        {
            return _class;
        }
        set
        {
            _class = value;
            LoadoutClassTxt.Text = value;
        }
    }

    /// <summary>
    /// The loadout's alias name.
    /// </summary>
    public string Alias
    {
        get
        {
            return _alias;
        }
        set
        {
            _alias = value;
            LoadoutAliasTxt.Text = AliasText;
        }
    }

    /// <summary>
    /// The alias text of the loadout.
    /// </summary>
    public string AliasText
    {
        get
        {
            return $"[{Alias}]";
        }
    }
    #endregion

}

