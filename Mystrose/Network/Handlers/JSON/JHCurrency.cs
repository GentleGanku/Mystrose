namespace Mystrose.Network.Handlers.JSON;

public class JHCurrency() : MessageHandler<JSONMessage>(new()
{
    ["balance"] = HandleBalance,
    ["addGoldExp"] = HandleAdd,
})
{

    #region Methods: Handlers
    private static void HandleBalance(JSONMessage message)
    {
        if (message.DataObject.ContainsKey("intGold"))
        {
            int gold = message.DataObject["intGold"]!.GetValue<int>();
            message.HostWorld.Avatar.Gold = gold;
        }

        if (message.DataObject.ContainsKey("intCoins"))
        {
            int coins = message.DataObject["intCoins"]!.GetValue<int>();
            message.HostWorld.Avatar.Coins = coins;
        }
    }

    private static void HandleAdd(JSONMessage message)
    {
        if (message.DataObject.TryGetPropertyValue("intGold", out JsonNode? goldNode))
        {
            int gold = goldNode!.GetValue<int>();
            message.HostWorld.Avatar.Gold += gold;
        }

        if (message.DataObject.TryGetPropertyValue("iCP", out JsonNode? classNode))
        {
            int classPoints = classNode!.GetValue<int>();
            message.HostWorld.Avatar.ClassPoints += classPoints;
        }

        if (goldNode is not null || classNode is not null)
        {
            MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, message.HostWorld.Avatar);
        }

        if (message.DataObject.TryGetPropertyValue("FactionID", out JsonNode? factionNode))
        {
            int factionId = factionNode!.GetValue<int>();

            Faction faction = message.HostWorld.Factions.Find(
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

            MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, faction);
        }
    }
    #endregion

}
