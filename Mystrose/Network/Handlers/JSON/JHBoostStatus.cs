namespace Mystrose.Network.Handlers.JSON;

public static class JHBoostStatus
{

    #region Fields
    private static readonly Dictionary<string, Action<JSONMessage>> _handlers = new()
    {
        ["xpboost"] = HandleBoost,
        ["gboost"] = HandleBoost,
        ["repboost"] = HandleBoost,
        ["cpboost"] = HandleBoost
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
    public static void HandleBoost(JSONMessage message)
    {
        bool isActive = message.DataObject["op"].Deserialize<string>() == "+";

        message.World.Boosts.SetBoost(message.Command, isActive);

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, message.World.Boosts);
    }
    #endregion

}
