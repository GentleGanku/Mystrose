namespace Mystrose.Network.Handlers.XT;

public class XHDungeonInterface() : MessageHandler<XTMessage>(new()
{
    ["dungeonMTC"] = HandleMoveToCell,
    ["dungeonCompleted"] = HandleDungeonCompletion
})
{

    #region Methods: Handlers
    private static void HandleMoveToCell(XTMessage message)
    {
        bool isSuccess = bool.Parse(message.Arguments[4]);

        if (!isSuccess)
        {
            return;
        }

        string[] destination = message.Arguments[5].Split(',');

        message.HostWorld.Avatar.Cell = destination[0];
        message.HostWorld.Avatar.Pad = destination[1];

        MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, message.HostWorld.Avatar);
    }

    private static void HandleDungeonCompletion(XTMessage message)
    {
        // TODO: Handle dungeon completion
    }
    #endregion

}
