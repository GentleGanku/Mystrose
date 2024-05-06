using Mystrose.Controls.Main;
using Mystrose.GameModels.General;
using Mystrose.Network.Messages;
using Mystrose.Network.Messages.Interfaces;
using Mystrose.ScriptMachine.Enumerations;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Mystrose.Network.Handlers.JSON;

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
    public void Handle(GameHost host, JSONMessage message)
    {
        switch (message.Command)
        {
            case "sAct":
                HandleSkillAction(host, message.DataObject);
                break;
            case "seia":
                HandleConsumable(host, message.DataObject);
                break;
        }
    }
    #endregion

    #region Methods: Skill Action
    private void HandleSkillAction(GameHost host, JsonObject obj)
    {
        JsonObject actions = obj["actions"].Deserialize<JsonObject>();

        foreach (KeyValuePair<string, JsonNode> actionObj in actions)
        {
            switch (actionObj.Key)
            {
                case "passive":
                    break;
                case "active":
                    host.World.Master.ActiveSkills = actionObj.Value.Deserialize<List<ActiveSkill>>();
                    host.World.Skills = new(host, host.World.Master.ActiveSkills);

                    host.World.Skills.ForEach(s => s.Index = host.World.Skills.IndexOf(s));
                    break;
            }
        }
    }
    #endregion

    #region Methods: Consumable
    private void HandleConsumable(GameHost host, JsonObject obj)
    {
        bool isReset = obj["iRes"].Deserialize<int>() == 1;

        if (!isReset)
        {
            return;
        }

        host.World.Skills[5].SetProperties(obj["o"].Deserialize<JsonObject>());

        host.ScriptManager.InvokeTriggerSystems(ScriptTriggerType.Skill, host.World.Skills[5]);
    }
    #endregion

}
