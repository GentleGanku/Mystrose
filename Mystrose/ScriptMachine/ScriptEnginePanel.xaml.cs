using Microsoft.Win32;
using Mystrose.ScriptMachine.Controls;
using Mystrose.ScriptMachine.Enumerations;
using Mystrose.ScriptMachine.Inputs;
using Mystrose.ScriptMachine.Interfaces;
using Mystrose.ScriptMachine.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Wpf.Ui.Common;
using WpfControls = Wpf.Ui.Controls;

namespace Mystrose.ScriptMachine;

/// <summary>
/// A window for managing scripts.
/// </summary>
public partial class ScriptEnginePanel : UserControl
{

    #region Constructor
    public ScriptEnginePanel(ScriptEngine engine)
    {
        InitializeComponent();
        Engine = engine;

        ActivePanel = Creator_CodelinePnl;
        InactivePanel = Editor_CodelinePnl;

        SelectedViewIcon = CommandsIcn;
        SelectedViewText = CommandsTxt;

        Creator_CodelineView.Visibility = Visibility.Hidden;
        Creator_CodelineMenu.Visibility = Visibility.Hidden;
        
        RefreshDictionary();
    }
    #endregion

    #region Fields
    public ScriptEngineType EngineType
    {
        get => Engine.Type;
    }

    public ListView CodelinesList
    {
        get => Script_List;
    }

    public Grid InternalListGrid
    {
        get => Script_ListedGrid;
    }

    public TextBlock InternalListName
    {
        get => InternalCmds_MasterTxt;
    }

    public TextBlock InternalListType
    {
        get => InternalCmds_TypeTxt;
    }

    public ComboBox StancesBox
    {
        get => Script_StanceBox;
    }

    public ScriptCodelineItem? SelectedCodeline
    {
        get => CodelinesList.SelectedItem as ScriptCodelineItem;
    }
    #endregion

    #region Active Fields
    public ComboBox CodelineTypeBox
    {
        get => ActivePanel.Name.Equals("Creator_CodelinePnl") ? Creator_CodelineTypeBox : Editor_CodelineTypeBox;
    }

    public ComboBox CodelineTargetBox
    {
        get => ActivePanel.Name.Equals("Creator_CodelinePnl") ? Creator_CodelineTargetBox : Editor_CodelineTargetBox;
    }

    public TextBlock CodelinePosText
    {
        get => ActivePanel.Name.Equals("Creator_CodelinePnl") ? Creator_CodelinePositionTxt : Editor_CodelinePositionTxt;
    }

    public TextBlock CodelineNameText
    {
        get => ActivePanel.Name.Equals("Creator_CodelinePnl") ? Creator_CodelineNameTxt : Editor_CodelineNameTxt;
    }

    public TextBlock CodelineDescText
    {
        get => ActivePanel.Name.Equals("Creator_CodelinePnl") ? Creator_CodelineDescTxt : Editor_CodelineDescTxt;
    }

    public WpfControls.DynamicScrollViewer CodelineScrollViewer
    {
        get => ActivePanel.Name.Equals("Creator_CodelinePnl") ? Creator_PrmScroll : Editor_PrmScroll;
    }

    public Label CodelineParameterLabel
    {
        get => ActivePanel.Name.Equals("Creator_CodelinePnl") ? Creator_PrmLbl : Editor_PrmLbl;
    }

    public Label CodelineOptParameterLabel
    {
        get => ActivePanel.Name.Equals("Creator_CodelinePnl") ? Creator_OptedPrmLbl : Editor_OptedPrmLbl;
    }

    public Label CodelineExtrasLabel
    {
        get => ActivePanel.Name.Equals("Creator_CodelinePnl") ? Creator_MoreOptedPrmLbl : Editor_MoreOptedPrmLbl;
    }

    public UIElementCollection CodelineParameters
    {
        get => ActivePanel.Name.Equals("Creator_CodelinePnl") ? Creator_PrmList.Children : Editor_PrmList.Children;
    }

