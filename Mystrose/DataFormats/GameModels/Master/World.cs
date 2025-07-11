namespace Mystrose.DataFormats.GameModels.Master;

public class World(ClientInstanceIdentifier? identifier = null) : GameObject
{

    #region (Private) Fields
    private readonly ClientInstanceIdentifier _identifier = identifier ?? new();
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
    } = new();

    public MainAvatar Avatar
    {
        get;
        init;
    } = new();

    public Dictionary<InventoryType, InventoryManager> Inventories
    {
        get;
        init;
    } = new()
    {
        [InventoryType.Base] = new(99),
        [InventoryType.Temp] = new(99),
        [InventoryType.House] = new(99),
        [InventoryType.Bank] = new(99)
    };

    public BoostStatuses Boosts
    {
        get;
        set;
    } = new();

    public ActiveSkills Skills
    {
        get;
        set;
    } = new([]);

    public AuraDictionary Auras
    {
        get;
        init;
    } = new();

    public Party Party
    {
        get;
        set;
    } = new();

    public Area Area
    {
        get;
        set;
    } = new();

    public Shop Shop
    {
        get;
        set;
    } = new();

    public Shop EnhancementShop
    {
        get;
        set;
    } = new();

    public List<Faction> Factions
    {
        get;
        init;
    } = [];

    public List<Quest> Quests
    {
        get;
        init;
    } = [];

    public List<BaseItem> Drops
    {
        get;
        init;
    } = [];
    #endregion

    #region Methods: Actions
    public void RefreshServer(Server server)
    {
        Server = server;
    }
    
    public void RefreshAvatar(MainAvatar avatar)
    {
        JsonObject avatarJson = JsonSerializer.Deserialize<JsonObject>(JsonSerializer.Serialize(avatar))!;

        Avatar.SetProperties(avatarJson);

        Inventories[InventoryType.Base].TotalSlots = Avatar.InventorySlots;
        Inventories[InventoryType.House].TotalSlots = Avatar.HouseSlots;
        Inventories[InventoryType.Bank].TotalSlots = Avatar.BankSlots;
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
