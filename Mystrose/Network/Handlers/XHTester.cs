namespace Mystrose.Network.Handlers;

public static class XHTester
{

    #region Methods: Invoker
    public static void Invoke(XTMessage message)
    {
        try
        {
            Handle(message);
        }
        catch (Exception ex)
        {
            SVCLogger.LogOnException($"({nameof(message)} - {message.Command}) {ex.ToString()}");
        }
    }
    #endregion

    #region Handlers
    public static void Handle(XTMessage message)
    {
        string xtPath = "packetFormats\\xt\\" + message.Command + ".txt";
        string[] xtContentArray = message.RawContent.Split("%");
        string xtContent = string.Join("%\r\n", xtContentArray);

        if (File.Exists(xtPath))
        {
            string[] existingContent = File.ReadAllLines(xtPath);

            if (xtContentArray.Length > existingContent.Length)
            {
                File.WriteAllText(xtPath, xtContent);
            }
        }
        else
        {
            File.WriteAllText(xtPath, xtContent);
        }
    }
    #endregion

}
