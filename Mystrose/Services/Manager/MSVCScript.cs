using Mystrose.Core.ScriptMachine.Base.Records;
using Mystrose.DataRecords.Game;

namespace Mystrose.Services;

public class MSVCScript() : ManagerService<ScriptEngineOld>(nameof(MSVCScript))
{

    #region Delegates & Handlers
    public delegate void EngineHandler(string codename, ScriptEngineOld? engine);
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
    public Dictionary<string, ScriptEngineOld?> CombatInstances
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

    public Dictionary<string, ScriptEngineOld?> SyncInstances
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
    public Response<ScriptEngineOld?> Activate(string codename)
    {
        if (Items.TryGetValue(codename, out ScriptEngineOld? engine) && engine is not null)
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

    public Response<ScriptEngineOld?> Deactivate(string codename)
    {
        if (Items.TryGetValue(codename, out ScriptEngineOld? engine) && engine is null)
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

    public Response<ScriptEngineOld?> DeactivateAll()
    {
        foreach (KeyValuePair<string, ScriptEngineOld?> engine in Items)
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
        if (Items.TryGetValue(codename, out ScriptEngineOld? engine) && engine is null)
        {
            return new(false,
                $"Script engine with the codename {codename} is not activated yet.",
                null);
        }

        World world = MSVCWorld.Instance.Collection[codename]!;
        IReadableModel readableModel = null;
        ScriptEntityModelType triggerType = ScriptEntityModelType.Variable;

        switch (gameModel)
        {
            case ActiveSkill skill:
                readableModel = new RMActiveSkill(skill, world);
                triggerType = ScriptEntityModelType.Skill;
                break;

            case Area map:
                readableModel = new RMArea(map, world);
                triggerType = ScriptEntityModelType.Map;
                break;

            case Aura aura:
                readableModel = new RMAura(aura, world);
                triggerType = ScriptEntityModelType.Aura;
                break;

            case MainAvatar avatar:
                readableModel = new RMSelf(avatar, world);
                triggerType = ScriptEntityModelType.Self;
                break;
            
            case Avatar player:
                readableModel = new RMAvatar(player, world);
                triggerType = ScriptEntityModelType.Player;
                break;

            case InventoryItem item:
                readableModel = new RMInventoryItem(item, world);
                triggerType = ScriptEntityModelType.Item;
                break;
            
            case BaseItem drop:
                readableModel = new RMItemDrop(drop, world);
                triggerType = ScriptEntityModelType.Drop;
                break;

            case CombatMessage message:
                readableModel = new RMCombatMessage(message, world);
                triggerType = ScriptEntityModelType.CombatMessage;
                break;

            case EventMessage message:
                readableModel = new RMEventMessage(message, world);
                triggerType = ScriptEntityModelType.EventMessage;
                break;

            case Faction faction:
                readableModel = new RMFaction(faction, world);
                triggerType = ScriptEntityModelType.Faction;
                break;

            case Monster monster:
                readableModel = new RMMonster(monster, world);
                triggerType = ScriptEntityModelType.Monster;
                break;

            case Quest quest:
                readableModel = new RMQuest(quest, world);
                triggerType = ScriptEntityModelType.Quest;
                break;

            case ScriptVariable variable:
                readableModel = new RMScriptVariable(variable, world);
                triggerType = ScriptEntityModelType.Variable;
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
