using Timer = System.Timers.Timer;

namespace Mystrose.Services;

public class MSVCVisualizer() : ManagerService<List<GameRecord<GameObject>>>(nameof(MSVCVisualizer))
{

    #region Delegates & Handlers
    public delegate void RecordHandler(string codename, object recordObject);
    public event RecordHandler RecordEvent;
    #endregion

    #region (Static) Fields
    public static MSVCVisualizer Instance
    {
        get
        {
            if (_instance is null)
            {
                _instance = new MSVCVisualizer();
                _instance.Construct();
            }
            
            return _instance;
        }
    }
    #endregion

    #region (Private) Fields
    private static MSVCVisualizer? _instance;
    #endregion

    #region Fields
    private Timer _timer;
    #endregion

    #region Methods: Builder
    public override void Construct()
    {
        try
        {
            _timer = new(180000)
            {
                Enabled = true
            };

            _timer.Elapsed += OnTimerElapse;

            RecordEvent += AddRecordObject;

            foreach (var pair in Items)
            {
                Items[pair.Key] = new()
                {
                    [0] = new (true, []), // Aura
                    [1] = new(true, []), // Combat Message
                    [2] = new(true, []) // Event Message
                };
            }

            Log("World Visualizer constructed successfully.", "Construct");
        }
        catch (Exception ex)
        {
            Log(ex.ToString(), "Construct");
        }
    }

    public override void Deconstruct()
    {
        try
        {
            _timer.Elapsed -= OnTimerElapse;
            _timer.Dispose();

            RecordEvent -= AddRecordObject;

            Items.Clear();

            Log("World Visualizer deconstructed successfully.", "Deconstruct");
        }
        catch (Exception ex)
        {
            Log(ex.ToString(), "Deconstruct");
        }
    }
    #endregion

    #region Methods: Read
    public Response<RMAura[]> GetAuraModels(string codename)
    {
        if (!Items.TryGetValue(codename, out var records))
        {
            return new(false,
                $"World with the codename {codename} does not exist.",
                []);
        }

        RMAura[] models = [.. records![0].Objects.Select(aura => new RMAura((Aura)aura))];

        return new(true,
            $"Aura models from {codename}'s World has been retrieved.",
            models);
    }

    public Response<RMCombatMessage[]> GetCombatMessageModels(string codename)
    {
        if (!Items.TryGetValue(codename, out var records))
        {
            return new(false,
                $"World with the codename {codename} does not exist.",
                []);
        }

        RMCombatMessage[] models = [.. records![1].Objects.Select(cbtMsg => new RMCombatMessage((CombatMessage)cbtMsg))];

        return new(true,
            $"Combat Message models from {codename}'s World has been retrieved.",
            models);
    }

    public Response<RMEventMessage[]> GetEventMessageModels(string codename)
    {
        if (!Items.TryGetValue(codename, out var records))
        {
            return new(false,
                $"World with the codename {codename} does not exist.",
                []);
        }

        RMEventMessage[] models = [.. records![2].Objects.Select(evtMsg => new RMEventMessage((EventMessage)evtMsg))];

        return new(true,
            $"Event Message models from {codename}'s World has been retrieved.",
            models);
    }

    public Response<RMActiveSkill[]> GetActiveSkillModels(string codename)
    {
        if (!MSVCWorld.Instance.Collection.TryGetValue(codename, out var world))
        {
            return new(false,
                $"World with the codename {codename} does not exist.",
                []);
        }

        RMActiveSkill[] models = [.. world.Skills.Select(activeSkill => new RMActiveSkill(activeSkill, world))];

        return new(true,
            $"Active Skill models from {codename}'s World has been retrieved.",
            models);    
    }

    public Response<RMAvatar[]> GetAvatarModels(string codename)
    {
        if (!MSVCWorld.Instance.Collection.TryGetValue(codename, out var world))
        {
            return new(false,
                $"World with the codename {codename} does not exist.",
                []);
        }

        RMAvatar[] models = [.. world.Area.Players.Select(avatar => new RMAvatar(avatar, world))];

        return new(true,
            $"Avatar models from {codename}'s World has been retrieved.",
            models);
    }

    public Response<RMMonster[]> GetMonsterModels(string codename)
    {
        if (!MSVCWorld.Instance.Collection.TryGetValue(codename, out var world))
        {
            return new(false,
                $"World with the codename {codename} does not exist.",
                []);
        }

        RMMonster[] models = [.. world.Area.Monsters.Select(monster => new RMMonster(monster, world))];

        return new(true,
            $"Monster models from {codename}'s World has been retrieved.",
            models);
    }

