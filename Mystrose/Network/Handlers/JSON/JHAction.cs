namespace Mystrose.Network.Handlers.JSON;

public class JHAction : IJSONMessageHandler
{

    #region Commands
    public string[] HandledCommands
    {
        get =>
        [
            "uotls",
            "mtls",
            "cb",
            "ct",
            "clearAuras"
        ];
    }
    #endregion

    #region Methods: Handler
    public void Handle(GameHost host, JSONMessage message)
    {
        switch (message.Command)
        {
            case "uotls":
                HandleUotls(host, message.DataObject);
                break;
            case "mtls":
                HandleMtls(host, message.DataObject);
                break;
            case "cb":
            case "ct":
                HandleCombat(host, message.DataObject);
                break;
            case "aura+":
            case "aura++":
            case "aura-":
            case "aura--":
            //case "aura+p":
            //case "aura*":
                HandleAura(host, message.DataObject, message.Command);
                break;
            case "clearAuras":
                HandleClearAuras(host, message.DataObject);
                break;
        }
    }
    #endregion

    #region Methods: UOTLS
    private void HandleUotls(GameHost host, JsonObject obj)
    {
        Avatar? avatar = host.World.Area.Players.Find(
            (avt) =>
            {
                return avt.Name == obj["unm"].Deserialize<string>();
            });

        if (avatar is null)
        {
            avatar = JsonSerializer.Deserialize<Avatar>(obj["o"].Deserialize<JsonObject>());
            host.World.Area.Players.Add(avatar);
        }
        else
        {
            avatar.SetProperties(obj["o"].Deserialize<JsonObject>());
            if (avatar.Name.Equals(host.World.Master.Name))
            {
                host.World.Master.SetProperties(obj["o"].Deserialize<JsonObject>());
                host.ScriptManager.InvokeTriggerSystems(ScriptTriggerType.Self, host.World.Master);
            }
        }

        host.ScriptManager.InvokeTriggerSystems(ScriptTriggerType.Player, avatar);
    }
    #endregion

    #region Methods: MTLS
    private void HandleMtls(GameHost host, JsonObject obj)
    {
        Monster? monster = host.World.Area.Monsters.Find(
            (mon) =>
            {
                return mon.MonMapID == obj["id"].Deserialize<int>();
            });

        if (monster is null)
        {
            return;
        }

        if (obj.ContainsKey("targets"))
        {
            monster.Targets = obj["targets"].Deserialize<List<string>>()!;
        }

        monster.SetProperties(obj["o"].Deserialize<JsonObject>());
        
        host.ScriptManager.InvokeTriggerSystems(ScriptTriggerType.Monster, monster);
    }
    #endregion

    #region Methods: Combat
    private void HandleCombat(GameHost host, JsonObject obj)
    {
        foreach (KeyValuePair<string, JsonNode> actionObj in obj)
        {
            switch (actionObj.Key)
            {
                case "anims":
                    HandleAnims(host, (JsonArray)actionObj.Value);
                    break;
                case "a":
                    HandleAuraList(host, (JsonArray)actionObj.Value);
                    break;
                case "p":
                    HandlePlayer(host, (JsonObject)actionObj.Value);
                    break;
                case "m":
                    HandleMonster(host, (JsonObject)actionObj.Value);
                    break;
            }
        }
    }
    #endregion

    #region Methods: Animation
    private void HandleAnims(GameHost host, JsonArray anims)
    {
        foreach (JsonObject obj in anims)
        {
            CombatMessage? msg = obj.Deserialize<CombatMessage>();
            msg.RealignHeader(host.World.Area);

            host.ScriptManager.InvokeTriggerSystems(ScriptTriggerType.CombatMessage, msg);
        }
    }
    #endregion

