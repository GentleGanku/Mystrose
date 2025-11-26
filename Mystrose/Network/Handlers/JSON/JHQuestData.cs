using Mystrose.DataRecords.Game;

namespace Mystrose.Network.Handlers.JSON;

public class JHQuestData() : MessageHandler<JSONMessage>(new()
{
    ["getQuests"] = HandleGetQuests,
    ["getQuests2"] = HandleGetQuests,
    ["acceptQuest"] = HandleAcceptQuest,
    ["ccqr"] = HandleCompleteQuest
})
{

    #region Methods: Handlers
    private static void HandleGetQuests(JSONMessage message)
    {
        foreach (KeyValuePair<string, JsonNode> questInfo in message.DataObject["quests"].Deserialize<JsonObject>()!)
        {
            Quest quest = questInfo.Value.Deserialize<Quest>()!;

            Quest? existingQuest = message.HostWorld.Quests.Find(
                (q) =>
                {
                    return q.ID == quest.ID;
                });

            if (existingQuest is null)
            {
                quest.ProcessType = QuestProcessType.Loaded;
                message.HostWorld.Quests.Add(quest);

                MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, quest);
            }
        }
    }

    private static void HandleAcceptQuest(JSONMessage message)
    {
        bool isSuccess = message.DataObject["bSuccess"].Deserialize<int>() == 1;
        int id = message.DataObject["QuestID"].Deserialize<int>();

        Quest? quest = message.HostWorld.Quests.Find(
            (q) =>
            {
                return q.ID == id;
            });

        if (quest is null || !isSuccess)
        {
            return;
        }

        quest.StatusType = QuestStatusType.Active;
        quest.ProcessType = QuestProcessType.Accepted;

        MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, quest);
    }

    private static void HandleCompleteQuest(JSONMessage message)
    {
        bool isSuccess = message.DataObject["bSuccess"].Deserialize<int>() == 1;
        int id = message.DataObject["QuestID"].Deserialize<int>();

        Quest? quest = message.HostWorld.Quests.Find(
            (q) =>
            {
                return q.ID == id;
            });

        if (quest is null || !isSuccess)
        {
            return;
        }

        quest.StatusType = QuestStatusType.Inactive;
        quest.ProcessType = QuestProcessType.Completed;

        MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, quest);
    }
    #endregion

}