    public UIElementCollection CodelineOptParameters
    {
        get => ActivePanel.Name.Equals("Creator_CodelinePnl") ? Creator_OptedPrmList.Children : Editor_OptedPrmList.Children;
    }

    public ComboBox CodelineExtrasBox
    {
        get => ActivePanel.Name.Equals("Creator_CodelinePnl") ? Creator_MoreOptedPrmBox : Editor_MoreOptedPrmBox;
    }

    public Button CodelineExtrasBtn
    {
        get => ActivePanel.Name.Equals("Creator_CodelinePnl") ? Creator_MoreOptedPrmBtn : Editor_MoreOptedPrmBtn;
    }
    #endregion

    #region Properties
    public ScriptDictionary CodelineDict
    {
        get;
        private set;
    }

    public ScriptViewType? ViewType
    {
        get;
        private set;
    }

    public ScriptEngine Engine
    {
        get;
        private set;
    }

    public Grid ActivePanel
    {
        get;
        private set;
    }

    public Grid InactivePanel
    {
        get;
        private set;
    }

    public WpfControls.SymbolIcon SelectedViewIcon
    {
        get;
        set;
    }

    public TextBlock SelectedViewText
    {
        get;
        set;
    }

    public ScriptStanceItem SelectedStanceItem
    {
        get;
        set;
    }

    public ScriptCommand? SelectedCommand
    {
        get;
        private set;
    }

    public ScriptCommand? EditedCommand
    {
        get;
        private set;
    }

    public ScriptCommand? SelectedInternalList
    {
        get;
        private set;
    }
    #endregion

    #region Methods: View
    public void RefreshCodelines(List<ScriptCommand> commands)
    {
        Dispatcher.Invoke(() =>
        {
            CodelinesList.Items.Clear();
            foreach (ScriptCommand cmd in commands)
            {
                CodelinesList.Items.Add(new ScriptCodelineItem(this, cmd, CodelinesList.Items.Count + 1));
            }
        });
    }

    public void SwitchCurrentView(ScriptViewType type)
    {
        ViewType = type;

        InternalListGrid.Visibility = Visibility.Collapsed;

        SelectedViewIcon.Filled = false;
        SelectedViewIcon.Foreground = new SolidColorBrush(Colors.White);
        SelectedViewText.Foreground = new SolidColorBrush(Colors.White);

        switch (type)
        {
            case ScriptViewType.Action:
                SelectedViewIcon = CommandsIcn;
                SelectedViewText = CommandsTxt;

                StancesBox.Visibility = Visibility.Visible;
                RefreshCodelines(Engine.CurrentStance.Commands);
                break;
            case ScriptViewType.Trigger:
                SelectedViewIcon = TriggersIcn;
                SelectedViewText = TriggersTxt;

                StancesBox.Visibility = Visibility.Collapsed;
                RefreshCodelines([.. Engine.CurrentLoadout.Triggers]);
                break;
            case ScriptViewType.Variable:
                SelectedViewIcon = VariablesIcn;
                SelectedViewText = VariablesTxt;

                StancesBox.Visibility = Visibility.Collapsed;
                RefreshCodelines([.. Engine.CurrentLoadout.PresetVariables]);
                break;
        }

        SelectedViewIcon.Filled = true;
        SelectedViewIcon.Foreground = new SolidColorBrush(Colors.DodgerBlue);
        SelectedViewText.Foreground = new SolidColorBrush(Colors.DodgerBlue);
    }

    public void SwitchInternalView(ScriptCommand command)
    {
        SelectedInternalList = command;
        SelectedViewIcon.Filled = false;
        SelectedViewIcon.Foreground = new SolidColorBrush(Colors.White);
        SelectedViewText.Foreground = new SolidColorBrush(Colors.White);

        InternalListGrid.Visibility = Visibility.Visible;
        StancesBox.Visibility = Visibility.Collapsed;

        IStackable stackableCmd = (command as IStackable)!;

        InternalListName.Text = stackableCmd.LabelName;
        InternalListType.Text = command.Type.ToString() + " Command";

        RefreshCodelines(stackableCmd.InternalCommands);

        ViewType = ScriptViewType.Listed;
        UnselectCodeline();
    }

