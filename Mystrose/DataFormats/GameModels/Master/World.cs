namespace Mystrose.DataFormats.GameModels.Master;

public class World : GameObject
{

    #region Constructor
    public World()
    {
        _identifier = new();
        Server = new();
    }
    
    public World(ClientInstanceIdentifier identifier, Server server)
    {
        _identifier = identifier;
        Server = server;
    }
    #endregion

    #region (Private) Fields
    private ClientInstanceIdentifier _identifier;
    #endregion

    #region Fields
    public MSVCScript ScriptService => MSVCScript.Instance;
    public HSTGame Host => MSVCGame.Instance[_identifier.Codename].Item2!;
    #endregion

    #region Properties
    public Server Server
    {
        get;
        set;
    }

    public MainAvatar Avatar
    {
        get;
        set;
    }

    public Dictionary<InventoryType, InventoryManager> Inventories
    {
        get;
        set;
    }

    public BoostStatuses Boosts
    {
        get;
        set;
    }

    public ActiveSkills Skills
    {
        get;
        set;
    }

    public AuraDictionary Auras
    {
        get;
        set;
    }

    public Party Party
    {
        get;
        set;
    }

    public Area Area
    {
        get;
        set;
    }

    public Shop Shop
    {
        get;
        set;
    }

    public Shop EnhancementShop
    {
        get;
        set;
    }

    public List<Faction> Factions
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
    #endregion

    #region Methods: Setup
    public void Destruct()
    {
        Inventories.Clear();
        Skills.Clear();
        Auras.Clear();
        Factions.Clear();
        Quests.Clear();
        Drops.Clear();
    }
    #endregion

    #region Methods: Actions
    public void RefreshAvatar(MainAvatar avatar)
    {
        int memberDays = Avatar.MemberDays;
        AccessType accessType = Avatar.AccessType;
        string username = Avatar.Username;
        int userId = Avatar.UserID;
        int level = Avatar.Level;

        Avatar = avatar;
        Avatar.MemberDays = memberDays;
        Avatar.AccessType = accessType;
        Avatar.Username = username;
        Avatar.UserID = userId;
        Avatar.Level = level;

        Inventories = new()
        {
            [InventoryType.Base] = new(Avatar.InventorySlots),
            [InventoryType.Temp] = new(99),
            [InventoryType.House] = new(Avatar.HouseSlots),
            [InventoryType.Bank] = new(Avatar.BankSlots)
        };

        Auras = new();
    }

    public void LockSkill(Aura aura)
    {
        if (!aura.TargetID.Equals(Avatar.Name, StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        ActiveSkill? prelockedSkill = Skills.Find(
            (s) =>
            {
                return s.IsLocked;
            });

        if (prelockedSkill is not null)
        {
            return;
        }

        ActiveSkill? skill = Skills[aura.Value];

        if (skill is not null && !skill.IsLocked)
        {
            skill.IsLocked = true;

            ScriptService.InvokeTrigger(_identifier.Codename, skill);
        }
    }

    public void UnlockSkill(Aura aura)
    {
        if (!aura.TargetID.Equals(Avatar.Name, StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        ActiveSkill? skill = Skills[aura.Value];

        if (skill is not null && skill.IsLocked)
        {
            skill.IsLocked = false;

            ScriptService.InvokeTrigger(_identifier.Codename, skill);
        }
    }
    #endregion

}
