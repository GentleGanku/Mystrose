using Mystrose.DataRecords.ReadableModels;

namespace Mystrose.Utilities.Tools;

public static class ScriptMachineParser
{

    #region Fields
    public const int EXECUTION_INTERVAL_TIME = 250;
    public const string INNATE_PREFIX = "INNATE.";
    public const string ADDITIONAL_PREFIX = "ADDITIONAL.";
    public const string TEST_SUBJECT_PREFIX = "TESTSUBJECT.";
    public const string CONDITIONAL_PREFIX = "CONDITIONAL.";
    public const string MANDATORY_PREFIX = "MANDATORY.";
    public const string OPTIONAL_PREFIX = "OPTIONAL.";
    
    #endregion

    #region Methods: Labeling
    public static string SanitizeLabel(string label)
    {
        return label[(label.LastIndexOf('.') + 1) ..];
    }
    #endregion

    #region Methods: Conditional
    public static ScriptConditionType GetConditionType(string conditionStr)
    {
        return conditionStr switch
        {
            "=  Equal to" => ScriptConditionType.Equal,
            "≠  Not equal to" => ScriptConditionType.NotEqual,

            "∋  To contain" => ScriptConditionType.Contains,
            "∌  To not contain" => ScriptConditionType.NotContains,
            "↦  Starts with" => ScriptConditionType.StartsWith,
            "↤  Ends with" => ScriptConditionType.EndsWith,

            "<  Less than" => ScriptConditionType.LessThan,
            "<= Less than or equal to" => ScriptConditionType.LessThanOrEqual,
            ">  More than" => ScriptConditionType.MoreThan,
            ">= More than or equal to" => ScriptConditionType.MoreThanOrEqual,

            _ => throw new ArgumentException("Invalid condition string provided.")
        };
    }

    public static Dictionary<string, ScriptParameter> GetConditionalsByModel(string model)
    {
        ScriptEntityModelType modelType = JSONParser.Deserialize<ScriptEntityModelType>(model);
        IReadableModel targetModel = modelType switch
        {
            ScriptEntityModelType.ActiveSkill => new RMActiveSkill(),
            ScriptEntityModelType.Area => new RMArea(),
            ScriptEntityModelType.Aura => new RMAura(),
            ScriptEntityModelType.Avatar => new RMAvatar(),
            ScriptEntityModelType.Cell => new RMCell(),
            ScriptEntityModelType.Faction => new RMFaction(),
            ScriptEntityModelType.InventoryItem => new RMInventoryItem(),
            ScriptEntityModelType.ItemDrop => new RMItemDrop(),
            ScriptEntityModelType.Monster => new RMMonster(),
            ScriptEntityModelType.Quest => new RMQuest(),
            ScriptEntityModelType.Self => new RMSelf(),
            ScriptEntityModelType.ShopItem => new RMShopItem(),
            ScriptEntityModelType.ScriptVariable => new RMScriptVariable(),
            _ => throw new ArgumentException("Invalid model type provided.")
        };

        Dictionary<string, ScriptParameter> parameters = ConvertToConditionals(targetModel)
            .Select(kvp => new KeyValuePair<string, ScriptParameter>(
                ADDITIONAL_PREFIX + (targetModel.KeyProperties.ContainsKey(kvp.Key) ? MANDATORY_PREFIX : OPTIONAL_PREFIX) + kvp.Key, 
                kvp.Value))
            .ToDictionary();

        return parameters;
    }