    public Response<RMCell[]> GetCellModels(string codename)
    {
        if (!MSVCWorld.Instance.Collection.TryGetValue(codename, out var world))
        {
            return new(false,
                $"World with the codename {codename} does not exist.",
                []);
        }

        RMCell[] models = [.. world.Area.Format.Cells.Select(cell => new RMCell(cell, world))];

        return new(true,
            $"Cell models from {codename}'s World has been retrieved.",
            models);
    }

    public Response<RMQuest[]> GetQuestModels(string codename)
    {
        if (!MSVCWorld.Instance.Collection.TryGetValue(codename, out var world))
        {
            return new(false,
                $"World with the codename {codename} does not exist.",
                []);
        }

        RMQuest[] models = [.. world.Quests.Select(quest => new RMQuest(quest, world))];

        return new(true,
            $"Quest models from {codename}'s World has been retrieved.",
            models);
    }

    public Response<RMFaction[]> GetFactionModels(string codename)
    {
        if (!MSVCWorld.Instance.Collection.TryGetValue(codename, out var world))
        {
            return new(false,
                $"World with the codename {codename} does not exist.",
                []);
        }

        RMFaction[] models = [.. world.Factions.Select(faction => new RMFaction(faction, world))];

        return new(true,
            $"Faction models from {codename}'s World has been retrieved.",
            models);
    }

    public Response<RMShopItem[]> GetShopItemModels(string codename)
    {
        if (!MSVCWorld.Instance.Collection.TryGetValue(codename, out var world))
        {
            return new(false,
                $"World with the codename {codename} does not exist.",
                []);
        }

        RMShopItem[] models = [.. world.Shop.Items.Select(shopItem => new RMShopItem(shopItem, world))];

        return new(true,
            $"Shop Item models from {codename}'s World has been retrieved.",
            models);
    }

    public Response<Dictionary<InventoryType, RMInventoryItem[]>> GetInventoryItemModels(string codename)
    {
        if (!MSVCWorld.Instance.Collection.TryGetValue(codename, out var world))
        {
            return new(false,
                $"World with the codename {codename} does not exist.",
                []);
        }

        Dictionary<InventoryType, RMInventoryItem[]> models = new()
        {
            [InventoryType.Base] = [.. world.Inventories[InventoryType.Base].Values.Select(inventoryItem => new RMInventoryItem(inventoryItem, world))],
            [InventoryType.Temp] = [.. world.Inventories[InventoryType.Temp].Values.Select(inventoryItem => new RMInventoryItem(inventoryItem, world))],
            [InventoryType.House] = [.. world.Inventories[InventoryType.House].Values.Select(inventoryItem => new RMInventoryItem(inventoryItem, world))],
            [InventoryType.Bank] = [.. world.Inventories[InventoryType.Bank].Values.Select(inventoryItem => new RMInventoryItem(inventoryItem, world))]
        };

        return new(true,
            $"Inventory Item models from {codename}'s World has been retrieved.",
            models);
    }

    public Response<RMItemDrop[]> GetItemDropModels(string codename)
    {
        if (!MSVCWorld.Instance.Collection.TryGetValue(codename, out var world))
        {
            return new(false,
                $"World with the codename {codename} does not exist.",
                []);
        }

        RMItemDrop[] models = [.. world.Drops.Select(item => new RMItemDrop(item, world))];

        return new(true,
            $"Shop Item models from {codename}'s World has been retrieved.",
            models);
    }
    #endregion

    #region Methods: Service Handlers
    private void AddRecordObject(string codename, object recordObject)
    {
        if (!Items.TryGetValue(codename, out List<GameRecord<GameObject>>? value))
        {
            return;
        }

        switch (recordObject)
        {
            case Aura aura:
                value![0].Add(aura);
                break;
            case CombatMessage combatMessage:
                value![1].Add(combatMessage);
                break;
            case EventMessage eventMessage:
                value![2].Add(eventMessage);
                break;

            default:
                return;
        }
    }

    private void OnTimerElapse(object? sender, ElapsedEventArgs e)
    {
        foreach (var pair in Items)
        {
            pair.Value![0].Expire();
            pair.Value![1].Expire();
            pair.Value![2].Expire();
        }
    }
    #endregion

}
