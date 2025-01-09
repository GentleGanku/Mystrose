namespace Mystrose.Services.Instantiable.Subservices;

public class SSVCBank(ISVCFlashAPI service) : Subservice<ISVCFlashAPI>(service)
{

    #region Methods: Service
    public void TransferToBank(InventoryItem item)
    {
        Execute(() =>
        {
            string itemString = JSONParser.Serialize(item);

            Service.CallGameFunction("world.sendBankFromInvRequest", itemString);
        });
    }

    public void TransferToInventory(InventoryItem item)
    {
        Execute(() =>
        {
            string itemString = JSONParser.Serialize(item);

            Service.CallGameFunction("world.sendBankToInvRequest", itemString);
        });
    }

    public void TransferSwap(InventoryItem bankItem, InventoryItem invItem)
    {
        Execute(() =>
        {
            string bankItemString = JSONParser.Serialize(bankItem);
            string invItemString = JSONParser.Serialize(invItem);
            
            Service.CallGameFunction("world.sendBankSwapInvRequest", bankItemString, invItemString);
        });
    }
    #endregion

    #region Methods: Overrides
    protected override void Log(string message)
    {
        HSVCLogger.Instance.LogOnConsole(message, Service.Identifier.Codename, "SSVCBank");
    }
    #endregion

}
