namespace Mystrose.Network.Handlers.WIP;

public class HFriendship : IJSONMessageHandler
{

    #region Commands
    public string[] HandledCommands
    {
        get =>
        [
            "friendshipStats",
            "friendshipInfo",
            "friendshipGift",
            "friendshipTalk",
            "friendshipChoice"
        ];
    }
    #endregion

    #region Methods: Handler
    public void Handle(GameHost host, JSONMessage message)
    {
        // TODO: Implement
    }
    #endregion

}