    public void RefreshDictionary()
    {
        CodelineDict = new();
        foreach (ScriptCodelineType type in CodelineDict.Keys)
        {
            CodelineTypeBox.Items.Add(JsonSerializer.Serialize(type).Replace("\"", ""));
        }
    }
    #endregion

    #region Methods: Panel
    public void SwitchCurrentPanel()
    {
        Grid grid = ActivePanel;

        ActivePanel = InactivePanel;
        ActivePanel.Visibility = Visibility.Visible;

        InactivePanel = grid;
        InactivePanel.Visibility = Visibility.Collapsed;
    }
    #endregion

    #region Methods: Codeline
    public void SelectCodeline(ScriptCommand cmd)
    {
        SelectedCommand = cmd;

        CodelineNameText.Text = cmd.CommandName;
        CodelineDescText.Text = cmd.CommandDescription;

        CodelineNameText.Foreground = cmd switch
        {
            SCMDAction actionCmd => new SolidColorBrush(Colors.DarkRed),
            SCMDFiller fillerCmd => new SolidColorBrush(Colors.LightGreen),
            SCMDStack listCmd => new SolidColorBrush(Colors.BlueViolet),
            SCMDStatement statementCmd => new SolidColorBrush(Colors.RoyalBlue),
            SCMDTrigger triggerCmd => new SolidColorBrush(Colors.DarkOrange),
            SCMDVariable variableCmd => new SolidColorBrush(Colors.LightPink),

            _ => new SolidColorBrush(Colors.White)
        };

        ClearParameters(CodelineParameterLabel, CodelineParameters);
        ClearParameters(CodelineOptParameterLabel, CodelineOptParameters);

        AddParameters(cmd);
    }

    public void SelectCodeline(ScriptCodelineItem item)
    {
        if (EditedCommand is null)
        {
            SwitchCurrentPanel();
        }

        EditedCommand = item.Command;

        CodelinePosText.Text = item.Command.Type.ToString() + " Command | Index " + item.Index;

        CodelineNameText.Text = item.Command.CommandName;
        CodelineDescText.Text = item.Command.CommandDescription;

        CodelineNameText.Foreground = item.Command switch
        {
            SCMDAction actionCmd => new SolidColorBrush(Colors.DarkRed),
            SCMDFiller fillerCmd => new SolidColorBrush(Colors.LightGreen),
            SCMDStack listCmd => new SolidColorBrush(Colors.BlueViolet),
            SCMDStatement statementCmd => new SolidColorBrush(Colors.RoyalBlue),
            SCMDTrigger triggerCmd => new SolidColorBrush(Colors.DarkOrange),
            SCMDVariable variableCmd => new SolidColorBrush(Colors.LightPink),

            _ => new SolidColorBrush(Colors.White)
        };

        ClearParameters(CodelineParameterLabel, CodelineParameters);
        ClearParameters(CodelineOptParameterLabel, CodelineOptParameters);

        AddParameters(item.Command);
    }

    public void UnselectCodeline()
    {
        CodelinesList.SelectedItem = null;
        EditedCommand = null;

        ClearParameters(CodelineParameterLabel, CodelineParameters);
        ClearParameters(CodelineOptParameterLabel, CodelineOptParameters);

        SwitchCurrentPanel();
    }

