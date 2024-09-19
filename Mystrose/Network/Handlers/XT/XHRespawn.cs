namespace Mystrose.Network.Handlers.XT;

public static class XHRespawn
{

    #region Fields
    private static readonly Dictionary<string, Action<XTMessage>> _handlers = new()
    {
        ["resTimed"] = HandleRespawn
    };
    #endregion

    #region Methods: Invoker
    public static void Invoke(XTMessage message)
    {
        if (!_handlers.TryGetValue(message.Command, out var handler))
        {
            return;
        }

        try
        {
            handler.Invoke(message);
        }
        catch (Exception ex)
        {
            SVCLogger.LogOnException($"({nameof(message)} - {message.Command}) {ex.ToString()}");
        }
    }
    #endregion

    #region Handlers
    public static void HandleRespawn(XTMessage message)
    {
        // WIP
    }
    #endregion

}
