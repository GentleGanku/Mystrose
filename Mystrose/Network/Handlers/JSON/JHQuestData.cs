namespace Mystrose.Network.Handlers.JSON;

public static class JHQuestData
{

    #region Fields
    private static readonly Dictionary<string, Action<JSONMessage>> _handlers = new()
    {
        ["getQuests"] = HandleGetQuests,
        ["getQuests2"] = HandleGetQuests,
        ["acceptQuest"] = HandleAcceptQuest,
        ["ccqr"] = HandleCompleteQuest
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
    private static void HandleGetQuests(JSONMessage message)
    {
        foreach (KeyValuePair<string, JsonNode> questInfo in message.DataObject["quests"].Deserialize<JsonObject>()!)
        {
            Quest quest = questInfo.Value.Deserialize<Quest>()!;

            Quest? existingQuest = message.World.Quests.Find(
                (q) =>
                {
                    return q.ID == quest.ID;
                });

            if (existingQuest is null)
            {
                quest.ProcessType = QuestProcessType.Loaded;
                message.World.Quests.Add(quest);

                SVCScriptManager.InvokeTrigger(message.Identifier.Codename, quest);
            }
        }
    }

    private static void HandleAcceptQuest(JSONMessage message)
    {
        bool isSuccess = message.DataObject["bSuccess"].Deserialize<int>() == 1;
        int id = message.DataObject["QuestID"].Deserialize<int>();

        Quest? quest = message.World.Quests.Find(
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

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, quest);
    }

    private static void HandleCompleteQuest(JSONMessage message)
    {
        bool isSuccess = message.DataObject["bSuccess"].Deserialize<int>() == 1;
        int id = message.DataObject["QuestID"].Deserialize<int>();

        Quest? quest = message.World.Quests.Find(
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

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, quest);
    }
    #endregion

}