    public void AddCodeline(ScriptCodelineType type, ScriptCommand cmd)
    {
        ScriptCommand clonedCmd = cmd switch
        {
            SCMDAction action => action.Clone(),
            SCMDFiller filler => filler.Clone(),
            SCMDStack list => list.Clone(),
            SCMDStatement statement => statement.Clone(),
            SCMDTrigger trigger => trigger.Clone(),
            SCMDVariable variable => variable.Clone()
        };

        ScriptViewType targetedViewType = type switch
        {
            ScriptCodelineType.Action or ScriptCodelineType.SpecialCommand => ScriptViewType.Action,
            ScriptCodelineType.Trigger => ScriptViewType.Trigger,
            ScriptCodelineType.Variable => ScriptViewType.Variable
        };

        switch (targetedViewType)
        {
            case ScriptViewType.Action:
                if (ViewType == ScriptViewType.Listed)
                {
                    AddInternalCodeline(clonedCmd);
                    return;
                }

                Engine.CurrentStance.Commands.Add(clonedCmd);
                break;
            case ScriptViewType.Trigger:
                Engine.CurrentLoadout.Triggers.Add((SCMDTrigger)clonedCmd);
                break;
            case ScriptViewType.Variable:
                Engine.CurrentLoadout.PresetVariables.Add((SCMDVariable)clonedCmd);
                break;
        }

        SwitchCurrentView(targetedViewType);
    }

    public void AddInternalCodeline(ScriptCommand cmd)
    {
        switch (SelectedInternalList)
        {
            case SCMDStack list:
                list.InternalCommands.Add(cmd);
                RefreshCodelines(list.InternalCommands);
                break;
            case SCMDTrigger trigger:
                trigger.InternalCommands.Add(cmd);
                RefreshCodelines(trigger.InternalCommands);
                break;
        }
    }

    public void RemoveCodeline(ScriptCodelineType type, ScriptCodelineItem item)
    {
        ScriptViewType targetedViewType = type switch
        {
            ScriptCodelineType.Action or ScriptCodelineType.SpecialCommand => ScriptViewType.Action,
            ScriptCodelineType.Trigger => ScriptViewType.Trigger,
            ScriptCodelineType.Variable => ScriptViewType.Variable
        };

        switch (targetedViewType)
        {
            case ScriptViewType.Action:
                if (ViewType == ScriptViewType.Listed)
                {
                    RemoveInternalCodeline(item);
                    return;
                }

                Engine.CurrentStance.Commands.Remove(item.Command);
                break;
            case ScriptViewType.Trigger:
                Engine.CurrentLoadout.Triggers.Remove((SCMDTrigger)item.Command);
                break;
            case ScriptViewType.Variable:
                Engine.CurrentLoadout.PresetVariables.Remove((SCMDVariable)item.Command);
                break;
        }

        UnselectCodeline();
        SwitchCurrentView(targetedViewType);    
    }

    public void RemoveInternalCodeline(ScriptCodelineItem item)
    {
        switch (SelectedInternalList)
        {
            case SCMDStack list:
                list.InternalCommands.Remove(item.Command);
                RefreshCodelines(list.InternalCommands);
                break;
            case SCMDTrigger trigger:
                trigger.InternalCommands.Remove(item.Command);
                RefreshCodelines(trigger.InternalCommands);
                break;
        }

        UnselectCodeline();
    }

    public void MoveCodelineUp()
    {
        foreach (ScriptCodelineItem item in CodelinesList.SelectedItems)
        {
            if (item.Index == 0)
            {
                break;
            }

            int index = item.Index - 1;

            CodelinesList.Items.RemoveAt(index);
            CodelinesList.Items.Insert(index - 1, item);
            Engine.CurrentStance.Commands.RemoveAt(index);
            Engine.CurrentStance.Commands.Insert(index - 1, Engine.CurrentStance.Commands[index]);

            item.Index--;
        }
    }

    public void MoveCodelineDown()
    {
        foreach (ScriptCodelineItem item in CodelinesList.SelectedItems)
        {
            if (item.Index == CodelinesList.Items.Count - 1)
            {
                break;
            }

            int index = item.Index - 1;

            CodelinesList.Items.RemoveAt(index);
            CodelinesList.Items.Insert(index + 1, item);
            Engine.CurrentStance.Commands.RemoveAt(index);
            Engine.CurrentStance.Commands.Insert(index + 1, Engine.CurrentStance.Commands[index]);

            item.Index++;
        }
    }

