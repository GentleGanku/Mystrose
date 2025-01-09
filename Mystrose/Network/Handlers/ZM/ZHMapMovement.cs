namespace Mystrose.Network.Handlers.ZM;

public class ZHMapMovement() : MessageHandler<ZMMessage>(new()
{
    ["mv"] = HandlePositionChange,
    ["moveToCell"] = HandleFrameChange,
})
{

    #region Methods: Handlers
    private static void HandlePositionChange(ZMMessage message)
    {
        double x = double.Parse(message.Arguments[5]);
        double y = double.Parse(message.Arguments[6]);

        message.HostWorld.Avatar.X = x;
        message.HostWorld.Avatar.Y = y;

        MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, message.HostWorld.Avatar);
    }

    private static void HandleFrameChange(ZMMessage message)
    {
        string cell = message.Arguments[5];
        string pad = message.Arguments[6];

        message.HostWorld.Avatar.Cell = cell;
        message.HostWorld.Avatar.Pad = pad;

        MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, message.HostWorld.Avatar);
    }
    #endregion

}
