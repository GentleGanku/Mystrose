namespace Mystrose.Network.Handlers.JSON;

public static class JHEventMessage
{

    #region Fields
    private static readonly Dictionary<string, Action<JSONMessage>> _handlers = new()
    {
        ["event"] = HandleEventMessage
    };
    #endregion

    #region Methods: Invoker
    public static void Invoke(JSONMessage message)
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
    public static void HandleEventMessage(JSONMessage message)
    {
        foreach (KeyValuePair<string, JsonNode> eventObj in message.DataObject)
        {
            EventMessage evtMsg = new(eventObj.Key, eventObj.Value);

            SVCScriptManager.InvokeTrigger(message.Identifier.Codename, evtMsg);
        }
    }
    #endregion

}
