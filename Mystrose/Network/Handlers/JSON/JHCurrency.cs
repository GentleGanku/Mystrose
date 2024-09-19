namespace Mystrose.Network.Handlers.JSON;

public static class JHCurrency
{

    #region Fields
    private static readonly Dictionary<string, Action<JSONMessage>> _handlers = new()
    {
        ["balance"] = HandleBalance,
        ["addGoldExp"] = HandleAdd,
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

    #region Handlers
    public static void HandleBalance(JSONMessage message)
    {
        if (message.DataObject.ContainsKey("intGold"))
        {
            int gold = message.DataObject["intGold"]!.GetValue<int>();
            message.World.Avatar.Gold = gold;
        }

        if (message.DataObject.ContainsKey("intCoins"))
        {
            int coins = message.DataObject["intCoins"]!.GetValue<int>();
            message.World.Avatar.Coins = coins;
        }
    }

    public static void HandleAdd(JSONMessage message)
    {
        if (message.DataObject.TryGetPropertyValue("intGold", out JsonNode? goldNode))
        {
            int gold = goldNode!.GetValue<int>();
            message.World.Avatar.Gold += gold;
        }

        if (message.DataObject.TryGetPropertyValue("iCP", out JsonNode? classNode))
        {
            int classPoints = classNode!.GetValue<int>();
            message.World.Avatar.ClassPoints += classPoints;
        }

        if (goldNode is not null || classNode is not null)
        {
            SVCScriptManager.InvokeTrigger(message.Identifier.Codename, message.World.Avatar);
        }

        if (message.DataObject.TryGetPropertyValue("FactionID", out JsonNode? factionNode))
        {
            int factionId = factionNode!.GetValue<int>();

            Faction faction = message.World.Factions.Find(
                (fct) =>
                {
                    return fct.ID == factionId;
                })!;

            int reputationPoints = message.DataObject["iRep"]!.GetValue<int>();
            faction.Points += reputationPoints;

            if (message.DataObject.ContainsKey("bonusRep"))
            {
                int bonusReputationPoints = message.DataObject["bonusRep"]!.GetValue<int>();
                faction.Points += bonusReputationPoints;
            }

            SVCScriptManager.InvokeTrigger(message.Identifier.Codename, faction);
        }
    }
    #endregion

}