    public void ClearSelectedCodelines()
    {
        if (CodelinesList.SelectedItems.Count > 0)
        {
            foreach (ScriptCodelineItem item in CodelinesList.SelectedItems)
            {
                CodelinesList.Items.Remove(item);
                Engine.CurrentStance.Commands.Remove(item.Command);
            }
        }
        else
        {
            CodelinesList.Items.Clear();
            Engine.CurrentStance.Commands.Clear();
        }
    }

    public void SelectCodelineType()
    {
        if (CodelineTypeBox.SelectedItem is null)
        {
            return;
        }

        ScriptCodelineType codelineType = (ScriptCodelineType)CodelineTypeBox.SelectedIndex;

        CodelineTargetBox.Items.Clear();
        foreach (ScriptCommand cmd in CodelineDict[codelineType])
        {
            CodelineTargetBox.Items.Add(cmd);
        }

        if (CodelineTargetBox.Items.Count == 1)
        {
            CodelineTargetBox.SelectedIndex = 0;
            CodelineTargetBox.IsEnabled = false;
        }
        else
        {
            CodelineTargetBox.IsEnabled = true;
        }
    }

    public void SelectCodelineTarget()
    {
        if (CodelineTargetBox.SelectedItem is null)
        {
            return;
        }

        ScriptCommand cmd = (ScriptCommand)CodelineTargetBox.SelectedItem;
        SelectCodeline(cmd);

        Creator_CodelineView.Visibility = Visibility.Visible;
        Creator_CodelineMenu.Visibility = Visibility.Visible;
    }
    #endregion

    #region Methods: Parameter
    public void ClearParameters(Label label, UIElementCollection collection)
    {
        collection.Clear();
        label.Visibility = Visibility.Collapsed;
    }

    public void PartialRefreshParameters()
    {
        ClearParameters(CodelineOptParameterLabel, CodelineOptParameters);
    }

    public void RefreshParameters()
    {
        ClearParameters(CodelineOptParameterLabel, CodelineOptParameters);

        foreach (ScriptParameterInput input in CodelineParameters)
        {
            input.RefreshInput();
        }
    }

    public void AddParameters(ScriptCommand cmd)
    {
        RefreshExtraParameters(string.Empty, []);

        Dictionary<string, ScriptParameter> parameters = cmd.Parameters;

        CodelineParameterLabel.Visibility = parameters.Count > 0 ? Visibility.Visible : Visibility.Collapsed;

        foreach (KeyValuePair<string, ScriptParameter> keyValuePair in parameters)
        {
            ScriptParameterInput inputItem = new(ScriptParameterType.Primary, keyValuePair.Key, keyValuePair.Value, this, cmd, keyValuePair.Value is ScriptOptions);
            CodelineParameters.Add(inputItem);
        }
    }

    public void AddParameters(ScriptCommand cmd, ScriptOptions options)
    {
        string selectedOpt = options.String;
        Dictionary<string, ScriptParameter> parameters = cmd.PassSecondaryParameters(options.String);
        bool isOptionalCoded = cmd.SecondaryParameters.ContainsKey("Optional");

        CodelineOptParameterLabel.Visibility = parameters.Count > 0 ? Visibility.Visible : Visibility.Collapsed;

        foreach (KeyValuePair<string, ScriptParameter> keyValuePair in parameters)
        {
            ScriptParameterInput inputItem = new(isOptionalCoded ? ScriptParameterType.Optional : ScriptParameterType.Secondary, keyValuePair.Key, keyValuePair.Value, this, cmd, false);
            CodelineOptParameters.Add(inputItem);
        }

        if (isOptionalCoded)
        {
            RefreshExtraParameters(selectedOpt, cmd.SecondaryParameters["Optional"]);
        }
    }

