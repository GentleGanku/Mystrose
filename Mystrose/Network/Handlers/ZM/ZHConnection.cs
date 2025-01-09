namespace Mystrose.Network.Handlers.ZM;

public class ZHConnection() : MessageHandler<ZMMessage>(new()
{
    ["sfcConnect"] = HandleConnection,
    ["sfcLoginInfo"] = HandleLoginInfo,
    ["sfcDisconnect"] = HandleDisconnection
})
{

    #region Methods: Handlers
    private static void HandleConnection(ZMMessage message)
    {
        bool isSuccess = bool.Parse(message.Arguments[5]);

        if (!isSuccess)
        {
            return;
        }

        string serverName = message.Arguments[4];
        
        MSVCWorld.Instance.Activate(message.Identifier.Codename, serverName);
    }

    private static void HandleLoginInfo(ZMMessage message)
    {
        string loginInfoString = message.RawContent.Split("[INFO]")[1];
        JsonObject loginInfo = JsonSerializer.Deserialize<JsonObject>(loginInfoString)!;

        message.HostWorld.Avatar = new()
        {
            MemberDays = loginInfo["iUpgDays"]!.GetValue<int>(),
            Level = loginInfo["iLevel"]!.GetValue<int>(),
            AccessType = (AccessType)loginInfo["iAccess"]!.GetValue<int>(),
            Username = loginInfo["unm"]!.GetValue<string>(),
            UserID = loginInfo["userid"]!.GetValue<int>()
        };
    }

    private static void HandleDisconnection(ZMMessage message)
    {
        bool isConnectionLost = bool.Parse(message.Arguments[4]);

        if (!isConnectionLost)
        {
            return;
        }

        MSVCWorld.Instance.Deactivate(message.Identifier.Codename);
    }
    #endregion

}
