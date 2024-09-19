namespace Mystrose.DataFormats.GameModels.Master;

public class World : GameObject
{

    #region Constructor
    public World(ClientUseIdentifier identifier, Server server)
    {
        _identifier = identifier;
        Server = server;
    }
    #endregion

    #region (Private) Fields
    private ClientUseIdentifier _identifier;
    private Server _server;
    private MainAvatar _avatar;
    private Dictionary<InventoryType, InventoryManager> _inventories;
    private BoostStatuses _boosts;
    private ActiveSkills _skills;
    private AuraDictionary _auras;
    private Party _party;
    private Area _area;
    private Shop _shop;
    private Shop _enhancementShop;
    private List<Faction> _factions;
    private List<Quest> _quests;
    private List<BaseItem> _drops;
    #endregion

    #region Fields
    public HSTGame Host => SVCGameManager.GetGameDict().Output[_identifier.Codename]!;
    #endregion

    #region Properties
    public Server Server
    {
        get => _server;
        set => _server = value;
    }

    public MainAvatar Avatar
    {
        get => _avatar;
        set => _avatar = value;
    }

    public Dictionary<InventoryType, InventoryManager> Inventories
    {
        get => _inventories;
        set => _inventories = value;
    }

    public BoostStatuses Boosts
    {
        get => _boosts;
        set => _boosts = value;
    }

    public ActiveSkills Skills
    {
        get => _skills;
        set => _skills = value;
    }

    public AuraDictionary Auras
    {
        get => _auras;
        set => _auras = value;
    }

    public Party Party
    {
        get => _party;
        set => _party = value;
    }

    public Area Area
    {
        get => _area;
        set => _area = value;
    }

    public Shop Shop
    {
        get => _shop;
        set => _shop = value;
    }

    public Shop EnhancementShop
    {
        get => _enhancementShop;
        set => _enhancementShop = value;
    }

    public List<Faction> Factions
    {
        get => _factions;
        set => _factions = value;
    }

    public List<Quest> Quests
    {
        get => _quests;
        set => _quests = value;
    }

    public List<BaseItem> Drops
    {
        get => _drops;
        set => _drops = value;
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

            SVCScriptManager.InvokeTrigger(_identifier.Codename, skill);
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

            SVCScriptManager.InvokeTrigger(_identifier.Codename, skill);
        }
    }
    #endregion

}