    public void RefreshExtraParameters(string key, Dictionary<string, ScriptParameter> extras)
    {
        CodelineExtrasLabel.Tag = key;

        if (extras.Count <= 0)
        {
            CodelineExtrasLabel.Visibility = Visibility.Collapsed;
            CodelineExtrasBox.Visibility = Visibility.Collapsed;
            CodelineExtrasBtn.Visibility = Visibility.Collapsed;
            return;
        }

        CodelineExtrasLabel.Visibility = Visibility.Visible;
        CodelineExtrasBox.Visibility = Visibility.Visible;
        CodelineExtrasBtn.Visibility = Visibility.Visible;

        CodelineExtrasBox.Items.Clear();
        foreach (string extraPrmKey in extras.Keys)
        {
            CodelineExtrasBox.Items.Add(extraPrmKey);
        }

        CodelineExtrasBox.SelectedIndex = 0;
    }

    public void AddExtraParameter(ScriptCommand cmd)
    {
        string optKey = (string)CodelineExtrasLabel.Tag;
        string prmName = (string)CodelineExtrasBox.SelectedValue;

        ScriptParameter prm = cmd.SecondaryParameters["Optional"][prmName];
        ScriptParameterInput prmInput = new(ScriptParameterType.Optional, prmName, prm, this, cmd, false);

        cmd.SecondaryParameters["Optional"].Remove(prmName);
        cmd.SecondaryParameters[optKey].Add(prmName, prm);

        CodelineOptParameters.Add(prmInput);
        CodelineOptParameterLabel.Visibility = Visibility.Visible;

        CodelineExtrasBox.Items.Remove(prmName);
        CodelineExtrasBox.SelectedIndex = 0;

        CodelineScrollViewer.ScrollToEnd();
    }

    public void RemoveExtraParameter(ScriptCommand cmd, ScriptParameterInput input)
    {
        string optKey = (string)CodelineExtrasLabel.Tag;
        string prmName = input.Name;

        ScriptParameter prm = input.Parameter;

        cmd.SecondaryParameters["Optional"].Add(prmName, prm);
        cmd.SecondaryParameters[optKey].Remove(prmName);

        CodelineOptParameters.Remove(input);
        CodelineOptParameterLabel.Visibility = CodelineOptParameters.Count > 0 ? Visibility.Visible : Visibility.Collapsed;

        CodelineExtrasBox.Items.Add(input.Name);
        CodelineExtrasBox.SelectedIndex = 0;

        CodelineScrollViewer.ScrollToEnd();
    }
    #endregion

    #region Methods: Loadout
    public void SetLoadout(ScriptLoadout loadout)
    {
        Engine.CurrentLoadout = loadout;
        RefreshStancesList();
    }
    #endregion

    #region Methods: Stance
    public void SwitchStance(ScriptStanceItem stanceItem)
    {
        if (SelectedStanceItem is not null)
        {
            SelectedStanceItem.ToggleButtonMode(false);
        }

        SelectedStanceItem = stanceItem;
        SelectedStanceItem.ToggleButtonMode(true);

        Engine.CurrentStance = stanceItem.Stance;

        SwitchCurrentView(ScriptViewType.Action);
    }

    public void SelectStance(ScriptStanceItem stanceItem)
    {
        StancesBox.SelectedItem = stanceItem;
    }

    public void RefreshStancesList()
    {
        StancesBox.Items.Clear();

        foreach (ScriptStance stance in Engine.CurrentLoadout.Stances)
        {
            ScriptStanceItem item = new(this, stance);

            StancesBox.Items.Add(item);
        }

        SelectStance((StancesBox.Items[0] as ScriptStanceItem)!);
    }

    public void AddStance(string label)
    {
        if (string.IsNullOrEmpty(label) || Engine.CurrentLoadout.Stances.Find(s => s.Name == label) is ScriptStance existingStance)
        {
            return;
        }

        ScriptStance stance = new(label);
        ScriptStanceItem stanceItem = new(this, stance);

        Engine.CurrentLoadout.Stances.Add(stance);
        StancesBox.Items.Add(stanceItem);

        SelectStance(stanceItem);
    }

    public void RemoveStance(ScriptStanceItem item)
    {
        Engine.CurrentLoadout.Stances.Remove(item.Stance);
        StancesBox.Items.Remove(item);
    }
    #endregion

