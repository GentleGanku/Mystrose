namespace Mystrose.Network.Handlers;

public class ZHTester() : MessageHandler<ZMMessage>([])
{

    #region Methods: Invoker
    public override void Invoke(ZMMessage message)
    {
        try
        {
            Handle(message);
        }
        catch (Exception ex)
        {
            HSVCLogger.Instance.LogOnException($"({nameof(message)} - {message.Command}) {ex}");
        }
    }
    #endregion

    #region Methods: Handlers
    private void Handle(ZMMessage message)
    {
        string zmPath = "packetFormats\\zm\\" + message.Command + ".txt";
        string[] zmContentArray = message.RawContent.Split("%");
        string zmContent = string.Join("\r\n%\r\n", zmContentArray);

        if (File.Exists(zmPath))
        {
            string[] existingContent = File.ReadAllLines(zmPath);

            if (zmContentArray.Length > existingContent.Length)
            {
                File.WriteAllText(zmPath, zmContent);
            }
        }
        else
        {
            File.WriteAllText(zmPath, zmContent);
        }
    }
    #endregion

}
