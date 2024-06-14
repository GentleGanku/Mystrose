using Mystrose.ScriptMachine.Enumerations;
using Mystrose.ScriptMachine.Inputs;
using Mystrose.ScriptMachine.Objects;
using System;
using System.Windows;
using System.Windows.Controls;
using WpfControls = Wpf.Ui.Controls;

namespace Mystrose.ScriptMachine;

/// <summary>
/// An User Control that represents a Script Parameter Input in the client.
/// </summary>
public partial class ScriptParameterInput : UserControl
{

    #region Constructors
    public ScriptParameterInput(ScriptParameterType type, string name, ScriptParameter parameter, ScriptEnginePanel panel, ScriptCommand command, bool isDependable = false)
    {
        InitializeComponent();
        IsDependable = isDependable;
        Panel = panel;
        ParentCommand = command;
        Type = type;
        Name = name;
        Parameter = parameter;
    }
    #endregion

    #region Private Fields
    private ScriptEnginePanel _panel;
    private ScriptCommand _parentCommand;
    private ScriptParameter _parameter;
    private ScriptParameterType _type;
    private ScriptParameterInputType _inputType;
    private string _name;
    private string _tooltip;
    private string _primaryValue;
    private string _secondaryValue;
    private bool _isDependable;
    private Border? _primaryBorder;
    private Border? _secondaryBorder;
    #endregion

    #region Properties
    /// <summary>
    /// The panel of the parameter.
    /// </summary>
    public ScriptEnginePanel Panel
    {
        get => _panel;
        set
        {
            _panel = value;
        }
    }

    /// <summary>
    /// The parent command of the parameter.
    /// </summary>
    public ScriptCommand ParentCommand
    {
        get => _parentCommand;
        set
        {
            _parentCommand = value;
        }
    }

    /// <summary>
    /// The script parameter.
    /// </summary>
    public ScriptParameter Parameter
    {
        get => _parameter;
        set
        {
            _parameter = value;
            SetParameterInfo(value);
        }
    }

    /// <summary>
    /// The type of the script parameter.
    /// </summary>
    public ScriptParameterType Type
    {
        get => _type;
        set
        {
            _type = value;
            SwitchType(value);
        }
    }

    /// <summary>
    /// The type of the script parameter input.
    /// </summary>
    public ScriptParameterInputType InputType
    {
        get => _inputType;
        set
        {
            _inputType = value;
            SwitchInputType(value);
        }
    }

