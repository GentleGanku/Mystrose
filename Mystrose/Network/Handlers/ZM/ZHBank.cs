namespace Mystrose.Network.Handlers.ZM;

public class ZHBank() : MessageHandler<ZMMessage>(new()
{
    ["bankLoad"] = HandleBankLoad
})
{

    #region Methods: Handlers
    private static void HandleBankLoad(ZMMessage message)
    {
        List<InventoryItem> bankData = JsonSerializer.Deserialize<List<InventoryItem>>(message.RawContent.Split("[BANK]")[1])!;

        message.HostWorld.Inventories[InventoryType.Bank].Clear();
        message.HostWorld.Inventories[InventoryType.Bank].AddRange(bankData);
    }
    #endregion

}
