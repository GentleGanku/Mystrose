namespace Mystrose.Network.Handlers.JSON;

public class JHQuest : IJSONMessageHandler
{

    #region Commands
    public string[] HandledCommands
    {
        get =>
        [
            "getQuests",
            "getQuests2",
            "acceptQuest",
            "ccqr",
            "updateQuest"
        ];
    }
    #endregion

    #region Methods: Handler
    public void Handle(GameHost host, JSONMessage message)
    {
        switch (message.Command)
        {
            case "getQuests":
            case "getQuests2":
                HandleGet(host, message.DataObject);
                break;
            case "acceptQuest":
                HandleAccept(host, message.DataObject);
                break;
            case "ccqr":
                HandleComplete(host, message.DataObject);
                break;
            case "updateQuest":
                HandleUpdate(host, message.DataObject);
                break;
        }
    }
    #endregion

    #region Methods: Get
    private void HandleGet(GameHost host, JsonObject obj)
    {
        foreach (KeyValuePair<string, JsonNode> questObj in obj["quests"].Deserialize<JsonObject>())
        {
            Quest? quest = questObj.Value.Deserialize<Quest>();

            host.World.Quests.Add(quest);
        }
    }
    #endregion

    #region Methods: Accept
    private void HandleAccept(GameHost host, JsonObject obj)
    {
        bool isSuccess = obj["bSuccess"].Deserialize<int>() == 1;

        int id = obj["QuestID"].Deserialize<int>();

        Quest? quest = host.World.Quests.Find(
            (q) =>
            {
                return q.ID == id;
            });

        quest.StatusType = isSuccess ? QuestStatusType.Active : QuestStatusType.Inactive;

        host.ScriptManager.InvokeTriggerSystems(ScriptTriggerType.Quest, quest);
    }
    #endregion

    #region Methods: Complete
    private void HandleComplete(GameHost host, JsonObject obj)
    {
        bool isSuccess = obj["bSuccess"].Deserialize<int>() == 1;

        if (!isSuccess)
        {
            return;
        }

        Quest? quest = host.World.Quests.Find(
            (q) =>
            {
                return q.ID == obj["QuestID"].Deserialize<int>();
            });

        quest.StatusType = QuestStatusType.Inactive;

        host.ScriptManager.InvokeTriggerSystems(ScriptTriggerType.Quest, quest);
    }
    #endregion

    #region Methods: Update
    private void HandleUpdate(GameHost host, JsonObject obj)
    {
        int index = obj["iIndex"].Deserialize<int>();
        int value = obj["iValue"].Deserialize<int>();

        // WIP
    }
    #endregion

}
