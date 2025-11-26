using Mystrose.DataRecords.Game;
using Mystrose.DataRecords.ReadableModels;
using Timer = System.Timers.Timer;

namespace Mystrose.Services;

public class MSVCVisualizer() : ManagerService<GameRecord>(nameof(MSVCVisualizer))
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

            Items = new()
            {
                ["Avernus"] = new(true, []),
                ["Beatrix"] = new(true, []),
                ["Cassiopeia"] = new(true, []),
                ["Durandal"] = new(true, []),
                ["Eligos"] = new(true, []),
                ["Fenrir"] = new(true, []),
                ["Gwyndell"] = new(true, []),
                ["Harbinger"] = new(true, []),
            };

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
        if (!Items.TryGetValue(codename, out var record))
        {
            return new(false,
                $"World with the codename {codename} does not exist.",
                []);
        }

        RMAura[] models = [.. record!.GetObjects<Aura>().Select(aura => new RMAura(aura.Item2))];
        
        return new(true,
            $"Aura models from {codename}'s World has been retrieved.",
            models);
    }

    public Response<RMCombatMessage[]> GetCombatMessageModels(string codename)
    {
        if (!Items.TryGetValue(codename, out var record))
        {
            return new(false,
                $"World with the codename {codename} does not exist.",
                []);
        }

        RMCombatMessage[] models = [.. record!.GetObjects<CombatMessage>().Select(cbtMsg => new RMCombatMessage(cbtMsg.Item2))];

        return new(true,
            $"Combat Message models from {codename}'s World has been retrieved.",
            models);
    }

    public Response<RMEventMessage[]> GetEventMessageModels(string codename)
    {
        if (!Items.TryGetValue(codename, out var record))
        {
            return new(false,
                $"World with the codename {codename} does not exist.",
                []);
        }

        RMEventMessage[] models = [.. record!.GetObjects<EventMessage>().Select(evtMsg => new RMEventMessage(evtMsg.Item2))];

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

        // RMCell[] models = [.. world.Area.Format.Cells.Select(cell => new RMCell(cell, world))];

        if (!MSVCGame.Instance.Collection.TryGetValue(codename, out var game))
        {
            return new(false,
                $"Game with the codename {codename} does not exist.",
                []);
        }
        
        RMCell[] models = [.. game.FlashAPI.Map.GetCells().Select(cell => new RMCell(cell, world))];
        
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
    
    #region Methods: Write
    public Response<GameRecord?> RefreshGameRecord(string codename)
    {
        if (!Items.TryGetValue(codename, out var record))
        {
            return new(false,
                $"World with the codename {codename} does not exist.",
                null);
        }

        Items[codename] = new(true, []);

        return new(true,
            $"World with the codename {codename} has been refreshed.",
            Items[codename]);
    }
    
    public Response<bool> AddRecordObject(string codename, object recordObject)
    {
        if (!Items.TryGetValue(codename, out var record))
        {
            return new(false,
                $"World with the codename {codename} does not exist.",
                false);
        }

        switch (recordObject)
        {
            case Aura aura:
                record!.Add(aura);
                break;
            case CombatMessage combatMessage:
                record!.Add(combatMessage);
                break;
            case EventMessage eventMessage:
                record!.Add(eventMessage);
                break;
            
            default:
                return new(false,       
                    $"Record object of type {recordObject.GetType().Name} is not supported.",
                    false);
        }
        
        RecordEvent?.Invoke(codename, recordObject);
        
        return new(true,
            $"Record object of type {recordObject.GetType().Name} has been added to the world with codename {codename}.",
            true);
    }
    #endregion

    #region Methods: Service Handlers
    private void OnTimerElapse(object? sender, ElapsedEventArgs e)
    {
        foreach (var gameRecordPair in ActiveCollection)
        {
            gameRecordPair.Value!.Expire();
        }
    }
    #endregion

}
