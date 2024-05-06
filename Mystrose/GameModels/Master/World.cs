using Mystrose.Controls.Main;
using Mystrose.GameModels.Base;
using Mystrose.GameModels.Environment;
using Mystrose.GameModels.Network;
using Mystrose.Utilities.Enumerations;
using System.Collections.Generic;

namespace Mystrose.GameModels.Master;

public class World
{

    #region Constructor
    public World(GameHost host, Server server)
    {
        ParentHost = host;

        Server = server;
        Quests = [];
        Drops = [];
        Auras = [];

        Party = null;
        Area = null;
        Shop = null;
        EnhancementShop = null;
    }
    #endregion

    #region Private Fields
    private MainAvatar _master;
    #endregion

    #region Fields: Inventories
    public InventoryManager Inventory
    {
        get => MasterInventory[InventoryType.Inventory];
    }

    public InventoryManager TemporaryInventory
    {
        get => MasterInventory[InventoryType.TemporaryInventory];
    }

    public InventoryManager HouseInventory
    {
        get => MasterInventory[InventoryType.HouseInventory];
    }

    public InventoryManager BankInventory
    {
        get => MasterInventory[InventoryType.BankInventory];
    }
    #endregion

    #region Fields: Currency
    public int Gold
    {
        get => Master.Gold;
    }

    public int AdventureCoins
    {
        get => Master.AdventureCoins;
    }
    #endregion

    #region Fields: Boosters
    public bool IsRepBoosted
    {
        get => Master.RepBoost;
    }

    public bool IsGoldBoosted
    {
        get => Master.GoldBoost;
    }

    public bool IsXPBoosted
    {
        get => Master.XPBoost;
    }

    public bool IsCPBoosted
    {
        get => Master.CPBoost;
    }
    #endregion

    #region Properties: Player
    public GameHost ParentHost
    {
        get;
        set;
    }

    public MainAvatar Master
    {
        get => _master;
        set
        {
            _master = value;
            Skills = new(ParentHost, value.ActiveSkills);
            MasterInventory = new()
            {
                [InventoryType.Inventory] = new(value.InventorySlots),
                [InventoryType.TemporaryInventory] = new(99),
                [InventoryType.HouseInventory] = new(value.HouseSlots),
                [InventoryType.BankInventory] = new(value.BankSlots)
            };
        }
    }

    public SkillManager Skills
    {
        get;
        set;
    }

    public Dictionary<InventoryType, InventoryManager> MasterInventory
    {
        get;
        set;
    }
    #endregion

    #region Properties: Environment
    public Server Server
    {
        get;
        set;
    }

    public Party? Party
    {
        get;
        set;
    }

    public Area? Area
    {
        get;
        set;
    }

    public Shop? Shop
    {
        get;
        set;
    }

    public Shop? EnhancementShop
    {
        get;
        set;
    }

    public List<Quest> Quests
    {
        get;
        set;
    }

    public List<BaseItem> Drops
    {
        get;
        set;
    }

    public AuraDictionary Auras
    {
        get;
        set;
    }
    #endregion

}
