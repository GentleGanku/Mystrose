namespace Mystrose.Services;

public class SVCLogger
{

    #region Delegates & Handlers
    public delegate void LogHandler(Response<LogMessage> response);
    public static event LogHandler LogEvent;
    #endregion

    #region Fields
    private static string _pathToFolder => "logs";
    private static string _pathToTraceLog => _pathToFolder + "\\traceLog.txt";
    private static string _pathToScriptLog => _pathToFolder + "\\scriptLog.txt";
    private static string _pathToExceptionLog => _pathToFolder + "\\exceptionLog.txt";
    #endregion

    #region Methods: Filing
    public static void Checkup()
    {
        try
        {
            if (!Directory.Exists(_pathToFolder))
            {
                Directory.CreateDirectory(_pathToFolder);
            }

            if (!File.Exists(_pathToTraceLog))
            {
                File.Create(_pathToTraceLog).Close();
            }

            if (!File.Exists(_pathToScriptLog))
            {
                File.Create(_pathToScriptLog).Close();
            }

            if (!File.Exists(_pathToExceptionLog))
            {
                File.Create(_pathToExceptionLog).Close();
            }

            LogOnConsole("Logger checkup completed.", "SVCLogger", "Checkup");
        }
        catch (Exception ex)
        {
            LogOnConsole(ex.ToString(), "SVCLogger", "Checkup");
        }
    }

    public static void Purge()
    {
        try
        {
            string _pathToPreviousFolder = "logs\\previousSession";
            if (!Directory.Exists(_pathToPreviousFolder))
            {
                Directory.CreateDirectory(_pathToPreviousFolder);
            }

            if (File.Exists(_pathToTraceLog))
            {
                File.Move(_pathToTraceLog, _pathToPreviousFolder + "\\traceLog.txt", true);
                File.Create(_pathToTraceLog).Close();
            }

            if (File.Exists(_pathToScriptLog))
            {
                File.Move(_pathToScriptLog, _pathToPreviousFolder + "\\scriptLog.txt", true);
                File.Create(_pathToScriptLog).Close();
            }

            if (File.Exists(_pathToExceptionLog))
            {
                File.Move(_pathToExceptionLog, _pathToPreviousFolder + "\\exceptionLog.txt", true);
                File.Create(_pathToExceptionLog).Close();
            }

            LogOnConsole("Logger purge completed.", "SVCLogger", "Purge");
        }
        catch (Exception ex)
        {
            LogOnConsole(ex.ToString(), "SVCLogger", "Purge");
        }
    }
    #endregion

    #region Methods: Log
    public static Response<string> GetLogsOnTrace()
    {
        string logs = ReadFromTraceLog();
        bool isSuccess = !string.IsNullOrEmpty(logs);

        Response<string> response = new(isSuccess,
            isSuccess ? "No logs found in trace log." : "Logs found in trace log.",
            logs);

        return response;
    }

    public static Response<LogMessage> LogOnTrace(string text)
    {
        LogMessage msg = new(0, text);
        bool isWritten = WriteToTraceLog(msg);

        Response<LogMessage> response = new(isWritten,
            isWritten ? "Log written successfully to trace log." : "Failed to write log to trace log.",
            msg);

        LogEvent.Invoke(response);

        return response;
    }

    public static Response<string> GetLogsOnScript()
    {
        string logs = ReadFromScriptLog();
        bool isSuccess = !string.IsNullOrEmpty(logs);

        Response<string> response = new(isSuccess,
            isSuccess ? "No logs found in script log." : "Logs found in script log.",
            logs);

        return response;
    }

    public static Response<LogMessage> LogOnScript(string text)
    {
        LogMessage msg = new(1, text);
        bool isWritten = WriteToScriptLog(msg);

        Response<LogMessage> response = new(isWritten,
            isWritten ? "Log written successfully to script log." : "Failed to write log to script log.",
            msg);

        LogEvent.Invoke(response);

        return response;
    }

    public static Response<string> GetLogsOnException()
    {
        string logs = ReadFromExceptionLog();
        bool isSuccess = !string.IsNullOrEmpty(logs);

        Response<string> response = new(isSuccess,
            isSuccess ? "No logs found in exception log." : "Logs found in exception log.",
            logs);

        return response;
    }

    public static Response<LogMessage> LogOnException(string text)
    {
        LogMessage msg = new(2, text);
        bool isWritten = WriteToExceptionLog(msg);

        Response<LogMessage> response = new(isWritten,
            isWritten ? "Log written successfully to exception log." : "Failed to write log to exception log.",
            msg);

        LogEvent.Invoke(response);

        return response;
    }

    public static Response<LogMessage> LogOnConsole(string text, string hostName = "host", string caller = "default")
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
    private static string ReadFromTraceLog()
    {
        if (!File.Exists(_pathToTraceLog))
        {
            return string.Empty;
        }

        return File.ReadAllText(_pathToTraceLog);
    }

    private static bool WriteToTraceLog(LogMessage message)
    {
        if (!File.Exists(_pathToTraceLog))
        {
            return false;
        }

        File.AppendAllText(_pathToTraceLog, message.ToString() + Environment.NewLine);
        return true;
    }

    private static string ReadFromScriptLog()
    {
        if (!File.Exists(_pathToScriptLog))
        {
            return string.Empty;
        }

        return File.ReadAllText(_pathToScriptLog);
    }

    private static bool WriteToScriptLog(LogMessage message)
    {
        if (!File.Exists(_pathToScriptLog))
        {
            return false;
        }

        File.AppendAllText(_pathToScriptLog, message.ToString() + Environment.NewLine);
        return true;
    }

    private static string ReadFromExceptionLog()
    {
        if (!File.Exists(_pathToExceptionLog))
        {
            return string.Empty;
        }

        return File.ReadAllText(_pathToExceptionLog);
    }

    private static bool WriteToExceptionLog(LogMessage message)
    {
        if (!File.Exists(_pathToExceptionLog))
        {
            return false;
        }

        File.AppendAllText(_pathToExceptionLog, message.ToString() + Environment.NewLine);
        return true;
    }
    #endregion

    #region Methods: Console
    private static void WriteToConsole(LogMessage message)
    {
        Debug.WriteLine(message.ToString());
    }
    #endregion

}