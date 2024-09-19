namespace Mystrose.Network.Handlers;

public class TestXT : IXTMessageHandler
{

    public string[] HandledCommands
    {
        get;
        set;
    } = new string[]
    {
        "dungeonCompleted",
        "dungeonMTC",
        "hi",
        "loginResponse",
        "loginIterator",
        "iterator",
        "loginMulti",
        "notify",
        "logoutWarning",
        "multiLoginWarning",
        "server",
        "serverf",
        "popup",
        "moderator",
        "wheel",
        "gsupdate",
        "frostupdate",
        "warning",
        "respawnMon",
        "resTimed",
        "exitArea",
        "uotls",
        "mtls",
        "spcs",
        "cc",
        "emotea",
        "em",
        "chatm",
        "whisper",
        "mute",
        "unmute",
        "mvna",
        "mvnb",
        "gtc",
        "mtcid",
        "hi",
        "Dragon Buff",
        "trap door",
        "gMOTD",
        "buyGSlots",
        "gRename",
        "fbRes",
        "elmSwitch"
    };

    public void Handle(XTMessage message)
    {
        string path = AppDomain.CurrentDomain.BaseDirectory + "packets\\xt\\" + message.Command + ".txt";

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
