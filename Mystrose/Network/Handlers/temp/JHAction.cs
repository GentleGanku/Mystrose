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
    public void Handle(JSONMessage message)
    {
        switch (message.Command)
        {
            case "uotls":
                HandleUotls(message);
                break;
            case "mtls":
                HandleMtls(message);
                break;
            case "cb":
            case "ct":
                HandleCombat(message);
                break;
            case "aura+":
            case "aura++":
            case "aura-":
            case "aura--":
            //case "aura+p":
            //case "aura*":
                HandleAura(message, message.DataObject, message.Command);
                break;
            case "clearAuras":
                HandleClearAuras(message);
                break;
        }
    }
    #endregion

    #region Methods: UOTLS
    private void HandleUotls(JSONMessage message)
    {
        World world = message.World;
        JsonObject obj = message.DataObject;

        Avatar? avatar = world.Environment.Area.Players.Find(
            (avt) =>
            {
                return avt.Name == obj["unm"].Deserialize<string>();
            });

        if (avatar is null)
        {
            avatar = JsonSerializer.Deserialize<Avatar>(obj["o"].Deserialize<JsonObject>());
            world.Environment.Area.Players.Add(avatar);
        }
        else
        {
            avatar.SetProperties(obj["o"].Deserialize<JsonObject>());
            if (avatar.Name.Equals(world.Avatar.Name))
            {
                world.Avatar.SetProperties(obj["o"].Deserialize<JsonObject>());

                SVCScriptManager.InvokeTrigger(message.Identifier.Codename, world.Avatar);
            }
        }

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, avatar);
    }
    #endregion

    #region Methods: MTLS
    private void HandleMtls(JSONMessage message)
    {
        World world = message.World;
        JsonObject obj = message.DataObject;

        Monster? monster = world.Environment.Area.Monsters.Find(
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

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, monster);
    }
    #endregion

    #region Methods: Combat
    private void HandleCombat(JSONMessage message)
    {
        World world = message.World;
        JsonObject obj = message.DataObject;

        foreach (KeyValuePair<string, JsonNode> actionObj in obj)
        {
            switch (actionObj.Key)
            {
                case "anims":
                    HandleAnims(message, (JsonArray)actionObj.Value);
                    break;
                case "a":
                    HandleAuraList(message, (JsonArray)actionObj.Value);
                    break;
                case "p":
                    HandlePlayer(message, (JsonObject)actionObj.Value);
                    break;
                case "m":
                    HandleMonster(message, (JsonObject)actionObj.Value);
                    break;
            }
        }
    }
    #endregion

    #region Methods: Animation
    private void HandleAnims(JSONMessage message, JsonArray anims)
    {
        foreach (JsonObject obj in anims)
        {
            CombatMessage? msg = obj.Deserialize<CombatMessage>();
            msg.RealignHeader(message.World.Environment.Area);

            SVCScriptManager.InvokeTrigger(message.Identifier.Codename, msg);
        }
    }
    #endregion

    #region Methods: Aura
    private void HandleAura(JSONMessage message, JsonObject obj, string cmd)
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
                    AddAura(message, aura, sourceData, targetData);

                    CombatMessage? msg = aura.Deserialize<CombatMessage>();
                    msg.Text = aura["msgOn"].Deserialize<string>() ?? aura["msgOff"].Deserialize<string>() ?? "";
                    msg.RealignHeader(message.World.Environment.Area);

                    SVCScriptManager.InvokeTrigger(message.Identifier.Codename, msg);
                }
                break;
            case "aura-":
            case "aura--":
                foreach (JsonObject aura in auras)
                {
                    RemoveAura(message, aura, sourceData, targetData);

                    CombatMessage? msg = aura.Deserialize<CombatMessage>();
                    msg.Text = aura["msgOn"].Deserialize<string>() ?? aura["msgOff"].Deserialize<string>() ?? "";
                    msg.RealignHeader(message.World.Environment.Area);

                    SVCScriptManager.InvokeTrigger(message.Identifier.Codename, msg);
                }
                break;
            case "aura+p":
                break;
            case "aura*":
                break;
        }
    }

    private void HandleAuraList(JSONMessage message, JsonArray auraObjects)
    {
        foreach (JsonObject obj in auraObjects)
        {
            HandleAura(message, obj, obj["cmd"].Deserialize<string>());
        }
    }

    private void HandleClearAuras(JSONMessage message)
    {
        World world = message.World;

        world.Auras[EntityType.Player, world.Avatar.EntityID.ToString()].Clear();
    }

    private void AddAura(JSONMessage message, JsonObject auraObject, string sourceData, string targetData)
    {
        World world = message.World;

        Aura aura = auraObject.Deserialize<Aura>().SetHeader(world.Auras, sourceData, targetData);
        aura.RealignHeader(world.Environment.Area);

        world.Auras.Add(aura);

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, aura);

        if (aura.Name.Equals("Skill Locked"))
        {
            world.LockSkill(aura);
        }
    }

    private void RemoveAura(JSONMessage message, JsonObject auraObject, string sourceData, string targetData)
    {
        World world = message.World;

        Aura aura = auraObject.Deserialize<Aura>().SetHeader(world.Auras, sourceData, targetData);
        aura.RealignHeader(world.Environment.Area);

        world.Auras.Remove(aura);

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, aura);

        if (aura.Name.Equals("Skill Locked"))
        {
            world.UnlockSkill(aura);
        }
    }
    #endregion

    #region Methods: Monster
    private void HandleMonster(JSONMessage message, JsonObject monsterObjects)
    {
        World world = message.World;

        foreach (KeyValuePair<string, JsonNode> obj in monsterObjects)
        {
            Monster? monster = world.Environment.Area.Monsters.Find(
                (mon) =>
                {
                    return mon.MonMapID == int.Parse(obj.Key);
                });

            if (monster is null)
            {
                return;
            }

            monster.SetProperties((JsonObject)obj.Value);

            SVCScriptManager.InvokeTrigger(message.Identifier.Codename, monster);
        }
    }
    #endregion

    #region Methods: Player
    private void HandlePlayer(JSONMessage message, JsonObject playerObjects)
    {
        World world = message.World;

        foreach (KeyValuePair<string, JsonNode> obj in playerObjects)
        {
            Avatar? avatar = world.Environment.Area.Players.Find(
                (avt) =>
                {
                    return avt.Name == obj.Key;
                });

            if (avatar is null)
            {
                return;
            }

            avatar.SetProperties((JsonObject)obj.Value);
            if (avatar.Name.Equals(world.Avatar.Name))
            {
                world.Avatar.SetProperties((JsonObject)obj.Value);

                SVCScriptManager.InvokeTrigger(message.Identifier.Codename, world.Avatar);
            }

            SVCScriptManager.InvokeTrigger(message.Identifier.Codename, avatar);
        }
    }
    #endregion

}