namespace Mystrose.Network.Handlers.XT;

public class XHRespawn : IXTMessageHandler
{

    #region Commands
    public string[] HandledCommands
    {
        get =>
        [
            "respawnMon",
            "resTimed",
        ];
    }
    #endregion

    #region Methods: Handler
    public void Handle(XTMessage message)
    {
        switch (message.Command)
        {
            case "respawnMon":
                HandleMonster(message);
                break;
            case "resTimed":
                HandleAvatar(message);
                break;
        }
    }
    #endregion

    #region Methods: Monster
    private void HandleMonster(XTMessage message)
    {
        string[] args = message.Arguments;

        string[] ids = args[4].Split(',');

        foreach (string id in ids)
        {
            // WIP
        }
    }
    #endregion

    #region Methods: Master
    private void HandleAvatar(XTMessage message)
    {
        // WIP
    }
    #endregion

}
