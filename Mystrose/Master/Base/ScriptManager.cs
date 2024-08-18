namespace Mystrose.Master.Base;

public class ScriptManager
{

    #region Constructor
    public ScriptManager(GameHost host)
    {
        World = host.World;

        RegularEngine = new(host);
        CombatEngine = new(host);
    }
    #endregion

    #region Properties
    public World World
    {
        get;
        private set;
    }

    public RegularEngine RegularEngine
    {
        get;
        private set;
    }

    public CombatEngine CombatEngine
    {
        get;
        private set;
    }
    #endregion

    #region Methods
    public void InvokeTriggerSystems(ScriptTriggerType type, object obj)
    {
        object readableObj = type switch
        {
            ScriptTriggerType.Variable => new RMScriptVariable((ScriptVariable)obj, World),
            ScriptTriggerType.Self => new RMSelf((MainAvatar)obj, World),
            ScriptTriggerType.Player => new RMAvatar((Avatar)obj, World),
            ScriptTriggerType.Monster => new RMMonster((Monster)obj, World),
            ScriptTriggerType.Skill => new RMActiveSkill((ActiveSkill)obj, World),
            ScriptTriggerType.Aura => new RMAura((Aura)obj, World),
            ScriptTriggerType.Map => new RMArea((Area)obj, World),
            ScriptTriggerType.Faction => new RMFaction((Faction)obj, World),
            ScriptTriggerType.Quest => new RMQuest((Quest)obj, World),
            ScriptTriggerType.Item => new RMInventoryItem((InventoryItem)obj, World),
            ScriptTriggerType.Drop => new RMItemDrop((BaseItem)obj, World),
            ScriptTriggerType.CombatMessage => new RMCombatMessage((CombatMessage)obj, World),
            ScriptTriggerType.EventMessage => new RMEventMessage((EventMessage)obj, World),
            _ => new()
        };

        RegularEngine.InvokeTrigger(type, readableObj);
        CombatEngine.InvokeTrigger(type, readableObj);
    }
    #endregion

}
