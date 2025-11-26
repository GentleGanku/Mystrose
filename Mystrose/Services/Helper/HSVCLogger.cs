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
    private string _pathToFolder => "logs";
    private string _pathToPreviousFolder => _pathToFolder + "\\previousSession";
    private string _pathToTraceLog => _pathToFolder + "\\traceLog.txt";
    private string _pathToExceptionLog => _pathToFolder + "\\exceptionLog.txt";
    private string _pathToScriptLog => _pathToFolder + "\\scriptLog-[CODENAME].txt";

    private string[] _traceableCodenames =
    [
        "Avernus",
        "Beatrix",
        "Cassiopeia",
        "Durandal",
        "Eligos",
        "Fenrir",
        "Gwyndell",
        "Harbinger"
    ];
    #endregion

    #region Methods: Builder
    public override void Construct()
    {
        try
        {
            if (!Directory.Exists(_pathToFolder))
            {
                Directory.CreateDirectory(_pathToFolder);
            }

            Deconstruct();

            if (!File.Exists(_pathToTraceLog))
            {
                File.Create(_pathToTraceLog).Close();
            }

            if (!File.Exists(_pathToExceptionLog))
            {
                File.Create(_pathToExceptionLog).Close();
            }

            foreach (string codename in _traceableCodenames)
            {
                string specificPath = _pathToScriptLog.Replace("[CODENAME]", codename);

                if (!File.Exists(specificPath))
                {
                    File.Create(specificPath).Close();
                }
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
            if (!Directory.Exists(_pathToPreviousFolder))
            {
                Directory.CreateDirectory(_pathToPreviousFolder);
            }

            if (File.Exists(_pathToTraceLog))
            {
                File.Move(_pathToTraceLog, _pathToPreviousFolder + "\\traceLog.txt", true);
                File.Create(_pathToTraceLog).Close();
            }

            if (File.Exists(_pathToExceptionLog))
            {
                File.Move(_pathToExceptionLog, _pathToPreviousFolder + "\\exceptionLog.txt", true);
                File.Create(_pathToExceptionLog).Close();
            }

            foreach (string codename in _traceableCodenames)
            {
                string specificPath = _pathToScriptLog.Replace("[CODENAME]", codename);

                if (File.Exists(specificPath))
                {
                    File.Move(specificPath, _pathToPreviousFolder + $"\\scriptLog-{codename}.txt", true);
                    File.Create(specificPath).Close();
                }
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

    public Response<string> GetLogsOnException()
    {
        string logs = ReadFromExceptionLog();
        bool isSuccess = !string.IsNullOrEmpty(logs);

        Response<string> response = new(isSuccess,
            isSuccess ? "No logs found in exception log." : "Logs found in exception log.",
            logs);

        return response;
    }

    public Response<string> GetLogsOnScript(string codename)
    {
        string logs = ReadFromScriptLog(codename);
        bool isSuccess = !string.IsNullOrEmpty(logs);

        Response<string> response = new(isSuccess,
            isSuccess ? "No logs found in script log." : "Logs found in script log.",
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

    public Response<LogMessage> LogOnScript(string codename, string text)
    {
        LogMessage msg = new(1, text);
        bool isWritten = WriteToScriptLog(codename, msg);

        Response<LogMessage> response = new(isWritten,
            isWritten ? "Log written successfully to script log." : "Failed to write log to script log.",
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
        if (!File.Exists(_pathToTraceLog))
        {
            return string.Empty;
        }

        return File.ReadAllText(_pathToTraceLog);
    }

    private bool WriteToTraceLog(LogMessage message)
    {
        if (!File.Exists(_pathToTraceLog))
        {
            return false;
        }

        File.AppendAllText(_pathToTraceLog, message.ToString() + Environment.NewLine);
        return true;
    }

    private string ReadFromExceptionLog()
    {
        if (!File.Exists(_pathToExceptionLog))
        {
            return string.Empty;
        }

        return File.ReadAllText(_pathToExceptionLog);
    }

    private bool WriteToExceptionLog(LogMessage message)
    {
        if (!File.Exists(_pathToExceptionLog))
        {
            return false;
        }

        File.AppendAllText(_pathToExceptionLog, message.ToString() + Environment.NewLine);
        return true;
    }

    private string ReadFromScriptLog(string codename)
    {
        string specificPath = _pathToScriptLog.Replace("[CODENAME]", codename);

        if (!File.Exists(specificPath))
        {
            return string.Empty;
        }

        return File.ReadAllText(specificPath);
    }

    private bool WriteToScriptLog(string codename, LogMessage message)
    {
        string specificPath = _pathToScriptLog.Replace("[CODENAME]", codename);

        if (!File.Exists(specificPath))
        {
            return false;
        }

        File.AppendAllText(specificPath, message.ToString() + Environment.NewLine);
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