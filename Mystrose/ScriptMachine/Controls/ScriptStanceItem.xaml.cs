using Mystrose.ScriptMachine.Objects;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Mystrose.ScriptMachine.Controls;

/// <summary>
/// Interaction logic for ScriptStanceItem.xaml
/// </summary>
public partial class ScriptStanceItem : UserControl
{

    #region Constructors
    public ScriptStanceItem(ScriptEnginePanel panel, ScriptStance stance)
    {
        InitializeComponent();

        Panel = panel;
        Stance = stance;
    }
    #endregion

    #region Private Fields
    private ScriptStance _stance;
    private string _label;
    #endregion

    #region Private Properties
    private ScriptEnginePanel Panel
    {
        get;
        set;
    }
    #endregion

    #region Public Properties
    public ScriptStance Stance
    {
        get => _stance;
        set
        {
            _stance = value;
            Label = value.Name;
        }
    }

    public string Label
    {
        get => _label;
        set
        {
            _label = value;
            BlockStanceName.Text = _label;
        }
    }
    #endregion

    #region Methods
    public void ToggleButtonMode(bool toggle)
    {
        if (toggle)
        {
            BtnRemove.Visibility = Visibility.Collapsed;
        }
        else
        {
            BtnRemove.Visibility = Visibility.Visible;
        }
    }
    #endregion

    #region Events
    private void OnBtnRemove_Click(object sender, RoutedEventArgs e)
    {
        if (Panel is null)
        {
            return;
        }

        Panel.RemoveStance(this);
    }
    #endregion

    #region Overrides
    public override string ToString()
    {
        return Label;
    }
    #endregion

}