    #region Methods: Aura
    private void HandleAura(GameHost host, JsonObject obj, string cmd)
    {
        string sourceData = obj["cInf"].Deserialize<string>();
        string targetData = obj["tInf"].Deserialize<string>();
        JsonArray? auras = obj.ContainsKey("auras") ? obj["auras"].Deserialize<JsonArray>() : [];

        if (obj.ContainsKey("aura"))
        {
            auras.Add(obj["aura"].Deserialize<JsonObject>());
        }

        switch (cmd)
        {
            case "aura+":
            case "aura++":
                foreach (JsonObject aura in auras)
                {
                    AddAura(host, aura, sourceData, targetData);

                    CombatMessage? msg = aura.Deserialize<CombatMessage>();
                    msg.Text = aura["msgOn"].Deserialize<string>() ?? aura["msgOff"].Deserialize<string>() ?? "";
                    msg.RealignHeader(host.World.Area);

                    host.ScriptManager.InvokeTriggerSystems(ScriptTriggerType.CombatMessage, msg);
                }
                break;
            case "aura-":
            case "aura--":
                foreach (JsonObject aura in auras)
                {
                    RemoveAura(host, aura, sourceData, targetData);

                    CombatMessage? msg = aura.Deserialize<CombatMessage>();
                    msg.Text = aura["msgOn"].Deserialize<string>() ?? aura["msgOff"].Deserialize<string>() ?? "";
                    msg.RealignHeader(host.World.Area);

                    host.ScriptManager.InvokeTriggerSystems(ScriptTriggerType.CombatMessage, msg);
                }
                break;
            case "aura+p":
                break;
            case "aura*":
                break;
        }
    }

    private void HandleAuraList(GameHost host, JsonArray auraObjects)
    {
        foreach (JsonObject obj in auraObjects)
        {
            HandleAura(host, obj, obj["cmd"].Deserialize<string>());
        }
    }

    private void HandleClearAuras(GameHost host, JsonObject obj)
    {
        host.World.Auras[EntityType.Player, host.World.Master.EntityID.ToString()].Clear();
    }

    private void AddAura(GameHost host, JsonObject auraObject, string sourceData, string targetData)
    {
        Aura aura = auraObject.Deserialize<Aura>().SetHeader(host.World.Auras, sourceData, targetData);
        aura.RealignHeader(host.World.Area);

        host.World.Auras.Add(aura);
        host.ScriptManager.InvokeTriggerSystems(ScriptTriggerType.Aura, aura);

        host.World.Skills.LockSkill(aura);
    }

    private void RemoveAura(GameHost host, JsonObject auraObject, string sourceData, string targetData)
    {
        Aura aura = auraObject.Deserialize<Aura>().SetHeader(host.World.Auras, sourceData, targetData);
        aura.RealignHeader(host.World.Area);

        host.World.Auras.Remove(aura);
        host.ScriptManager.InvokeTriggerSystems(ScriptTriggerType.Aura, aura);

        host.World.Skills.UnlockSkill(aura);
    }
    #endregion

    #region Methods: Monster
    private void HandleMonster(GameHost host, JsonObject monsterObjects)
    {
        foreach (KeyValuePair<string, JsonNode> obj in monsterObjects)
        {
            Monster? monster = host.World.Area.Monsters.Find(
                (mon) =>
                {
                    return mon.MonMapID == int.Parse(obj.Key);
                });

            if (monster is null)
            {
                return;
            }

            monster.SetProperties((JsonObject)obj.Value);

            host.ScriptManager.InvokeTriggerSystems(ScriptTriggerType.Monster, monster);
        }
    }
    #endregion

    #region Methods: Player
    private void HandlePlayer(GameHost host, JsonObject playerObjects)
    {
        foreach (KeyValuePair<string, JsonNode> obj in playerObjects)
        {
            Avatar? avatar = host.World.Area.Players.Find(
                (avt) =>
                {
                    return avt.Name == obj.Key;
                });

            if (avatar is null)
            {
                return;
            }

            avatar.SetProperties((JsonObject)obj.Value);
            if (avatar.Name.Equals(host.World.Master.Name))
            {
                host.World.Master.SetProperties((JsonObject)obj.Value);
                host.ScriptManager.InvokeTriggerSystems(ScriptTriggerType.Self, host.World.Master);
            }

            host.ScriptManager.InvokeTriggerSystems(ScriptTriggerType.Player, avatar);
        }
    }
    #endregion

}