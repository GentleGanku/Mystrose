namespace Mystrose.Network.Handlers.XT;

public static class XHConnectionResponse
{

    #region Fields
    private static readonly Dictionary<string, Action<XTMessage>> _handlers = new()
    {
        ["loginResponse"] = HandleLoginResponse
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
    public static void HandleLoginResponse(XTMessage message)
    {
        int entId = int.Parse(message.Arguments[5]);
        string name = message.Arguments[6];

        message.World.Avatar.EntityID = entId;
        message.World.Avatar.Name = name;
    }
    #endregion

}
