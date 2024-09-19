namespace Mystrose.Network.Handlers.temp;

public class JHSkill : IJSONMessageHandler
{

    #region Commands
    public string[] HandledCommands
    {
        get =>
        [
            "sAct",
            "seia"
        ];
    }
    #endregion

    #region Methods: Handler
    public void Handle(JSONMessage message)
    {
        switch (message.Command)
        {
            case "sAct":
                HandleSkillAction(message);
                break;
            case "seia":
                HandleConsumable(message);
                break;
        }
    }
    #endregion

    #region Methods: Skill Action
    private void HandleSkillAction(JSONMessage message)
    {
        World world = message.World;
        JsonObject obj = message.DataObject;

        JsonObject actions = obj["actions"].Deserialize<JsonObject>()!;

        foreach (KeyValuePair<string, JsonNode> actionObj in actions)
        {
            switch (actionObj.Key)
            {
                case "passive":
                    break;
                case "active":
                    world.Avatar.ActiveSkills = actionObj.Value.Deserialize<List<ActiveSkill>>()!;
                    world.Skills.AddRange(world.Avatar.ActiveSkills);

                    world.Skills.ForEach(s => s.Index = world.Skills.IndexOf(s));
                    break;
            }
        }
    }
    #endregion

    #region Methods: Consumable
    private void HandleConsumable(JSONMessage message)
    {
        World world = message.World;
        JsonObject obj = message.DataObject;

        bool isReset = obj["iRes"].Deserialize<int>() == 1;

        if (!isReset)
        {
            return;
        }

        world.Skills[5].SetProperties(obj["o"].Deserialize<JsonObject>()!);

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, world.Skills[5]);
    }
    #endregion

}
