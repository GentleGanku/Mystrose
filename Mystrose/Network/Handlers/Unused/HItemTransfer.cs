namespace Mystrose.Network.Handlers.Unused;

public class HItemTransfer : IJSONMessageHandler
{

    #region Commands
    public string[] HandledCommands
    {
        get =>
        [
            "bankFromInv",
            "bankToInv",
            "bankSwapInv"
        ];
    }
    #endregion

    #region Methods: Handler
    public void Handle(GameHost host, JSONMessage message)
    {
        switch (message.Command)
        {
            case "bankFromInv":
                bool isSuccess = message.DataObject["bSuccess"].GetValue<int>() == 1;

                if (!isSuccess)
                {
                    return;
                }

                InventoryItem? inventoryItem = host.World.Inventory[message.DataObject["ItemID"].GetValue<int>()];

                if (inventoryItem == null)
                {
                    return;
                }

                host.World.BankInventory.Add(inventoryItem.ID, inventoryItem);
                if (!inventoryItem.IsCoinTagged)
                {
                    host.World.Master.UsedBankSlots++;
                }

                host.World.Inventory.Remove(inventoryItem.ID);
                break;
            case "bankToInv":
                InventoryItem? bankItem = host.World.BankInventory[message.DataObject["ItemID"].GetValue<int>()];

                if (bankItem == null)
                {
                    return;
                }

                host.World.Inventory.Add(bankItem.ID, bankItem);

                host.World.BankInventory.Remove(bankItem.ID);
                if (!bankItem.IsCoinTagged)
                {
                    host.World.Master.UsedBankSlots--;
                }
                break;
            case "bankSwapInv":
                InventoryItem? bankSwapItem = host.World.BankInventory[message.DataObject["bankItemID"].GetValue<int>()];

                if (bankSwapItem == null)
                {
                    return;
                }

                InventoryItem? inventorySwapItem = host.World.Inventory[message.DataObject["invItemID"].GetValue<int>()];

                if (inventorySwapItem == null)
                {
                    return;
                }

                host.World.BankInventory.Add(inventorySwapItem.ID, inventorySwapItem);
                host.World.BankInventory.Remove(bankSwapItem.ID);

                host.World.Inventory.Add(bankSwapItem.ID, bankSwapItem);
                host.World.Inventory.Remove(inventorySwapItem.ID);
                break;
        }
    }
    #endregion

}
