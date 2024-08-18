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
    public void Handle(GameHost host, XTMessage message)
    {
        switch (message.Command)
        {
            case "respawnMon":
                HandleMonster(host, message.Arguments);
                break;
            case "resTimed":
                HandleMaster(host, message.Arguments);
                break;
        }
    }
    #endregion

    #region Methods: Monster
    private void HandleMonster(GameHost host, string[] args) 
    {
        string[] ids = args[4].Split(',');

        foreach (string id in ids)
        {
            // WIP
        }
    }
    #endregion

    #region Methods: Master
    private void HandleMaster(GameHost host, string[] args)
    {
        // WIP
    }
    #endregion

}
