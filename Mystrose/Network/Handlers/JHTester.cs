namespace Mystrose.Network.Handlers;

public class JHTester() : MessageHandler<JSONMessage>([])
{

    #region Fields
    private readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true
    };
    #endregion

    #region Methods: Invoker
    public override void Invoke(JSONMessage message)
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
    protected void Handle(JSONMessage message)
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