    #region Methods: Script
    public void LoadScript()
    {
        OpenFileDialog openFileDialog = new()
        {
            Title = "Load Script",
            InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + "Scripts\\" + Engine.Type.ToString(),
            DefaultExt = ".json",
            Filter = "Mystrose Scripts|*.json",
            CheckFileExists = true
        };

        if (openFileDialog.ShowDialog() == true)
        {
                string jsonString = File.ReadAllText(openFileDialog.FileName);
                ScriptLoadout loadout = ScriptRepository.ConvertToLoadout(jsonString);

                SetLoadout(loadout);
        }
    }

    public void SaveScript()
    {
        SaveFileDialog saveFileDialog = new()
        {
            Title = "Save Script",
            InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + "Scripts\\" + Engine.Type.ToString(),
            FileName = Engine.CurrentLoadout.Name + ".json",
            DefaultExt = ".json",
            Filter = "Mystrose Scripts|*.json",
            CheckFileExists = false,
            OverwritePrompt = true
        };

        if (saveFileDialog.ShowDialog() == true)
        {
            try
            {
                string jsonString = ScriptRepository.ConvertFromLoadout(Engine.CurrentLoadout);
                File.WriteAllText(saveFileDialog.FileName, jsonString);
            }
            catch (Exception e)
            {
                // TODO: Log error
            }
        }
    }

    public async void ToggleScript()
    {
        if (!Engine.IsRunning)
        {
            Engine.ScriptRunHandler += OnScriptRun;
            Engine.ScriptResultHandler += OnScriptResult;
            Engine.StartScript();

            Script_SwitchIcon.Filled = true;
            Script_TempSwitchIcon.Symbol = SymbolRegular.Pause16;
        }
        else
        {
            Engine.ScriptRunHandler -= OnScriptRun;
            Engine.ScriptResultHandler -= OnScriptResult;
            Engine.StopScript();

            Script_SwitchIcon.Filled = false;
            Script_TempSwitchIcon.Symbol = SymbolRegular.PauseOff16;
        }

        ToggleButtons();
    }

    public void TemporaryToggleScript()
    {
        if (!Engine.IsPaused)
        {
            Engine.PauseScript();

            Script_TempSwitchIcon.Filled = true;
        }
        else
        {
            Engine.ResumeScript();

            Script_SwitchIcon.Filled = false;
        }

        ToggleButtons();
    }

    public async void ToggleButtons()
    {
        Script_SwitchBtn.Opacity = 0.25;
        Script_SwitchBtn.IsEnabled = false;

        Script_TempSwitchBtn.Opacity = 0.25;
        Script_TempSwitchBtn.IsEnabled = false;

        await Task.Delay(2000);

        Script_SwitchBtn.Opacity = 1;
        Script_SwitchBtn.IsEnabled = true;

        Script_TempSwitchBtn.Opacity = 1;
        Script_TempSwitchBtn.IsEnabled = true;
    }
    #endregion

    #region Methods: Delegate
    public void OnScriptRun(ScriptCommand cmd)
    {
        Dispatcher.Invoke(() =>
        {
            switch (ViewType)
            {
                case ScriptViewType.Action:
                    int index = Engine.CurrentStance.Commands.IndexOf(cmd);
                    CodelinesList.SelectedIndex = index;
                    break;
            }
        });
    }

    public void OnScriptResult(ScriptResultType type, string msg = "")
    {
        Dispatcher.Invoke(() =>
        {
            switch (type)
            {
                case ScriptResultType.Busy:
                    break;
                case ScriptResultType.Failure:
                    break;
                case ScriptResultType.Success:
                    break;
                case ScriptResultType.Cancel:
                    Script_SwitchIcon.Filled = false;
                    Script_TempSwitchIcon.Symbol = SymbolRegular.PauseOff16;

                    ToggleButtons();
                    break;
                case ScriptResultType.Error:
                    Script_SwitchIcon.Filled = false;
                    Script_TempSwitchIcon.Symbol = SymbolRegular.PauseOff16;

                    ToggleButtons();

                    System.Diagnostics.Debug.WriteLine(msg);
                    break;
            }
        });
    }
    #endregion

