namespace Mystrose.Network.Handlers.XT;

public class XHConnectionResponse() : MessageHandler<XTMessage>(new()
{
    ["loginResponse"] = HandleLoginResponse
})
{

    #region Methods: Handlers
    private static void HandleLoginResponse(XTMessage message)
    {
        int entId = int.Parse(message.Arguments[5]);
        string name = message.Arguments[6];

        message.HostWorld.Avatar.EntityID = entId;
        message.HostWorld.Avatar.Name = name;
    }
    #endregion

}
