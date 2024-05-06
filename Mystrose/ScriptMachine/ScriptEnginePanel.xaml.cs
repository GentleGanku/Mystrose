using Microsoft.Win32;
using Mystrose.ScriptMachine.Enumerations;
using Mystrose.ScriptMachine.Inputs;
using Mystrose.ScriptMachine.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Common;

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
        ViewType = ScriptCodelineType.Action;

        ActivePanel = Creator_CodelinePnl;
        InactivePanel = Editor_CodelinePnl;

        Creator_CodelineView.Visibility = Visibility.Hidden;
        Creator_CodelineMenu.Visibility = Visibility.Hidden;
        
        RefreshDictionary();
        SetLoadout(new("Loadout"));
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

    public Label CodelineParameterLabel
    {
        get => ActivePanel.Name.Equals("Creator_CodelinePnl") ? Creator_PrmLbl : Editor_PrmLbl;
    }

    public Label CodelineOptParameterLabel
    {
        get => ActivePanel.Name.Equals("Creator_CodelinePnl") ? Creator_OptedPrmLbl : Editor_OptedPrmLbl;
    }

    public UIElementCollection CodelineParameters
    {
        get => ActivePanel.Name.Equals("Creator_CodelinePnl") ? Creator_PrmList.Children : Editor_PrmList.Children;
    }

    public UIElementCollection CodelineOptParameters
    {
        get => ActivePanel.Name.Equals("Creator_CodelinePnl") ? Creator_OptedPrmList.Children : Editor_OptedPrmList.Children;
    }
    #endregion

    #region Properties
    public ScriptDictionary CodelineDict
    {
        get;
        private set;
    }

    public ScriptCodelineType ViewType
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
    #endregion

    #region Methods: View
    public void RefreshCodelines(List<ScriptCommand> commands)
    {
        CodelinesList.Items.Clear();
        foreach (ScriptCommand cmd in commands)
        {
            CodelinesList.Items.Add(new ScriptCodelineItem(this, cmd, CodelinesList.Items.Count + 1));
        }
    }

    public void SwitchCurrentView(ScriptCodelineType type)
    {
        ViewType = type;
        switch (type)
        {
            case ScriptCodelineType.Action:
            case ScriptCodelineType.SpecialCommand:
                StancesBox.Visibility = Visibility.Visible;
                RefreshCodelines(Engine.CurrentStance.Commands);
                break;
            case ScriptCodelineType.Trigger:
                StancesBox.Visibility = Visibility.Collapsed;
                RefreshCodelines([.. Engine.CurrentLoadout.Triggers]);
                break;
            case ScriptCodelineType.Variable:
                StancesBox.Visibility = Visibility.Collapsed;
                RefreshCodelines([.. Engine.CurrentLoadout.PresetVariables]);
                break;
        }
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

        ClearParameters(CodelineParameterLabel, CodelineParameters);
        ClearParameters(CodelineOptParameterLabel, CodelineOptParameters);

        AddParameters(cmd);
    }

    public void SelectCodeline(ScriptCodelineItem item)
    {
        if (EditedCommand == item.Command)
        {
            return;
        }

        SwitchCurrentPanel();

        EditedCommand = item.Command;
        CodelinePosText.Text = item.Command.Type.ToString() + " Command | Index " + item.Index;

        SelectCodeline(item.Command);
    }

    public void AddCodeline(ScriptCodelineType type, ScriptCommand cmd)
    {
        SwitchCurrentView(type);

        switch (type)
        {
            case ScriptCodelineType.Action:
            case ScriptCodelineType.SpecialCommand:
                Engine.CurrentStance.Commands.Add(cmd);
                break;
            case ScriptCodelineType.Trigger:
                Engine.CurrentLoadout.Triggers.Add((SCMDTrigger)cmd);
                break;
            case ScriptCodelineType.Variable:
                Engine.CurrentLoadout.PresetVariables.Add((SCMDVariable)cmd);
                break;
        }

        ScriptCodelineItem item = new(this, cmd, CodelinesList.Items.Count + 1);
        CodelinesList.Items.Add(item);
    }

    public void AddCodeline(ScriptCodelineType type, ScriptCodelineItem item)
    {
        SwitchCurrentView(type);

        switch (type)
        {
            case ScriptCodelineType.Action:
            case ScriptCodelineType.SpecialCommand:
                Engine.CurrentStance.Commands.Add(item.Command);
                break;
            case ScriptCodelineType.Trigger:
                Engine.CurrentLoadout.Triggers.Add((SCMDTrigger)item.Command);
                break;
            case ScriptCodelineType.Variable:
                Engine.CurrentLoadout.PresetVariables.Add((SCMDVariable)item.Command);
                break;
        }

        CodelinesList.Items.Add(item);
    }

    public void RemoveCodeline(ScriptCodelineType type, ScriptCodelineItem item)
    {
        SwitchCurrentView(type);

        switch (type)
        {
            case ScriptCodelineType.Action:
            case ScriptCodelineType.SpecialCommand:
                Engine.CurrentStance.Commands.Remove(item.Command);
                break;
            case ScriptCodelineType.Trigger:
                Engine.CurrentLoadout.Triggers.Remove((SCMDTrigger)item.Command);
                break;
            case ScriptCodelineType.Variable:
                Engine.CurrentLoadout.PresetVariables.Remove((SCMDVariable)item.Command);
                break;
        }

        CodelinesList.Items.Remove(item);
    }

    public void UpdateCodeline(ScriptCodelineType type, ScriptCodelineItem item)
    {
        SwitchCurrentView(type);

        item.Refresh();
    }

    public void MoveCodelineUp()
    {
        CodelinesList.Dispatcher.Invoke(() =>
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
        });
    }

    public void MoveCodelineDown()
    {
        CodelinesList.Dispatcher.Invoke(() =>
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
        });
    }

    public void ClearSelectedCodelines()
    {
        CodelinesList.Dispatcher.Invoke(() =>
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
        });
    }

    public void SelectCodelineType()
    {
        if (CodelineTypeBox.SelectedItem is null)
        {
            return;
        }

        ScriptCodelineType codelineType = (ScriptCodelineType)CodelineTypeBox.SelectedIndex;

        List<ScriptCommand> commands = [];
        foreach (ScriptCommand cmd in CodelineDict[codelineType])
        {
            commands.Add(cmd.Clone());
        }

        CodelineTargetBox.Items.Clear();
        foreach (ScriptCommand cmd in commands)
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

        SelectCodeline((ScriptCommand)CodelineTargetBox.SelectedItem);

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

    public void AddParameters(ScriptCommand cmd, string? optKey = null)
    {
        ScriptParameterType targetType;
        UIElementCollection collection;
        Dictionary<string, ScriptParameter> parameters;

        switch (optKey)
        {
            case null:
                targetType = ScriptParameterType.Parameter;
                collection = CodelineParameters;
                parameters = cmd.Parameters;
                CodelineParameterLabel.Visibility = parameters.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
                break;
            default:
                targetType = ScriptParameterType.SecondaryParameter;
                collection = CodelineOptParameters;
                parameters = cmd.PassSecondaryParameters(optKey);
                CodelineOptParameterLabel.Visibility = parameters.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
                break;
        }

        foreach (KeyValuePair<string, ScriptParameter> keyValuePair in parameters)
        {
            ScriptParameterInput inputItem = new(targetType, keyValuePair.Key, this, cmd, keyValuePair.Value, keyValuePair.Value is ScriptOptions && optKey is null);
            collection.Add(inputItem);
        }
    }
    #endregion

    #region Methods: Loadout
    public void SetLoadout(ScriptLoadout loadout)
    {
        Engine.CurrentLoadout = loadout;

        RefreshStancesList();

        SwitchCurrentView(ScriptCodelineType.Action);
    }
    #endregion

    #region Methods: Stance
    public void SwitchCurrentStance(ScriptStance stance)
    {
        StancesBox.SelectedItem = stance;

        Engine.CurrentStance = stance;

        RefreshCodelines(stance.Commands);
    }

    public void RefreshStancesList()
    {
        StancesBox.Items.Clear();
        foreach (ScriptStance stance in Engine.CurrentLoadout.Stances)
        {
            StancesBox.Items.Add(stance);
        }
        SwitchCurrentStance((ScriptStance)StancesBox.Items[0]);
    }
    #endregion

    #region Methods: Script
    public void LoadScript()
    {
        OpenFileDialog openFileDialog = new()
        {
            Title = "Load Script",
            InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + "\\Scripts\\" + Engine.Type.ToString(),
            DefaultExt = ".json",
            Filter = "Mystrose Scripts|*.json",
            CheckFileExists = true
        };

        if (openFileDialog.ShowDialog() == true)
        {
            try
            {
                string jsonString = File.ReadAllText(openFileDialog.FileName);
                ScriptLoadout loadout = JsonSerializer.Deserialize<ScriptLoadout>(jsonString);

                SetLoadout(loadout);
            }
            catch (Exception ex)
            {
                // TODO: Log error
            }
        }
    }

    public void SaveScript()
    {
        SaveFileDialog saveFileDialog = new()
        {
            Title = "Save Script",
            InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + "\\Scripts\\" + Engine.Type.ToString(),
            DefaultExt = ".json",
            Filter = "Mystrose Scripts|*.json",
            CheckFileExists = false,
            OverwritePrompt = true
        };

        if (saveFileDialog.ShowDialog() == true)
        {
            try
            {
                JsonSerializerOptions options = new()
                {
                    WriteIndented = true
                };

                string jsonString = JsonSerializer.Serialize(Engine.CurrentLoadout, options);
                File.WriteAllText(saveFileDialog.FileName, jsonString);
            }
            catch (Exception ex)
            {
                // TODO: Log error
            }
        }
    }

    public void ToggleScript()
    {
        if (!Engine.IsRunning)
        {
            Engine.StartScript();

            Script_SwitchIcon.Filled = true;
            Script_TempSwitchIcon.Symbol = SymbolRegular.Pause16;
        }
        else
        {
            Engine.StopScript();

            Script_SwitchIcon.Filled = false;
            Script_TempSwitchIcon.Symbol = SymbolRegular.PauseOff16;
        }
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
    }
    #endregion

    #region Methods: Event
    private void View_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Wpf.Ui.Controls.Button btn)
        {
            return;
        }

        SwitchCurrentView(btn.Name switch
        {
            "CommandsBtn" => ScriptCodelineType.Action,
            "TriggersBtn" => ScriptCodelineType.Trigger,
            "VariablesBtn" => ScriptCodelineType.Variable,
        });
    }

    private void Menu_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Wpf.Ui.Controls.Button btn)
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
        if (CodelinesList.SelectionMode is not SelectionMode.Single)
        {
            return;
        }

        SelectCodeline((ScriptCodelineItem)CodelinesList.SelectedItem);
    }

    private void Stance_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is not ComboBox cb)
        {
            return;
        }

        if (cb.SelectedItem is not ScriptStance stance)
        {
            return;
        }

        SwitchCurrentStance(stance);
    }

    private void Return_Click(object sender, RoutedEventArgs e)
    {
        CodelinesList.SelectedItem = null;

        ClearParameters(CodelineParameterLabel, CodelineParameters);
        ClearParameters(CodelineOptParameterLabel, CodelineOptParameters);

        SwitchCurrentPanel();
    }

    private void CodelineMenu_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Wpf.Ui.Controls.Button btn)
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
            case "Editor_CodelineUpdBtn":
                UpdateCodeline(CodelineDict[SelectedCodeline.Command], SelectedCodeline);
                break;

            case "Creator_CodelineInfoBtn":
            case "Editor_CodelineInfoBtn":
                // TODO: Implement codeline info
                break;
            
            case "Creator_ClearPrmBtn":
            case "Editor_ClearPrmBtn":
                RefreshParameters();
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
