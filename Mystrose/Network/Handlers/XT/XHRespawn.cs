namespace Mystrose.Network.Handlers.XT;

public class XHRespawn() : MessageHandler<XTMessage>(new()
{
    ["resTimed"] = HandleRespawn
})
{
    
    #region Methods: Handlers
    private static void HandleRespawn(XTMessage message)
    {
        // TODO: Implement respawn handling
    }
    #endregion

}
