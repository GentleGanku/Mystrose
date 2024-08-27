using Clipboard = System.Windows.Clipboard;
using Button = Wpf.Ui.Controls.Button;

namespace Mystrose.Views.Logger;

public partial class VWLogger : MystWindow
{

    #region Constructor
    public VWLogger() : base()
    {
        // Nothing to do here.
    }
    #endregion

    #region (Private) Fields
    private readonly Dictionary<string, List<string>> _logs = new()
    {
        ["Trace"] = [],
        ["Script"] = [],
        ["Exception"] = []
    };
    #endregion

    #region Methods: Pre-setup
    private void Refresh()
    {
        foreach (var logType in _logs.Keys)
        {
            CB_LogTypes.Items.Add(logType);
        }

        _logs["Trace"].AddRange(ConvertStringToList(SVCLogger.GetLogsOnTrace().Output));
        _logs["Script"].AddRange(ConvertStringToList(SVCLogger.GetLogsOnScript().Output));
        _logs["Exception"].AddRange(ConvertStringToList(SVCLogger.GetLogsOnException().Output));

        CB_LogTypes.SelectedIndex = 0;

        MBTN_Scroll.Button.Click += MenuButton_Click;
        MBTN_CopySelected.Button.Click += MenuButton_Click;
        MBTN_CopyAll.Button.Click += MenuButton_Click;
        MBTN_ClearCurrent.Button.Click += MenuButton_Click;

        SVCLogger.LogEvent += AddIncomingLog;
    }

    private void RefreshLogs(int type)
    {
        List<string> logMessages = _logs[_logs.Keys.ElementAt(type)];

        LV_LogMessages.Items.Clear();
        foreach (var msg in logMessages)
        {
            LV_LogMessages.Items.Add(msg);
        }

        RefreshStats();
    }

    private void RefreshStats()
    {
        TB_LogCount.Text = "Count: " + LV_LogMessages.Items.Count;
        TB_SelectedLog.Text = "Selected: " + (LV_LogMessages.SelectedIndex >= 0 ? LV_LogMessages.SelectedIndex : "-");
    }
    #endregion

    #region Methods: Utility
    private List<string> ConvertStringToList(string log)
    {
        List<string> list = [.. log.Split("\r\n")];
        list.RemoveAll(s => string.IsNullOrEmpty(s));

        return list;
    }

    private string ConvertListToString(List<string> logs)
    {
        return logs.Count > 0 ? string.Join("\r\n", logs) : string.Empty;
    }
    #endregion

    #region Methods: Operations
    private void ScrollToSelectedLog()
    {
        if (LV_LogMessages.SelectedIndex < 0)
        {
            NotifyInfo("Menu Action", "No log selected to scroll to.");
            return;
        }

        Response<Action> response = Invoke(() =>
        {
            LV_LogMessages.ScrollIntoView(LV_LogMessages.SelectedItem);
        });

        if (response.IsSuccess)
        {
            NotifySuccess("Menu Action", "Successfully scrolled to selected log.");
        }
        else
        {
            NotifyFailure("Menu Action", "Failed to scroll to selected log.");
        }
    }

    private void CopySelectedLog()
    {
        if (LV_LogMessages.SelectedIndex < 0)
        {
            NotifyInfo("Menu Action", "No log selected to copy.");
            return;
        }

        Response<Action> response = Invoke(() =>
        {
            string selectedLog = LV_LogMessages.SelectedItem.ToString()!;
            Clipboard.SetText(selectedLog);
        });

        if (response.IsSuccess)
        {
            NotifySuccess("Menu Action", "Successfully copied selected log.");
        }
        else
        {
            NotifyFailure("Menu Action", "Failed to copy selected log.");
        }
    }

    private void CopyAllLogs()
    {
        if (LV_LogMessages.Items.Count <= 0)
        {
            NotifyInfo("Menu Action", "No logs found to copy.");
            return;
        }

        Response<Action> response = Invoke(() =>
        {
            string allLogs = ConvertListToString(LV_LogMessages.Items.Cast<string>().ToList());
            Clipboard.SetText(allLogs);
        });

        if (response.IsSuccess)
        {
            NotifySuccess("Menu Action", "Successfully copied all logs from the list.");
        }
        else
        {
            NotifyFailure("Menu Action", "Failed to copy all logs from the list.");
        }
    }

    private void ClearCurrentLog()
    {
        if (LV_LogMessages.Items.Count <= 0)
        {
            NotifyInfo("Menu Action", "No logs found to clear.");
            return;
        }

        Response<Action> response = Invoke(() =>
        {
            _logs[_logs.Keys.ElementAt(CB_LogTypes.SelectedIndex)].Clear();
            LV_LogMessages.Items.Clear();
        });

        if (response.IsSuccess)
        {
            NotifySuccess("Menu Action", "Successfully cleared current log section.");
        }
        else
        {
            NotifyFailure("Menu Action", "Failed to clear current log section.");
        }
    }
    #endregion

    #region Methods: Event Handlers
    private void AddIncomingLog(Response<LogMessage> response)
    {
        if (!response.IsSuccess)
        {
            return;
        }

        string logMessage = response.Output.ToString();

        _logs[_logs.Keys.ElementAt(response.Output.Type)].Add(logMessage);

        if (CB_LogTypes.SelectedIndex == response.Output.Type)
        {
            LV_LogMessages.Items.Add(logMessage);
            RefreshStats();
        }
    }
    #endregion

    #region Methods: Interface Handlers
    private void CB_LogTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        RefreshLogs(CB_LogTypes.SelectedIndex);
    }

    private void LV_LogMessages_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        RefreshStats();
    }
    #endregion

    #region Overrides: Interface
    public override void Destruct()
    {
        SVCLogger.LogEvent -= AddIncomingLog;

        MBTN_Scroll.Button.Click -= MenuButton_Click;
        MBTN_CopySelected.Button.Click -= MenuButton_Click;
        MBTN_CopyAll.Button.Click -= MenuButton_Click;
        MBTN_ClearCurrent.Button.Click -= MenuButton_Click;

        _logs.Clear();
        LV_LogMessages.Items.Clear();
    }
    #endregion

    #region Overrides: Events
    protected override void OnLoaded(object sender, RoutedEventArgs e)
    {
        base.InitializeComponent();
        Refresh();

        SVCLogger.LogOnConsole("VWLogger is ready to go.", "VWLogger", "OnLoaded");
    }

    protected override void OnClosed(EventArgs e)
    {
        Destruct();

        base.OnClosed(e);
        base.Dispose();
    }
    #endregion

    #region Handlers: Events
    private void MenuButton_Click(object sender, RoutedEventArgs e)
    {
        string btnName = ((sender as Button)!.Parent as MenuButton)!.Name;

        switch (btnName)
        {
            case "MBTN_Scroll":
                ScrollToSelectedLog();
                break;

            case "MBTN_CopySelected":
                CopySelectedLog();
                break;

            case "MBTN_CopyAll":
                CopyAllLogs();
                break;

            case "MBTN_ClearCurrent":
                ClearCurrentLog();
                break;
        }
    }
    #endregion

}
