namespace Mystrose.Network.Handlers;

public class XHTester() : MessageHandler<XTMessage>([])
{

    #region Methods: Invoker
    public override void Invoke(XTMessage message)
    {
        try
        {
            Handle(message);
        }
        catch (Exception ex)
        {
            HSVCLogger.Instance.LogOnException($"({nameof(message)} - {message.Command}) {ex.ToString()}");
        }
    }
    #endregion

    #region Methods: Handlers
    protected void Handle(XTMessage message)
    {
        string xtPath = "packetFormats\\xt\\" + message.Command + ".txt";
        string[] xtContentArray = message.RawContent.Split("%");
        string xtContent = string.Join("\r\n%\r\n", xtContentArray);

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
