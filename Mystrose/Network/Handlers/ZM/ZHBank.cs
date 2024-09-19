namespace Mystrose.Network.Handlers.ZM;

public static class ZHBank
{

    #region Fields
    private static readonly Dictionary<string, Action<ZMMessage>> _handlers = new()
    {
        ["bankLoad"] = HandleBankLoad
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
    public static void HandleBankLoad(ZMMessage message)
    {
        List<InventoryItem> bankData = JsonSerializer.Deserialize<List<InventoryItem>>(message.RawContent.Split("[BANK]")[1])!;

        message.World.Inventories[InventoryType.Bank].Clear();
        message.World.Inventories[InventoryType.Bank].AddRange(bankData);
    }
    #endregion

}
