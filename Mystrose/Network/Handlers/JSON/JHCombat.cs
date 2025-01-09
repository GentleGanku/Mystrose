namespace Mystrose.Network.Handlers.JSON;

public class JHCombat() : MessageHandler<JSONMessage>(new()
{
    ["cb"] = HandleCombat,
    ["ct"] = HandleCombat,
    ["clearAuras"] = HandleAuraClear
})
{

    #region Methods: Command
    private static void CommandAnims(JSONMessage message, JsonArray animsArray)
    {
        foreach (JsonObject anim in animsArray)
        {
            CombatMessage msg = anim.Deserialize<CombatMessage>()!;
            msg.RealignHeader(message.HostWorld.Area);

            MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, msg);
        }
    }

    private static void CommandAuraList(JSONMessage message, JsonArray aurasArray)
    {
        foreach (JsonObject auraInfo in aurasArray)
        {
            string sourceData = auraInfo["cInf"].Deserialize<string>();
            string targetData = auraInfo["tInf"].Deserialize<string>();
            JsonArray auras = auraInfo.ContainsKey("auras") ? auraInfo["auras"].Deserialize<JsonArray>()! : [];

            if (auraInfo.ContainsKey("aura"))
            {
                auras.Add(auraInfo["aura"].Deserialize<JsonObject>());
            }

            string auraCommand = auraInfo["cmd"].Deserialize<string>()!;
            switch (auraCommand)
            {
                case "aura+":
                case "aura++":
                    foreach (JsonObject aura in auras)
                    {
                        AddAura(message, aura, sourceData, targetData);

                        CombatMessage plusMsg = aura.Deserialize<CombatMessage>()!;
                        plusMsg.Text = aura["msgOn"].Deserialize<string>() ?? aura["msgOff"].Deserialize<string>() ?? "";
                        plusMsg.RealignHeader(message.HostWorld.Area);

                        MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, plusMsg);
                    }
                    break;
                case "aura-":
                case "aura--":
                    foreach (JsonObject aura in auras)
                    {
                        RemoveAura(message, aura, sourceData, targetData);

                        CombatMessage minMsg = aura.Deserialize<CombatMessage>()!;
                        minMsg.Text = aura["msgOn"].Deserialize<string>() ?? aura["msgOff"].Deserialize<string>() ?? "";
                        minMsg.RealignHeader(message.HostWorld.Area);

                        MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, minMsg);
                    }
                    break;
                case "aura+p":
                case "aura*":
                    break;
            }
        }
    }

    private static void CommandPlayer(JSONMessage message, JsonObject playerInfo)
    {
        foreach (KeyValuePair<string, JsonNode> player in playerInfo)
        {
            Avatar? avatar = message.HostWorld.Area.Players.Find(
                (avt) =>
                {
                    return avt.Name == player.Key;
                });

            if (avatar is null)
            {
                return;
            }

            avatar.SetProperties((JsonObject)player.Value);
            if (avatar.Name.Equals(message.HostWorld.Avatar.Name))
            {
                message.HostWorld.Avatar.SetProperties((JsonObject)player.Value);

                MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, message.HostWorld.Avatar);
            }

            MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, avatar);
        }
    }

    private static void CommandMonster(JSONMessage message, JsonObject monsterInfo)
    {
        foreach (KeyValuePair<string, JsonNode> monster in monsterInfo)
        {
            Monster? mon = message.HostWorld.Area.Monsters.Find(
                (mon) =>
                {
                    return mon.MonMapID == int.Parse(monster.Key);
                });

            if (mon is null)
            {
                return;
            }

            mon.SetProperties((JsonObject)monster.Value);

            MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, mon);
        }
    }
    #endregion

    #region Methods: Data
    private static void AddAura(JSONMessage message, JsonObject auraObject, string sourceData, string targetData)
    {
        Aura aura = auraObject.Deserialize<Aura>()!.SetHeader(sourceData, targetData);
        aura.RealignHeader(message.HostWorld.Area);

        message.HostWorld.Auras.Add(aura);

        MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, aura);

        if (aura.Name.Equals("Skill Locked"))
        {
            message.HostWorld.LockSkill(aura);
        }
    }

    private static void RemoveAura(JSONMessage message, JsonObject auraObject, string sourceData, string targetData)
    {
        Aura aura = auraObject.Deserialize<Aura>()!.SetHeader(sourceData, targetData);
        aura.RealignHeader(message.HostWorld.Area);

        message.HostWorld.Auras.Remove(aura);

        MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, aura);

        if (aura.Name.Equals("Skill Locked"))
        {
            message.HostWorld.UnlockSkill(aura);
        }
    }
    #endregion

    #region Methods: Handlers
    private static void HandleCombat(JSONMessage message)
    {
        foreach (KeyValuePair<string, JsonNode> actionObj in message.DataObject)
        {
            switch (actionObj.Key)
            {
                case "anims":
                    CommandAnims(message, (JsonArray)actionObj.Value);
                    break;
                case "a":
                    CommandAuraList(message, (JsonArray)actionObj.Value);
                    break;
                case "p":
                    CommandPlayer(message, (JsonObject)actionObj.Value);
                    break;
                case "m":
                    CommandMonster(message, (JsonObject)actionObj.Value);
                    break;
            }
        }
    }

    private static void HandleAuraClear(JSONMessage message)
    {
        message.HostWorld.Auras[EntityType.Player, message.HostWorld.Avatar.EntityID.ToString()]!.Clear();
    }
    #endregion

}
