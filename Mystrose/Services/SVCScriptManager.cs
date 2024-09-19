using Mystrose.DataFormats.GameModels.Master;

namespace Mystrose.Services;

public class SVCScriptManager
{

    #region Delegates & Handlers
    public delegate void EngineHandler(string codename, ScriptEngine? engine);
    public static event EngineHandler ActivatedEngineEvent;
    public static event EngineHandler DeactivatedEngineEvent;
    #endregion

    #region Fields
    private static readonly Dictionary<string, RegularEngine?> _regularEngines = new()
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
    private static readonly Dictionary<string, CombatEngine?> _combatEngines = new()
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
    private static readonly Dictionary<string, ScriptEngine?> _syncEngines = new()
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

    #region Methods: Read/Write
    public static Response<ScriptEngine?> Activate(string codename)
    {
        if (_regularEngines.TryGetValue(codename, out RegularEngine? engine) && engine is not null)
        {
            return new(false,
                $"Script engine with the codename {codename} is already activated.",
                engine);
        }

        ClientUseIdentifier identifier = new(codename);

        _regularEngines[codename] = new(identifier);
        _combatEngines[codename] = new(identifier);

        ActivatedEngineEvent?.Invoke(codename, _regularEngines[codename]);

        return new(true,
            $"Script engine with the codename {codename} has been activated.",
            _regularEngines[codename]);
    }

    public static Response<ScriptEngine?> Deactivate(string codename)
    {
        if (_regularEngines.TryGetValue(codename, out RegularEngine? engine) && engine is null)
        {
            return new(false,
                $"Script engine with the codename {codename} is already deactivated.",
                engine);
        }

        _regularEngines[codename]!.Destruct();
        _regularEngines[codename] = null;

        _combatEngines[codename]!.Destruct();
        _combatEngines[codename] = null;

        DeactivatedEngineEvent?.Invoke(codename, null);

        return new(true,
            $"Script engine with the codename {codename} has been deactivated.",
            null);
    }
    #endregion

    #region Methods: Invoker
    public static Response<ReadableModel?> InvokeTrigger(string codename, object gameModel)
    {
        ReadableModel? readableModel = null;
        ScriptTriggerType triggerType = ScriptTriggerType.Variable;
        World world = SVCWorldVisualizer.GetWorldDict().Output[codename]!;

        switch (gameModel)
        {
            case ScriptVariable variable:
                readableModel = new RMScriptVariable(variable, world);
                triggerType = ScriptTriggerType.Variable;
                break;

            case MainAvatar avatar:
                readableModel = new RMSelf(avatar, world);
                triggerType = ScriptTriggerType.Self;
                break;

            case Avatar player:
                readableModel = new RMAvatar(player, world);
                triggerType = ScriptTriggerType.Player;
                break;

            case Monster monster:
                readableModel = new RMMonster(monster, world);
                triggerType = ScriptTriggerType.Monster;
                break;

            case ActiveSkill skill:
                readableModel = new RMActiveSkill(skill, world);
                triggerType = ScriptTriggerType.Skill;
                break;

            case Aura aura:
                readableModel = new RMAura(aura, world);
                triggerType = ScriptTriggerType.Aura;
                break;

            case Area map:
                readableModel = new RMArea(map, world);
                triggerType = ScriptTriggerType.Map;
                break;

            case Faction faction:
                readableModel = new RMFaction(faction, world);
                triggerType = ScriptTriggerType.Faction;
                break;

            case Quest quest:
                readableModel = new RMQuest(quest, world);
                triggerType = ScriptTriggerType.Quest;
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

            default:
                return new(false,
                    "The game model is not supported by the script manager.",
                    null);
        }

        _regularEngines[codename]!.InvokeTrigger(triggerType, readableModel);
        _combatEngines[codename]!.InvokeTrigger(triggerType, readableModel);

        return new(true,
            "Successfully invoked the trigger systems.",
            readableModel);
    }
    #endregion

}