    #region Methods: Event
    private void Engine_Loaded(object sender, RoutedEventArgs e)
    {
        SetLoadout(new("Loadout"));
    }

    private void View_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not WpfControls.Button btn)
        {
            return;
        }

        SwitchCurrentView(btn.Name switch
        {
            "CommandsBtn" => ScriptViewType.Action,
            "TriggersBtn" => ScriptViewType.Trigger,
            "VariablesBtn" => ScriptViewType.Variable,
        });
    }

    private void Menu_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not WpfControls.Button btn)
        {
            return;
        }

        switch (btn.Name)
        {
            case "Script_SwitchBtn":
                ToggleScript();
                break;
            case "Script_TempSwitchBtn":
                TemporaryToggleScript();
                break;

            case "Script_SearchBtn":
                // TODO: Implement search
                break;
            case "Script_MoveUpBtn":
                MoveCodelineUp();
                break;
            case "Script_MoveDownBtn":
                MoveCodelineDown();
                break;
            case "Script_ClearBtn":
                ClearSelectedCodelines();
                break;
            case "Script_LoadBtn":
                LoadScript();
                break;
            case "Script_SaveBtn":
                SaveScript();
                break;
            case "Script_InfoBtn":
                // TODO: Implement info
                break;
            case "Script_NotesBtn":
                // TODO: Implement notes
                break;
        }
    }

    private void CodelineItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (SelectedCodeline is null)
        {
            return;
        }

        if (Engine.IsRunning)
        {
            return;
        }

        if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
        {
            System.Diagnostics.Debug.WriteLine("Sex");
        }

        SelectCodeline(SelectedCodeline);
    }

    private void Stance_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is not ComboBox cb)
        {
            return;
        }

        if (cb.SelectedItem is not ScriptStanceItem item)
        {
            return;
        }

        SwitchStance(item);
    }

    private void Stance_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            AddStance(StancesBox.Text);
        }
    }

    private void InternalExit_Click(object sender, RoutedEventArgs e)
    {
        SwitchCurrentView(SelectedViewText.Text switch
        {
            "Commands" => ScriptViewType.Action,
            "Triggers" => ScriptViewType.Trigger,
            "Variables" => ScriptViewType.Variable,
            _ => ScriptViewType.Action
        });
        
        foreach (ScriptCodelineItem item in CodelinesList.Items)
        {
            if (item.Command == SelectedCommand)
            {
                CodelinesList.SelectedItem = item;
                break;
            }
        }
    }

    private void Return_Click(object sender, RoutedEventArgs e)
    {
        UnselectCodeline();
    }

    private void CodelineMenu_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not WpfControls.Button btn)
        {
            return;
        }
        
        switch (btn.Name)
        {
            case "Creator_CodelineAddBtn":
                AddCodeline((ScriptCodelineType)CodelineTypeBox.SelectedIndex, SelectedCommand);
                break;
            case "Editor_CodelineRemBtn":
                RemoveCodeline(CodelineDict[SelectedCodeline.Command], SelectedCodeline);
                break;

            case "Creator_CodelineInfoBtn":
            case "Editor_CodelineInfoBtn":
                // TODO: Implement codeline info
                break;
            
            case "Creator_ClearPrmBtn":
            case "Editor_ClearPrmBtn":
                RefreshParameters();
                break;

            case "Creator_MoreOptedPrmBtn":
                AddExtraParameter(SelectedCommand);
                break;
            case "Editor_MoreOptedPrmBtn":
                AddExtraParameter(EditedCommand);
                break;
        }
    }

    private void CodelineBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is not ComboBox cb)
        {
            return;
        }

        switch (cb.Name)
        {
            case "Creator_CodelineTypeBox":
                SelectCodelineType();
                break;
            case "Creator_CodelineTargetBox":
                SelectCodelineTarget();
                break;
        }
    }
    #endregion

}
