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
    public void Handle(JSONMessage message)
    {
        switch (message.Command)
        {
            case "getQuests":
            case "getQuests2":
                HandleGet(message);
                break;
            case "acceptQuest":
                HandleAccept(message);
                break;
            case "ccqr":
                HandleComplete(message);
                break;
            case "updateQuest":
                HandleUpdate(message);
                break;
        }
    }
    #endregion

    #region Methods: Get
    private void HandleGet(JSONMessage message)
    {
        World world = message.World;
        JsonObject obj = message.DataObject;

        foreach (KeyValuePair<string, JsonNode> questObj in obj["quests"].Deserialize<JsonObject>()!)
        {
            Quest? quest = questObj.Value.Deserialize<Quest>();

            world.Environment.Quests.Add(quest);
        }
    }
    #endregion

    #region Methods: Accept
    private void HandleAccept(JSONMessage message)
    {
        World world = message.World;
        JsonObject obj = message.DataObject;

        bool isSuccess = obj["bSuccess"].Deserialize<int>() == 1;

        int id = obj["QuestID"].Deserialize<int>();

        Quest? quest = world.Environment.Quests.Find(
            (q) =>
            {
                return q.ID == id;
            });

        quest.StatusType = isSuccess ? QuestStatusType.Active : QuestStatusType.Inactive;

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, quest);
    }
    #endregion

    #region Methods: Complete
    private void HandleComplete(JSONMessage message)
    {
        World world = message.World;
        JsonObject obj = message.DataObject;

        bool isSuccess = obj["bSuccess"].Deserialize<int>() == 1;

        if (!isSuccess)
        {
            return;
        }

        Quest? quest = world.Environment.Quests.Find(
            (q) =>
            {
                return q.ID == obj["QuestID"].Deserialize<int>();
            });

        quest.StatusType = QuestStatusType.Inactive;

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, quest);
    }
    #endregion

    #region Methods: Update
    private void HandleUpdate(JSONMessage message)
    {
        World world = message.World;
        JsonObject obj = message.DataObject;

        int index = obj["iIndex"].Deserialize<int>();
        int value = obj["iValue"].Deserialize<int>();

        // WIP
    }
    #endregion

}
