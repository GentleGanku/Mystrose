using Mystrose.DataFormats.GameModels.Master;
using System.Windows.Documents;

namespace Mystrose.Network.Handlers.JSON;

public static class JHCombat
{

    #region Fields
    private static readonly Dictionary<string, Action<JSONMessage>> _handlers = new()
    {
        ["cb"] = HandleCombat,
        ["ct"] = HandleCombat,
        ["clearAuras"] = HandleAuraClear
    };
    #endregion

    #region Methods: Invoker
    public static void Invoke(JSONMessage message)
    {
        if (!_handlers.TryGetValue(message.Command, out var handler))
        {
            return;
        }

        try
        {
            handler.Invoke(message);
        }
        catch (Exception ex)
        {
            SVCLogger.LogOnException($"({nameof(message)} - {message.Command}) {ex.ToString()}");
        }
    }
    #endregion

    #region Methods: Command
    private static void CommandAnims(JSONMessage message, JsonArray animsArray)
    {
        foreach (JsonObject anim in animsArray)
        {
            CombatMessage msg = anim.Deserialize<CombatMessage>()!;
            msg.RealignHeader(message.World.Area);

            SVCScriptManager.InvokeTrigger(message.Identifier.Codename, msg);
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
                        plusMsg.RealignHeader(message.World.Area);

                        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, plusMsg);
                    }
                    break;
                case "aura-":
                case "aura--":
                    foreach (JsonObject aura in auras)
                    {
                        RemoveAura(message, aura, sourceData, targetData);

                        CombatMessage minMsg = aura.Deserialize<CombatMessage>()!;
                        minMsg.Text = aura["msgOn"].Deserialize<string>() ?? aura["msgOff"].Deserialize<string>() ?? "";
                        minMsg.RealignHeader(message.World.Area);

                        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, minMsg);
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
            Avatar? avatar = message.World.Area.Players.Find(
                (avt) =>
                {
                    return avt.Name == player.Key;
                });

            if (avatar is null)
            {
                return;
            }

            avatar.SetProperties((JsonObject)player.Value);
            if (avatar.Name.Equals(message.World.Avatar.Name))
            {
                message.World.Avatar.SetProperties((JsonObject)player.Value);

                SVCScriptManager.InvokeTrigger(message.Identifier.Codename, message.World.Avatar);
            }

            SVCScriptManager.InvokeTrigger(message.Identifier.Codename, avatar);
        }
    }

    private static void CommandMonster(JSONMessage message, JsonObject monsterInfo)
    {
        foreach (KeyValuePair<string, JsonNode> monster in monsterInfo)
        {
            Monster? mon = message.World.Area.Monsters.Find(
                (mon) =>
                {
                    return mon.MonMapID == int.Parse(monster.Key);
                });

            if (mon is null)
            {
                return;
            }

            mon.SetProperties((JsonObject)monster.Value);

            SVCScriptManager.InvokeTrigger(message.Identifier.Codename, mon);
        }
    }
    #endregion

    #region Methods: Utility
    private static void AddAura(JSONMessage message, JsonObject auraObject, string sourceData, string targetData)
    {
        Aura aura = auraObject.Deserialize<Aura>()!.SetHeader(sourceData, targetData);
        aura.RealignHeader(message.World.Area);

        message.World.Auras.Add(aura);

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, aura);

        if (aura.Name.Equals("Skill Locked"))
        {
            message.World.LockSkill(aura);
        }
    }

    private static void RemoveAura(JSONMessage message, JsonObject auraObject, string sourceData, string targetData)
    {
        Aura aura = auraObject.Deserialize<Aura>()!.SetHeader(sourceData, targetData);
        aura.RealignHeader(message.World.Area);

        message.World.Auras.Remove(aura);

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, aura);

        if (aura.Name.Equals("Skill Locked"))
        {
            message.World.UnlockSkill(aura);
        }
    }
    #endregion

    #region Handlers
    public static void HandleCombat(JSONMessage message)
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

    public static void HandleAuraClear(JSONMessage message)
    {
        message.World.Auras[EntityType.Player, message.World.Avatar.EntityID.ToString()]!.Clear();
    }
    #endregion

}
