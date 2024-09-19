namespace Mystrose.Network.Handlers;

public static class JHTester
{

    #region Fields
    private static readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true
    };
    #endregion

    #region Methods: Invoker
    public static void Invoke(JSONMessage message)
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
    public static void Handle(JSONMessage message)
    {
        string jsonPath = "packetFormats\\json\\" + message.Command + ".json";
        string jsonContent = JsonSerializer.Serialize(message.Object, _options);

        if (File.Exists(jsonPath))
        {
            string[] newContent = jsonContent.Split("\r\n");
            string[] existingContent = File.ReadAllLines(jsonPath);
            
            if (newContent.Length > existingContent.Length)
            {
                File.WriteAllText(jsonPath, jsonContent);
            }
        }
        else
        {
            File.WriteAllText(jsonPath, jsonContent);
        }
    }
    #endregion

}
