using Mystrose.GameModels.General;
using Mystrose.Utilities.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mystrose.GameModels.Base;

public class InventoryManager : Dictionary<int, InventoryItem>
{

    #region Constructor
    public InventoryManager(int totalSlots)
    {
        TotalSlots = totalSlots;
    }
    #endregion

    #region Fields: Items
    public InventoryItem? this[int id]
    {
        get => base[id];
    }

    public InventoryItem? this[string name]
    {
        get => Values.FirstOrDefault(i => i.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public InventoryItem[] this[EquipmentType type]
    {
        get => Values.Where(i => i.EquipmentType == type).ToArray();
    }

    public InventoryItem[] this[ItemType type]
    {
        get => Values.Where(i => i.Type == type).ToArray();
    }
    #endregion

    #region Fields: Slots
    public int UsedSlots
    {
        get => Count;
    }

    public int FreeSlots
    {
        get => TotalSlots - UsedSlots;
    }
    #endregion

    #region Properties
    public int TotalSlots
    {
        get;
        set;
    }
    #endregion

    #region Methods
    public void AddRange(IEnumerable<InventoryItem> items)
    {
        foreach (InventoryItem item in items)
        {
            base[item.ID] = item;
        }
    }
    #endregion

}
