namespace Mystrose.Network.Handlers.ZM;

public static class ZHConnection
{

    #region Fields
    private static readonly Dictionary<string, Action<ZMMessage>> _handlers = new()
    {
        ["sfcConnect"] = HandleConnection,
        ["sfcLoginInfo"] = HandleLoginInfo,
        ["sfcDisconnect"] = HandleDisconnection
    };
    #endregion

    #region Methods: Invoker
    public static void Invoke(ZMMessage message)
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
    public static void HandleConnection(ZMMessage message)
    {
        bool isSuccess = bool.Parse(message.Arguments[5]);

        if (!isSuccess)
        {
            return;
        }

        string serverName = message.Arguments[4];
        
        SVCWorldVisualizer.Activate(message.Identifier.Codename, serverName);
    }

    public static void HandleLoginInfo(ZMMessage message)
    {
        string loginInfoString = message.RawContent.Split("[INFO]")[1];
        JsonObject loginInfo = JsonSerializer.Deserialize<JsonObject>(loginInfoString)!;

        message.World.Avatar = new()
        {
            MemberDays = loginInfo["iUpgDays"]!.GetValue<int>(),
            Level = loginInfo["iLevel"]!.GetValue<int>(),
            AccessType = (AccessType)loginInfo["iAccess"]!.GetValue<int>(),
            Username = loginInfo["unm"]!.GetValue<string>(),
            UserID = loginInfo["userid"]!.GetValue<int>()
        };
    }

    public static void HandleDisconnection(ZMMessage message)
    {
        bool isConnectionLost = bool.Parse(message.Arguments[4]);

        if (!isConnectionLost)
        {
            return;
        }

        SVCWorldVisualizer.Deactivate(message.Identifier.Codename);
    }
    #endregion

}