    public static Dictionary<string, ScriptParameter> GetConditionalsByEngine(string model, ScriptEngine engine, Dictionary<string, ScriptParameter> mandatories)
    {
        ScriptEntityModelType modelType = JSONParser.Deserialize<ScriptEntityModelType>(model);
        object? targetModel = modelType switch
        {
            ScriptEntityModelType.ActiveSkill => new RMActiveSkill(engine.Skills
                .Find(s => s.Index == mandatories["Index"].GetVariable(engine).Integer)),
            ScriptEntityModelType.Area => new RMArea(engine.Map),
            ScriptEntityModelType.Aura => new RMAura(engine.World.Auras
                [JSONParser.Deserialize<EntityType>(mandatories["Target Type"].GetVariable(engine).String),
                mandatories["Target ID"].GetVariable(engine).String,
                mandatories["Name"].GetVariable(engine).String]),
            ScriptEntityModelType.Avatar => new RMAvatar(engine.Map.Players
                .Find(p => p.Name
                .Equals(mandatories["Name"].GetVariable(engine).String, StringComparison.OrdinalIgnoreCase))),
            ScriptEntityModelType.Faction => new RMFaction(engine.World.Factions
                .Find(f => f.Name
                .Equals(mandatories["Name"].GetVariable(engine).String, StringComparison.OrdinalIgnoreCase))),
            ScriptEntityModelType.InventoryItem => new RMInventoryItem(engine.World.Inventories
                [JSONParser.Deserialize<InventoryType>(mandatories["Inventory Type"].GetVariable(engine).String)]
                [mandatories["Name"].GetVariable(engine).String]),
            ScriptEntityModelType.ItemDrop => new RMItemDrop(engine.World.Drops
                .Find(d => d.Name
                .Equals(mandatories["Name"].GetVariable(engine).String, StringComparison.OrdinalIgnoreCase))),
            ScriptEntityModelType.Monster => new RMMonster(engine.Map.Monsters
                .Find(m => m.MonMapID == mandatories["Monster Map ID"].GetVariable(engine).Integer)),
            ScriptEntityModelType.Quest => new RMQuest(engine.Quests
                .Find(q => q.ID == mandatories["ID"].GetVariable(engine).Integer)),
            ScriptEntityModelType.Self => new RMSelf(engine.Player),
            ScriptEntityModelType.ScriptVariable => new RMScriptVariable(engine.ActiveLoadout.Variables[mandatories["Key"].String]),
            _ => throw new ArgumentException("Invalid model type provided.")
        };

        if (targetModel is null)
        {
            return [];
        }

        Dictionary<string, ScriptParameter> parameters = JSONParser.Deserialize<JsonObject>(JSONParser.Serialize(targetModel))
            .ToDictionary(
                kvp => kvp.Key,
                kvp => new ScriptParameter(kvp.Value!.ToString()));

        return parameters;
    }
    #endregion

    #region Methods: Cloning
    public static List<ScriptCodeline> CloneToCommandsList(List<ScriptCodeline> cmds)
    {
        List<ScriptCodeline> newCmds = [];

        foreach (ScriptCodeline cmd in cmds)
        {
            newCmds.Add(cmd switch
            {
                SCLFiller fillerCmd => fillerCmd.Clone(),
                SCLAction actionCmd => actionCmd.Clone(),
                SCLStatement statementCmd => statementCmd.Clone(),
                SCLTrigger triggerCmd => triggerCmd.Clone(),
                SCLStack listCmd => listCmd.Clone(),
                SCLVariable variableCmd => variableCmd.Clone(),
            });
        }

        return newCmds;
    }

    public static Dictionary<string, ScriptParameter> CloneToParameters(Dictionary<string, ScriptParameter> prms)
    {
        Dictionary<string, ScriptParameter> newPrms = [];

        foreach (KeyValuePair<string, ScriptParameter> prm in prms)
        {
            string newKey = JsonSerializer.Deserialize<string>(JsonSerializer.Serialize(prm.Key))!;

            newPrms.Add(newKey, prm.Value switch
            {
                ScriptConditional conditionalPrm => JsonSerializer.Deserialize<ScriptConditional>(JsonSerializer.Serialize(conditionalPrm))!,
                ScriptOptions optionsPrm => JsonSerializer.Deserialize<ScriptOptions>(JsonSerializer.Serialize(optionsPrm))!,
                ScriptKeyValuePair keyValuePairPrm => JsonSerializer.Deserialize<ScriptKeyValuePair>(JsonSerializer.Serialize(keyValuePairPrm))!,
                ScriptParameter defaultPrm => JsonSerializer.Deserialize<ScriptParameter>(JsonSerializer.Serialize(defaultPrm))!
            });
        }

        return newPrms;
    }

    public static Dictionary<string, Dictionary<string, ScriptParameter>> CloneToSecondaryParameters(Dictionary<string, Dictionary<string, ScriptParameter>> prms)
    {
        Dictionary<string, Dictionary<string, ScriptParameter>> newPrms = [];

        foreach (KeyValuePair<string, Dictionary<string, ScriptParameter>> prm in prms)
        {
            string newKey = JsonSerializer.Deserialize<string>(JsonSerializer.Serialize(prm.Key))!;
            Dictionary<string, ScriptParameter> newPrm = CloneToParameters(prm.Value);

            newPrms.Add(newKey, newPrm);
        }

        return newPrms;
    }
    #endregion