    /// <summary>
    /// The name of the input.
    /// </summary>
    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            NameBlock.Text = _name;
        }
    }

    /// <summary>
    /// The tooltip of the input.
    /// </summary>
    public string Tooltip
    {
        get => _tooltip;
        set
        {
            _tooltip = value;
            TooltipLbl.Content = _tooltip;
        }
    }

    /// <summary>
    /// The primary text of the input.
    /// </summary>
    public string PrimaryValue
    {
        get => _primaryValue;
        set
        {
            _primaryValue = value;
            InputOpt.SelectedValue = _primaryValue;
            InputTxt.Text = _primaryValue;
        }
    }

    /// <summary>
    /// The secondary text of the input.
    /// </summary>
    public string SecondaryValue
    {
        get => _secondaryValue;
        set
        {
            _secondaryValue = value;
            InputCb.SelectedValue = _secondaryValue;
            InputLbl.Text = _secondaryValue;
        }
    }

    /// <summary>
    /// The condition of whether the input is dependable for the Secondary Parameter set.
    /// </summary>
    public bool IsDependable
    {
        get => _isDependable;
        set
        {
            _isDependable = value;
        }
    }

    /// <summary>
    /// The primary border of the input.
    /// </summary>
    public Border? PrimaryBorder
    {
        get => _primaryBorder;
        set
        {
            if (_primaryBorder is not null)
            {
                _primaryBorder.Visibility = Visibility.Collapsed;
            }
            _primaryBorder = value;
            if (value is not null)
            {
                _primaryBorder.Visibility = Visibility.Visible;
            }
        }
    }

    /// <summary>
    /// The secondary border of the input.
    /// </summary>
    public Border? SecondaryBorder
    {
        get => _secondaryBorder;
        set
        {
            if (_secondaryBorder is not null)
            {
                _secondaryBorder.Visibility = Visibility.Collapsed;
            }
            _secondaryBorder = value;
            if (value is not null)
            {
                _secondaryBorder.Visibility = Visibility.Visible;
            }
        }
    }
    #endregion

    #region Methods: Input
    public void RefreshInput()
    {
        switch (InputType)
        {
            case ScriptParameterInputType.Parameter:
                InputTxt.Text = string.Empty;
                break;
            case ScriptParameterInputType.Options:
                InputOpt.SelectedIndex = 0;
                break;
            case ScriptParameterInputType.Conditional:
                InputTxt.Text = string.Empty;
                InputCb.SelectedIndex = 0;
                break;
            case ScriptParameterInputType.KeyValuePair:
                InputTxt.Text = string.Empty;
                InputLbl.Text = string.Empty;
                break;
        }
    }
    #endregion

    #region Methods: Utility
    private void Refresh()
    {
        int index = Panel.ViewType switch
        {
            ScriptCodelineType.Action or ScriptCodelineType.SpecialCommand => Panel.Engine.CurrentStance.Commands.IndexOf(ParentCommand),
            ScriptCodelineType.Trigger => Panel.Engine.CurrentLoadout.Triggers.IndexOf((SCMDTrigger)ParentCommand),
            ScriptCodelineType.Variable => Panel.Engine.CurrentLoadout.PresetVariables.IndexOf((SCMDVariable)ParentCommand),
        };

        if (index == -1)
        {
            return;
        }

        ScriptCodelineItem item = (Panel.CodelinesList.Items[index] as ScriptCodelineItem)!;
        item.Refresh();
    }

    private void SetGridDefs(ScriptParameterInputType inputType)
    {
        InputGrid.ColumnDefinitions.Clear();

        switch (inputType)
        {
            case ScriptParameterInputType.Conditional:
            case ScriptParameterInputType.KeyValuePair:
                InputGrid.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = new GridLength(0.25, GridUnitType.Star)
                });
                break;

            default:
                break;
        }

        InputGrid.ColumnDefinitions.Add(new ColumnDefinition()
        {
            Width = new GridLength(1, GridUnitType.Star)
        });
    }
    #endregion

    #region Methods: Design
    private void SetParameterInfo(ScriptParameter parameter)
    {
        switch (parameter)
        {
            case ScriptKeyValuePair keyValuePair:
                InputType = ScriptParameterInputType.KeyValuePair;
                Tooltip = "The key-value pair input of the parameter. Contains key and value. Its value can be either in String, Integer, Double, or Boolean.";

                PrimaryValue = keyValuePair.Value.ToString()!;
                SecondaryValue = keyValuePair.Key;
                break;
            case ScriptConditional conditional:
                InputType = ScriptParameterInputType.Conditional;
                Tooltip = "The conditional input of the parameter. Contains comparison type and value to be compared with.\n" + "Note: " + conditional.Hint;

                foreach (ScriptConditionalType item in Enum.GetValues<ScriptConditionalType>())
                {
                    InputCb.Items.Add(ScriptRepository.GetConditionString(item));
                }

                PrimaryValue = conditional.ToString()!;
                SecondaryValue = ScriptRepository.GetConditionString((ScriptConditionalType)conditional.Condition!);
                break;
            case ScriptOptions options:
                InputType = ScriptParameterInputType.Options;
                Tooltip = "The selectable input of the parameter. Contains multiple options.\n" + "Note: " + options.Hint;

                foreach (string item in options.GetOptionsList())
                {
                    InputOpt.Items.Add(item);
                }

                PrimaryValue = options.ToString()!;
                break;
            case ScriptParameter value:
                InputType = ScriptParameterInputType.Parameter;
                Tooltip = "The singular input of the parameter, either in String, Integer, Double, or Boolean.\n" + "Note: " + value.Hint;

                PrimaryValue = value.ToString()!;
                break;
        }
    }

    private void SwitchType(ScriptParameterType type)
    {
        RemoveBtn.Visibility = type switch
        {
            ScriptParameterType.Primary or ScriptParameterType.Secondary => Visibility.Collapsed,
            ScriptParameterType.Optional => Visibility.Visible
        };
    }

    private void SwitchInputType(ScriptParameterInputType type)
    {
        SetGridDefs(type);
        PrimaryBorder = type switch
        {
            ScriptParameterInputType.Parameter => InputTxtBdr,
            ScriptParameterInputType.Options => InputOptBdr,
            ScriptParameterInputType.Conditional => InputTxtBdr,
            ScriptParameterInputType.KeyValuePair => InputTxtBdr,
        };
        SecondaryBorder = type switch
        {
            ScriptParameterInputType.Parameter => null,
            ScriptParameterInputType.Options => null,
            ScriptParameterInputType.Conditional => InputCbBdr,
            ScriptParameterInputType.KeyValuePair => InputLblBdr,
        };
    }
    #endregion

    #region Methods: Event
    private void Btn_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not WpfControls.Button btn)
        {
            return;
        }

        switch (btn.Name)
        {
            case "RemoveBtn":
                Panel.RemoveExtraParameter(ParentCommand, this);
                break;
        }
    }

    private void Input_TextChanged(object sender, TextChangedEventArgs e)
    {
        switch (InputType)
        {
            case ScriptParameterInputType.Parameter:
            case ScriptParameterInputType.Conditional:
                Parameter.SetValue(InputTxt.Text);
                break;
            case ScriptParameterInputType.KeyValuePair:
                ScriptKeyValuePair kvp = (ScriptKeyValuePair)Parameter;

                kvp.SetKey(InputLbl.Text);
                kvp.SetValue(InputTxt.Text);
                break;
        }

        Refresh();
    }

    private void Input_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        switch (InputType)
        {
            case ScriptParameterInputType.Options:
                Parameter.SetValue((string)InputOpt.SelectedValue);

                if (IsDependable)
                {
                    Panel.PartialRefreshParameters();
                    Panel.AddParameters(ParentCommand, (ScriptOptions)Parameter);
                }
                break;
            case ScriptParameterInputType.Conditional:
                ScriptConditional cond = (ScriptConditional)Parameter;
                cond.SetCondition((string)InputCb.SelectedValue);
                break;
        }

        Refresh();
    }
    #endregion

}
