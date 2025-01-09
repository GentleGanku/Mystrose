namespace Mystrose.Services;

public class MSVCScript() : ManagerService<ScriptEngine>(nameof(MSVCScript))
{

    #region Delegates & Handlers
    public delegate void EngineHandler(string codename, ScriptEngine? engine);
    public event EngineHandler ActivatedEngineEvent;
    public event EngineHandler DeactivatedEngineEvent;
    #endregion

    #region (Static) Fields
    public static MSVCScript Instance
    {
        get
        {
            if (_instance is null)
            {
                _instance = new MSVCScript();
                _instance.Construct();
            }
            
            return _instance;
        }
    }
    #endregion

    #region (Private) Fields
    private static MSVCScript? _instance;
    #endregion

    #region Properties
    public Dictionary<string, ScriptEngine?> CombatInstances
    {
        get;
        set;
    } = new()
    {
        ["Avernus"] = null,
        ["Beatrix"] = null,
        ["Cassiopeia"] = null,
        ["Durandal"] = null,
        ["Eligos"] = null,
        ["Fenrir"] = null,
        ["Gwyndell"] = null,
        ["Harbinger"] = null,
    };

    public Dictionary<string, ScriptEngine?> SyncInstances
    {
        get;
        set;
    } = new()
    {
        ["Avernus"] = null,
        ["Beatrix"] = null,
        ["Cassiopeia"] = null,
        ["Durandal"] = null,
        ["Eligos"] = null,
        ["Fenrir"] = null,
        ["Gwyndell"] = null,
        ["Harbinger"] = null,
    };
    #endregion

    #region Methods: Overrides
    public override void Construct()
    {
        try
        {
            Log("Script Manager has been constructed.", "Construct");
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
            DeactivateAll();
            Items.Clear();

            Log("Script Manager has been deconstructed.", "Deconstruct");
        }
        catch (Exception ex)
        {
            Log(ex.ToString(), "Deconstruct");
        }
    }
    #endregion

    #region Methods: Read/Write
    public Response<ScriptEngine?> Activate(string codename)
    {
        if (Items.TryGetValue(codename, out ScriptEngine? engine) && engine is not null)
        {
            return new(false,
                $"Script engine with the codename {codename} is already activated.",
                engine);
        }

        ClientInstanceIdentifier identifier = new(codename);

        Items[codename] = new RegularEngine(identifier);
        CombatInstances[codename] = new CombatEngine(identifier);

        ActivatedEngineEvent?.Invoke(codename, Items[codename]);

        return new(true,
            $"Script engine with the codename {codename} has been activated.",
            Items[codename]);
    }

    public Response<ScriptEngine?> Deactivate(string codename)
    {
        if (Items.TryGetValue(codename, out ScriptEngine? engine) && engine is null)
        {
            return new(false,
                $"Script engine with the codename {codename} is already deactivated.",
                engine);
        }

        Items[codename]!.Destruct();
        Items[codename] = null;

        CombatInstances[codename]!.Destruct();
        CombatInstances[codename] = null;

        DeactivatedEngineEvent?.Invoke(codename, null);

        return new(true,
            $"Script engine with the codename {codename} has been deactivated.",
            null);
    }

    public Response<ScriptEngine?> DeactivateAll()
    {
        foreach (KeyValuePair<string, ScriptEngine?> engine in Items)
        {
            Deactivate(engine.Key);
        }

        return new(true,
            "All script engines have been deactivated.",
            null);
    }
    #endregion

    #region Methods: Invoker
    public Response<IReadableModel?> InvokeTrigger(string codename, object gameModel)
    {
        if (Items.TryGetValue(codename, out ScriptEngine? engine) && engine is null)
        {
            return new(false,
                $"Script engine with the codename {codename} is not activated yet.",
                null);
        }

        World world = MSVCWorld.Instance.Collection[codename]!;
        IReadableModel readableModel = null;
        ScriptTriggerType triggerType = ScriptTriggerType.Variable;

        switch (gameModel)
        {
            case ActiveSkill skill:
                readableModel = new RMActiveSkill(skill, world);
                triggerType = ScriptTriggerType.Skill;
                break;

            case Area map:
                readableModel = new RMArea(map, world);
                triggerType = ScriptTriggerType.Map;
                break;

            case Aura aura:
                readableModel = new RMAura(aura, world);
                triggerType = ScriptTriggerType.Aura;
                break;

            case MainAvatar avatar:
                readableModel = new RMSelf(avatar, world);
                triggerType = ScriptTriggerType.Self;
                break;
            
            case Avatar player:
                readableModel = new RMAvatar(player, world);
                triggerType = ScriptTriggerType.Player;
                break;

            case InventoryItem item:
                readableModel = new RMInventoryItem(item, world);
                triggerType = ScriptTriggerType.Item;
                break;
            
            case BaseItem drop:
                readableModel = new RMItemDrop(drop, world);
                triggerType = ScriptTriggerType.Drop;
                break;

            case CombatMessage message:
                readableModel = new RMCombatMessage(message, world);
                triggerType = ScriptTriggerType.CombatMessage;
                break;

            case EventMessage message:
                readableModel = new RMEventMessage(message, world);
                triggerType = ScriptTriggerType.EventMessage;
                break;

            case Faction faction:
                readableModel = new RMFaction(faction, world);
                triggerType = ScriptTriggerType.Faction;
                break;

            case Monster monster:
                readableModel = new RMMonster(monster, world);
                triggerType = ScriptTriggerType.Monster;
                break;

            case Quest quest:
                readableModel = new RMQuest(quest, world);
                triggerType = ScriptTriggerType.Quest;
                break;

            case ScriptVariable variable:
                readableModel = new RMScriptVariable(variable, world);
                triggerType = ScriptTriggerType.Variable;
                break;

            default:
                return new(false,
                    "The game model is not supported by the script manager.",
                    null);
        }

        Items[codename]!.InvokeTrigger(triggerType, readableModel);
        CombatInstances[codename]!.InvokeTrigger(triggerType, readableModel);

        return new(true,
            "Successfully invoked the trigger systems.",
            readableModel);
    }
    #endregion

}
