using Mystrose.ScriptMachine.Objects;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Mystrose.ScriptMachine;

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
            Refresh();
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
    public void Refresh()
    {
        Label = Command.ToString();

        CommandsPnl.Visibility = Command switch
        {
            SCMDList => Visibility.Visible,
            SCMDTrigger => Visibility.Visible,
            _ => Visibility.Collapsed
        };

        IndexTxt.Opacity = Command switch
        {
            SCMDFiller fillerCmd => 0.25,
            _ => 1
        };

        IndexTxt.Foreground = Command switch
        {
            SCMDFiller fillerCmd => new SolidColorBrush(Colors.LightGreen),
            _ => new SolidColorBrush(Colors.DodgerBlue)
        };

        LabelTxt.Foreground = Command switch
        {
            SCMDFiller fillerCmd => new SolidColorBrush(Colors.LightGreen),
            _ => new SolidColorBrush(Colors.WhiteSmoke)
        };
    }
    #endregion

    #region Methods: Command TODO
    public void AddInternalCommand(ScriptCommand command)
    {
        if (Command is not SCMDList or SCMDTrigger)
        {
            return;
        }

        int index = 0;
        switch (Command)
        {
            case SCMDList list:
                index = list.InternalCommands.Count + 1;
                list.InternalCommands.Add(command);
                break;
            case SCMDTrigger trigger:
                index = trigger.InternalCommands.Count + 1;
                trigger.InternalCommands.Add(command);
                break;
        }

        // Panel.AddCodelineItem(ScriptCodelineType.Command, index, Panel, command);
        ScriptCodelineItem item = new(Panel, command, index);
        CommandsLst.Items.Add(item);
    }

    public void RemoveInternalCommand(ScriptCommand command, ScriptCodelineItem item)
    {
        if (Command is not SCMDList or SCMDTrigger)
        {
            return;
        }

        switch (Command)
        {
            case SCMDList list:
                list.InternalCommands.Remove(command);
                break;
            case SCMDTrigger trigger:
                trigger.InternalCommands.Remove(command);
                break;
        }

        CommandsLst.Items.Remove(item);
    }
    #endregion

}
