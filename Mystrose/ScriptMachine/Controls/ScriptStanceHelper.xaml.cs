using System.Windows;
using System.Windows.Controls;

namespace Mystrose.ScriptMachine.Controls;

/// <summary>
/// Interaction logic for ScriptStanceHelper.xaml
/// </summary>
public partial class ScriptStanceHelper : UserControl
{

    #region Constructors
    public ScriptStanceHelper(ScriptEnginePanel panel)
    {
        InitializeComponent();

        Panel = panel;
    }
    #endregion

    #region Private Properties
    private ScriptEnginePanel Panel
    {
        get;
        set;
    }
    #endregion

    #region Public Properties
    public string Label
    {
        get => BoxStanceName.Text;
    }
    #endregion

    #region Events
    private void OnBtnAdd_Click(object sender, RoutedEventArgs e)
    {
        if (Panel is null)
        {
            return;
        }

        Panel.AddStance(Label);
    }
    #endregion

}