    #region Methods: Conversion To
    public static ScriptLoadout ConvertToLoadout(string jsonString)
    {
        JsonObject jsonObj = JsonSerializer.Deserialize<JsonObject>(jsonString)!;

        ScriptLoadout loadout = new()
        {
            Name = jsonObj["Name"]!.ToString(),
            Description = jsonObj["Description"]!.ToString(),
            Author = jsonObj["Author"]!.ToString(),

            Stances = ConvertToStancesList(jsonObj["Stances"]!.ToString()),
            Triggers = ConvertToTriggersList(jsonObj["Triggers"]!.ToString()),
            PresetVariables = ConvertToVariablesList(jsonObj["PresetVariables"]!.ToString())
        };

        return loadout;
    }

    public static ScriptStance ConvertToStance(string jsonString)
    {
        JsonObject jsonObj = JsonSerializer.Deserialize<JsonObject>(jsonString)!;

        ScriptStance stance = new(jsonObj["Name"]!.ToString())
        {
            Commands = ConvertToCommandsList(jsonObj["Commands"]!.ToString())
        };

        return stance;
    }

    public static List<ScriptStance> ConvertToStancesList(string stances)
    {
        JsonArray jsonArr = JsonSerializer.Deserialize<JsonArray>(stances)!;
        List<ScriptStance> stancesList = [];

        foreach (JsonNode stance in jsonArr)
        {
            stancesList.Add(ConvertToStance(stance!.ToString()));
        }

        return stancesList;
    }

    public static ScriptCodeline ConvertToCommand(string cmd)
    {
        JsonObject jsonObj = JsonSerializer.Deserialize<JsonObject>(cmd)!;
        string id = jsonObj["ID"]!.ToString();

        ScriptCodeline scriptCommand = ScriptDictionary[id]!.Clone();

        if (scriptCommand is IStackable stackableCmd)
        {
            stackableCmd.InternalCommands = ConvertToCommandsList(jsonObj["InternalCommands"]!.ToString());
        }

        scriptCommand.Parameters = ConvertToParameters(jsonObj["Parameters"]!.ToString());
        scriptCommand.SecondaryParameters = ConvertToSecondaryParameters(jsonObj["SecondaryParameters"]!.ToString());

        return scriptCommand;
    }

    public static List<ScriptCodeline> ConvertToCommandsList(string cmds)
    {
        JsonArray jsonArr = JsonSerializer.Deserialize<JsonArray>(cmds)!;
        List<ScriptCodeline> commands = [];

        foreach (JsonNode cmd in jsonArr)
        {
            commands.Add(ConvertToCommand(cmd!.ToString()));
        }

        return commands;
    }

    public static List<SCLTrigger> ConvertToTriggersList(string cmds)
    {
        JsonArray jsonArr = JsonSerializer.Deserialize<JsonArray>(cmds)!;
        List<SCLTrigger> commands = [];

        foreach (JsonNode cmd in jsonArr)
        {
            commands.Add((SCLTrigger)ConvertToCommand(cmd!.ToString()));
        }

        return commands;
    }

    public static List<SCLVariable> ConvertToVariablesList(string cmds)
    {
        JsonArray jsonArr = JsonSerializer.Deserialize<JsonArray>(cmds)!;
        List<SCLVariable> commands = [];

        foreach (JsonNode cmd in jsonArr)
        {
            commands.Add((SCLVariable)ConvertToCommand(cmd!.ToString()));
        }

        return commands;
    }

    public static Dictionary<string, ScriptParameter> ConvertToParameters(string prms)
    {
        JsonObject jsonObj = JsonSerializer.Deserialize<JsonObject>(prms)!;
        Dictionary<string, ScriptParameter> parameters = [];

        foreach (KeyValuePair<string, JsonNode> prm in jsonObj)
        {
            ScriptParameterInputType type = prm.Value["InputType"].Deserialize<ScriptParameterInputType>()!;

            parameters.Add(prm.Key, type switch
            {
                ScriptParameterInputType.Parameter => prm.Value.Deserialize<ScriptParameter>()!,
                ScriptParameterInputType.Options => prm.Value.Deserialize<ScriptOptions>()!,
                ScriptParameterInputType.Conditional => prm.Value.Deserialize<ScriptConditional>()!,
                ScriptParameterInputType.KeyValuePair => prm.Value.Deserialize<ScriptKeyValuePair>()!,
                _ => prm.Value.Deserialize<ScriptParameter>()!
            });
        }

        return parameters;
    }

    public static Dictionary<string, Dictionary<string, ScriptParameter>> ConvertToSecondaryParameters(string prms)
    {
        JsonObject jsonObj = JsonSerializer.Deserialize<JsonObject>(prms)!;
        Dictionary<string, Dictionary<string, ScriptParameter>> parameters = [];

        foreach (KeyValuePair<string, JsonNode> prm in jsonObj)
        {
            Dictionary<string, ScriptParameter> secondaryParameters = ConvertToParameters(prm.Value.ToString());
            parameters.Add(prm.Key, secondaryParameters);
        }

        return parameters;
    }

