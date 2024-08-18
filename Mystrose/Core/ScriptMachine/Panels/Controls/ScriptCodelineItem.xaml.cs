using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using UserControl = System.Windows.Controls.UserControl;

namespace Mystrose.Core.ScriptMachine.Panels.Controls;

/// <summary>
/// An User Control that represents a Script Codeline in the client.
/// </summary>
public partial class ScriptCodelineItem : UserControl
{

    #region Constructor
    public ScriptCodelineItem(ScriptEnginePanel panel, ScriptCommand command, int index)
    {
        InitializeComponent();

        Panel = panel;
        Command = command;
        Index = index;

        DefineStyle();
        Refresh();
    }
    #endregion

    #region Private Variables
    private ScriptEnginePanel _panel;
    private ScriptCommand _command;
    private int _index;
    private string _label;
    #endregion

    #region Properties
    public ScriptEnginePanel Panel
    {
        get => _panel;
        set
        {
            _panel = value;
        }
    }

    public ScriptCommand Command
    {
        get => _command;
        set
        {
            _command = value;
        }
    }

    public int Index
    {
        get => _index;
        set
        {
            _index = value;
            IndexTxt.Text = IndexText;
        }
    }

    public string Label
    {
        get => _label;
        set
        {
            _label = value;
            LabelTxt.Text = value;
        }
    }

    public string IndexText
    {
        get => "[" + _index + "]";
    }
    #endregion

    #region Methods: Utility
    public void DefineStyle()
    {
        IndexTxt.Opacity = Command switch
        {
            SCMDFiller fillerCmd => 0.5,
            _ => 1
        };

        IndexTxt.Foreground = LabelTxt.Foreground = Command switch
        {
            SCMDAction actionCmd => new SolidColorBrush(Colors.OrangeRed),
            SCMDFiller fillerCmd => new SolidColorBrush(Colors.LightGreen),
            SCMDStack listCmd => new SolidColorBrush(Colors.BlueViolet),
            SCMDStatement statementCmd => new SolidColorBrush(Colors.RoyalBlue),
            SCMDTrigger triggerCmd => new SolidColorBrush(Colors.DarkOrange),
            SCMDVariable variableCmd => new SolidColorBrush(Colors.LightPink),

            _ => IndexTxt.Foreground
        };
    }

    public void Refresh()
    {
        Label = Command.ToString();

        if (Command is IStackable stackableCmd)
        {
            InternalCommandsTxt.Visibility = Visibility.Visible;
            InternalCommandsTxt.Text = stackableCmd.InternalCommands.Count + (stackableCmd.InternalCommands.Count == 1 ? " internal command" : " internal commands");

        }
        else
        {
            InternalCommandsTxt.Visibility = Visibility.Collapsed;
        }
    }
    #endregion

    #region Methods: Events
    private void Item_DoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (Command is IStackable)
        {
            Panel.SwitchInternalView(Command);
        }
    }

    private void Item_MouseEnter(object sender, MouseEventArgs e)
    {
        InternalCommandsTxt.Text += " (double-click to view)";
    }

    private void Item_MouseLeave(object sender, MouseEventArgs e)
    {
        InternalCommandsTxt.Text = InternalCommandsTxt.Text.Replace(" (double-click to view)", "");
    }
    #endregion

}
