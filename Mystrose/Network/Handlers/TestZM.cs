namespace Mystrose.Network.Handlers;

public class TestZM : IZMMessageHandler
{

    public string[] HandledCommands
    {
        get;
        set;
    } = new string[]
    {
    };

    public void Handle(ZMMessage message)
    {
        string path = AppDomain.CurrentDomain.BaseDirectory + "packets\\zm\\" + message.Command + ".txt";

        if (File.Exists(path))
        {
            if (File.ReadAllText(path).Length < message.RawContent.Length)
            {
                File.WriteAllText(path, message.RawContent);
            }
        }
        else
        {
            File.WriteAllText(path, message.RawContent);
        }
    }

}
