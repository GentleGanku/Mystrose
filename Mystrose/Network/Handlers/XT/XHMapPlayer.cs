namespace Mystrose.Network.Handlers.XT;

public class XHMapPlayer() : MessageHandler<XTMessage>(new()
{
    ["exitArea"] = HandleExit
})
{

    #region Methods: Handlers
    private static void HandleExit(XTMessage message)
    {
        int userId = int.Parse(message.Arguments[4]);

        Avatar? avatar = message.HostWorld.Area.Players.Find(
            (avt) =>
            {
                return avt.EntityID == userId;
            });

        if (avatar is null)
        {
            return;
        }

        message.HostWorld.Area.Players.Remove(avatar);

        avatar.Cell = avatar.Pad = "None";

        MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, avatar);
    }
    #endregion

}