    public static Dictionary<string, ScriptParameter> ConvertToParameters(object? obj)
    {
        JsonObject? jsonTarget = JsonSerializer.Deserialize<JsonObject>(JsonSerializer.Serialize(obj));
        Dictionary<string, ScriptParameter> parameters = [];

        foreach (KeyValuePair<string, JsonNode> property in jsonTarget)
        {
            string propertyKey = property.Key.Replace("_", " ");
            string propertyValue = property.Value.ToString();

            ScriptParameter param = new(propertyValue);
            parameters.Add(propertyKey, param);
        }

        return parameters;
    }

    public static Dictionary<string, ScriptParameter> ConvertToConditionals(object obj)
    {
        JsonObject? jsonTarget = JsonSerializer.Deserialize<JsonObject>(JsonSerializer.Serialize(obj));
        Dictionary<string, ScriptParameter> conditionals = [];

        foreach (KeyValuePair<string, JsonNode> property in jsonTarget)
        {
            string propertyKey = property.Key.Replace("_", " ");
            string propertyValue = property.Value.ToString();

            ScriptConditional cond = new(ScriptConditionType.Equal, propertyValue);
            conditionals.Add(propertyKey, cond);
        }

        return conditionals;
    }
    #endregion

    #region Methods: Conversion From
    public static string ConvertFromLoadout(ScriptLoadout loadout)
    {
        string loadoutString = JSONParser.Serialize(loadout);
        JsonObject jsonObj = JSONParser.Deserialize<JsonObject>(loadoutString)!;

        string stancesString = ConvertFromStances(loadout.Stances);
        string triggersString = ConvertFrom(loadout.Triggers);
        string presetVariablesString = ConvertFrom(loadout.PresetVariables);

        jsonObj["Stances"] = JSONParser.Deserialize<JsonNode>(stancesString);
        jsonObj["Triggers"] = JSONParser.Deserialize<JsonNode>(triggersString);
        jsonObj["PresetVariables"] = JSONParser.Deserialize<JsonNode>(presetVariablesString);

        return JSONParser.Serialize(jsonObj);
    }

    public static string ConvertFromStances(List<ScriptStance> stances)
    {
        JsonArray jsonArr = [];

        foreach (ScriptStance stance in stances)
        {
            string stanceString = ConvertFrom(stance);
            jsonArr.Add(JSONParser.Deserialize<JsonNode>(stanceString));
        }

        return JSONParser.Serialize(jsonArr);
    }

    public static string ConvertFrom(ScriptStance stn)
    {
        JsonObject stnObject = JSONParser.Deserialize<JsonObject>(JSONParser.Serialize(stn));



        string commandsString = ConvertFrom(stn.Commands);
        stnObject["Commands"] = JSONParser.Deserialize<JsonNode>(commandsString);

        return JSONParser.Serialize(stnObject);
    }

    public static string ConvertFrom(ScriptCodeline cmd)
    {
        JsonObject cmdObject = JSONParser.Deserialize<JsonObject>(JSONParser.Serialize(cmd));
        cmdObject["Parameters"] = JSONParser.SerializeToNode(ConvertFrom(cmd.Parameters));

        if (cmd is IStackable stackable)
        {
            cmdObject["InternalCommands"] = JSONParser.SerializeToNode(ConvertFrom(stackable.InternalCommands));
        }

        return JSONParser.Serialize(cmdObject);
    }

    public static string ConvertFrom(ScriptCodeline[] cmds)
    {
        JsonArray cmdArray = [.. cmds
            .Select(c => JSONParser.SerializeToNode(ConvertFrom(c)))];

        return JSONParser.Serialize(cmdArray);
    }

    public static string ConvertFrom(Dictionary<string, ScriptParameter> prms)
    {
        JsonObject dictObject = [.. prms
            .Select(kvp =>
            {
                JsonNode? node = kvp.Value switch
                {
                    ScriptConditional sc => JSONParser.SerializeToNode(sc),
                    ScriptOptions so => JSONParser.SerializeToNode(so),
                    ScriptKeyValuePair skvp => JSONParser.SerializeToNode(skvp),
                    ScriptParameter sp => JSONParser.SerializeToNode(sp),
                    _ => JSONParser.SerializeToNode(kvp.Value)
                };
                
                return new KeyValuePair<string, JsonNode?>(kvp.Key, node);
            })];

        return JSONParser.Serialize(dictObject);
    }
    #endregion

}
