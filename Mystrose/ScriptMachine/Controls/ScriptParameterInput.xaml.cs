using Mystrose.ScriptMachine.Enumerations;
using Mystrose.ScriptMachine.Inputs;
using Mystrose.ScriptMachine.Objects;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Mystrose.ScriptMachine;

/// <summary>
/// An User Control that represents a Script Parameter Input in the client.
/// </summary>
public partial class ScriptParameterInput : UserControl
{

    #region Constructors
    public ScriptParameterInput(ScriptParameterType type, string name, ScriptEnginePanel panel, ScriptCommand command, ScriptParameter parameter, bool isDependable = false)
    {
        InitializeComponent();
        IsDependable = isDependable;
        Panel = panel;
        ParentCommand = command;
        Parameter = parameter;
        Type = type;
        Name = name;
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
    private string _primaryPlaceHolderText;
    private string _secondaryPlaceHolderText;
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
    /// The primary placeholder text of the input.
    /// </summary>
    public string PrimaryPlaceHolderText
    {
        get => _primaryPlaceHolderText;
        set
        {
            _primaryPlaceHolderText = value;
            InputOpt.Text = _primaryPlaceHolderText;
            InputTxt.Text = _primaryPlaceHolderText;
        }
    }

    /// <summary>
    /// The secondary placeholder text of the input.
    /// </summary>
    public string SecondaryPlaceHolderText
    {
        get => _secondaryPlaceHolderText;
        set
        {
            _secondaryPlaceHolderText = value;
            InputCb.Text = _secondaryPlaceHolderText;
            InputLbl.Text = _secondaryPlaceHolderText;
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
                InputTxt.Text = PrimaryPlaceHolderText;
                break;
            case ScriptParameterInputType.Options:
                InputOpt.SelectedItem = PrimaryPlaceHolderText;
                break;
            case ScriptParameterInputType.Conditional:
                InputTxt.Text = PrimaryPlaceHolderText;
                InputCb.SelectedItem = SecondaryPlaceHolderText;
                break;
            case ScriptParameterInputType.KeyValuePair:
                InputTxt.Text = PrimaryPlaceHolderText;
                InputLbl.Text = SecondaryPlaceHolderText;
                break;
        }
    }
    #endregion

    #region Methods: Utility
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
                PrimaryPlaceHolderText = keyValuePair.PlaceholderText;
                SecondaryPlaceHolderText = "Key name";
                break;
            case ScriptConditional conditional:
                InputType = ScriptParameterInputType.Conditional;
                Tooltip = "The conditional input of the parameter. Contains comparison type and value to be compared with.\n" + "Note: " + conditional.Hint;
                PrimaryPlaceHolderText = conditional.PlaceholderText;
                foreach (ScriptConditionalType item in Enum.GetValues<ScriptConditionalType>())
                {
                    InputCb.Items.Add(ScriptRepository.GetConditionString(item));
                }
                SecondaryPlaceHolderText = InputCb.Items[0].ToString();
                break;
            case ScriptOptions options:
                InputType = ScriptParameterInputType.Options;
                Tooltip = "The selectable input of the parameter. Contains multiple options.\n" +
                    "Note: " + options.Hint;
                foreach (string item in options.List)
                {
                    InputOpt.Items.Add(item);
                }
                PrimaryPlaceHolderText = options.String;
                break;
            case ScriptParameter value:
                InputType = ScriptParameterInputType.Parameter;
                Tooltip = "The singular input of the parameter, either in String, Integer, Double, or Boolean.\n" + "Note: " + value.Hint;
                PrimaryPlaceHolderText = value.PlaceholderText;
                break;
        }
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
    }

    private void Input_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        switch (InputType)
        {
            case ScriptParameterInputType.Options:
                Parameter.SetValue(InputOpt.SelectedItem);

                if (IsDependable)
                {
                    Panel.PartialRefreshParameters();
                    Panel.AddParameters(ParentCommand, (string)InputOpt.SelectedItem);
                }
                break;
            case ScriptParameterInputType.Conditional:
                ScriptConditional cond = (ScriptConditional)Parameter;
                cond.SetCondition((string)InputCb.SelectedItem);
                break;
        }
    }
    #endregion

}
