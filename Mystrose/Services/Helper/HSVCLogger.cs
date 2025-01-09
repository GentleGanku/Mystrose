namespace Mystrose.Services.Helper;

public class HSVCLogger() : HelperService(nameof(HSVCLogger))
{

    #region Delegates & Handlers
    public delegate void LogHandler(Response<LogMessage> response);
    public event LogHandler LogEvent;
    #endregion

    #region (Static) Fields
    public static HSVCLogger Instance
    {
        get
        {
            if (_instance is null)
            {
                _instance = new HSVCLogger();
                _instance.Construct();
            }
            
            return _instance;
        }
    }
    #endregion

    #region (Private) Fields
    private static HSVCLogger? _instance;
    #endregion

    #region Fields
    private string PathToFolder => "logs";
    private string PathToPreviousFolder => PathToFolder + "\\previousSession";
    private string PathToTraceLog => PathToFolder + "\\traceLog.txt";
    private string PathToScriptLog => PathToFolder + "\\scriptLog.txt";
    private string PathToExceptionLog => PathToFolder + "\\exceptionLog.txt";
    #endregion

    #region Methods: Builder
    public override void Construct()
    {
        try
        {
            if (!Directory.Exists(PathToFolder))
            {
                Directory.CreateDirectory(PathToFolder);
            }

            Deconstruct();

            if (!File.Exists(PathToTraceLog))
            {
                File.Create(PathToTraceLog).Close();
            }

            if (!File.Exists(PathToScriptLog))
            {
                File.Create(PathToScriptLog).Close();
            }

            if (!File.Exists(PathToExceptionLog))
            {
                File.Create(PathToExceptionLog).Close();
            }

            Log("Logger constructed successfully.", "Construct");
        }
        catch (Exception ex)
        {
            Log(ex.ToString(), "Construct");
        }
    }

    public override void Deconstruct()
    {
        try
        {
            if (!Directory.Exists(PathToPreviousFolder))
            {
                Directory.CreateDirectory(PathToPreviousFolder);
            }

            if (File.Exists(PathToTraceLog))
            {
                File.Move(PathToTraceLog, PathToPreviousFolder + "\\traceLog.txt", true);
                File.Create(PathToTraceLog).Close();
            }

            if (File.Exists(PathToScriptLog))
            {
                File.Move(PathToScriptLog, PathToPreviousFolder + "\\scriptLog.txt", true);
                File.Create(PathToScriptLog).Close();
            }

            if (File.Exists(PathToExceptionLog))
            {
                File.Move(PathToExceptionLog, PathToPreviousFolder + "\\exceptionLog.txt", true);
                File.Create(PathToExceptionLog).Close();
            }

            Log("Logger deconstructed successfully.", "Deconstruct");
        }
        catch (Exception ex)
        {
            Log(ex.ToString(), "Deconstruct");
        }
    }
    #endregion

    #region Methods: Log
    public Response<string> GetLogsOnTrace()
    {
        string logs = ReadFromTraceLog();
        bool isSuccess = !string.IsNullOrEmpty(logs);

        Response<string> response = new(isSuccess,
            isSuccess ? "No logs found in trace log." : "Logs found in trace log.",
            logs);

        return response;
    }

    public Response<string> GetLogsOnScript()
    {
        string logs = ReadFromScriptLog();
        bool isSuccess = !string.IsNullOrEmpty(logs);

        Response<string> response = new(isSuccess,
            isSuccess ? "No logs found in script log." : "Logs found in script log.",
            logs);

        return response;
    }

    public Response<string> GetLogsOnException()
    {
        string logs = ReadFromExceptionLog();
        bool isSuccess = !string.IsNullOrEmpty(logs);

        Response<string> response = new(isSuccess,
            isSuccess ? "No logs found in exception log." : "Logs found in exception log.",
            logs);

        return response;
    }

    public Response<LogMessage> LogOnTrace(string text)
    {
        LogMessage msg = new(0, text);
        bool isWritten = WriteToTraceLog(msg);

        Response<LogMessage> response = new(isWritten,
            isWritten ? "Log written successfully to trace log." : "Failed to write log to trace log.",
            msg);

        LogEvent?.Invoke(response);

        return response;
    }

    public Response<LogMessage> LogOnScript(string text)
    {
        LogMessage msg = new(1, text);
        bool isWritten = WriteToScriptLog(msg);

        Response<LogMessage> response = new(isWritten,
            isWritten ? "Log written successfully to script log." : "Failed to write log to script log.",
            msg);

        LogEvent?.Invoke(response);

        return response;
    }

    public Response<LogMessage> LogOnException(string text)
    {
        LogMessage msg = new(2, text);
        bool isWritten = WriteToExceptionLog(msg);

        Response<LogMessage> response = new(isWritten,
            isWritten ? "Log written successfully to exception log." : "Failed to write log to exception log.",
            msg);

        LogEvent?.Invoke(response);

        return response;
    }

    public Response<LogMessage> LogOnConsole(string text, string hostName = "host", string caller = "default")
    {
        string sourceTemplate = $"{hostName}.{caller}: ";
        LogMessage msg = new(-1, sourceTemplate + text);

        WriteToConsole(msg);

        Response<LogMessage> response = new(true,
            "Log written successfully to console.",
            msg);

        return response;
    }
    #endregion

    #region Methods: Read/Write
    private string ReadFromTraceLog()
    {
        if (!File.Exists(PathToTraceLog))
        {
            return string.Empty;
        }

        return File.ReadAllText(PathToTraceLog);
    }

    private bool WriteToTraceLog(LogMessage message)
    {
        if (!File.Exists(PathToTraceLog))
        {
            return false;
        }

        File.AppendAllText(PathToTraceLog, message.ToString() + Environment.NewLine);
        return true;
    }

    private string ReadFromScriptLog()
    {
        if (!File.Exists(PathToScriptLog))
        {
            return string.Empty;
        }

        return File.ReadAllText(PathToScriptLog);
    }

    private bool WriteToScriptLog(LogMessage message)
    {
        if (!File.Exists(PathToScriptLog))
        {
            return false;
        }

        File.AppendAllText(PathToScriptLog, message.ToString() + Environment.NewLine);
        return true;
    }

    private string ReadFromExceptionLog()
    {
        if (!File.Exists(PathToExceptionLog))
        {
            return string.Empty;
        }

        return File.ReadAllText(PathToExceptionLog);
    }

    private bool WriteToExceptionLog(LogMessage message)
    {
        if (!File.Exists(PathToExceptionLog))
        {
            return false;
        }

        File.AppendAllText(PathToExceptionLog, message.ToString() + Environment.NewLine);
        return true;
    }
    #endregion

    #region Methods: Console
    private void WriteToConsole(LogMessage message)
    {
        Debug.WriteLine(message.ToString());
    }
    #endregion

}