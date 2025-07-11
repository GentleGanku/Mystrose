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

        message.HostWorld.Avatar.Username = loginInfo["unm"]!.GetValue<string>();
        message.HostWorld.Avatar.Name = message.HostWorld.Avatar.Username.ToLower();
        message.HostWorld.Avatar.UserID = loginInfo["userid"]!.GetValue<int>();
        message.HostWorld.Avatar.AccessType = (AccessType)loginInfo["iAccess"]!.GetValue<int>();
        message.HostWorld.Avatar.MemberDays = loginInfo["iUpgDays"]!.GetValue<int>();
        message.HostWorld.Avatar.Level = loginInfo["iLevel"]!.GetValue<int>();
    }

    private static void HandleDisconnection(ZMMessage message)
    {
        bool isConnectionLost = bool.Parse(message.Arguments[4]);

        if (!isConnectionLost)
        {
            return;
        }

        MSVCWorld.Instance.Activate(message.Identifier.Codename);
    }
    #endregion

}
