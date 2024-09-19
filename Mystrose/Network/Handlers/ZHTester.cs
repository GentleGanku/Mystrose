namespace Mystrose.Network.Handlers;

public static class ZHTester
{

    #region Methods: Invoker
    public static void Invoke(ZMMessage message)
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
    public static void Handle(ZMMessage message)
    {
        string zmPath = "packetFormats\\zm\\" + message.Command + ".txt";
        string[] zmContentArray = message.RawContent.Split("%");
        string zmContent = string.Join("%\r\n", zmContentArray);

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
