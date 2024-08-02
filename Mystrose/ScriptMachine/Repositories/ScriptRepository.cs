using Mystrose.ScriptMachine.Enumerations;
using Mystrose.ScriptMachine.Inputs;
using Mystrose.ScriptMachine.Interfaces;
using Mystrose.ScriptMachine.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Mystrose.ScriptMachine;

public static class ScriptRepository
{

    #region Properties
    public static readonly JsonSerializerOptions SerializerOptions = new()
    {
        WriteIndented = true
    };

    private static readonly ScriptDictionary ScriptDictionary = new();
    #endregion

    #region Methods: Script
    public static bool ValidateScript(ScriptLoadout scriptLoadout)
    {
        if (scriptLoadout == null)
        {
            throw new ArgumentNullException(nameof(scriptLoadout));
        }

        if (scriptLoadout.Stances[0].Commands.Count == 0)
        {
            throw new ArgumentException("The script loadout has no commands.", nameof(scriptLoadout));
        }

        return true;
    }
    #endregion

    #region Methods: Cloning
    public static List<ScriptCommand> CloneToCommandsList(List<ScriptCommand> cmds)
    {
        List<ScriptCommand> newCmds = [];

        foreach (ScriptCommand cmd in cmds)
        {
            newCmds.Add(cmd switch
            {
                SCMDFiller fillerCmd => fillerCmd.Clone(),
                SCMDAction actionCmd => actionCmd.Clone(),
                SCMDStatement statementCmd => statementCmd.Clone(),
                SCMDTrigger triggerCmd => triggerCmd.Clone(),
                SCMDStack listCmd => listCmd.Clone(),
                SCMDVariable variableCmd => variableCmd.Clone(),
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

        System.Diagnostics.Debug.WriteLine(loadout);
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

    public static ScriptCommand ConvertToCommand(string cmd)
    {
        JsonObject jsonObj = JsonSerializer.Deserialize<JsonObject>(cmd)!;
        string id = jsonObj["ID"]!.ToString();

        ScriptCommand scriptCommand = ScriptDictionary[id]!.Clone();

        if (scriptCommand is IStackable stackableCmd)
        {
            stackableCmd.InternalCommands = ConvertToCommandsList(jsonObj["InternalCommands"]!.ToString());
        }

        scriptCommand.Parameters = ConvertToParameters(jsonObj["Parameters"]!.ToString());
        scriptCommand.SecondaryParameters = ConvertToSecondaryParameters(jsonObj["SecondaryParameters"]!.ToString());

        return scriptCommand;
    }

    public static List<ScriptCommand> ConvertToCommandsList(string cmds)
    {
        JsonArray jsonArr = JsonSerializer.Deserialize<JsonArray>(cmds)!;
        List<ScriptCommand> commands = [];

        foreach (JsonNode cmd in jsonArr)
        {
            commands.Add(ConvertToCommand(cmd!.ToString()));
        }

        return commands;
    }

    public static List<SCMDTrigger> ConvertToTriggersList(string cmds)
    {
        JsonArray jsonArr = JsonSerializer.Deserialize<JsonArray>(cmds)!;
        List<SCMDTrigger> commands = [];

        foreach (JsonNode cmd in jsonArr)
        {
            commands.Add((SCMDTrigger)ConvertToCommand(cmd!.ToString()));
        }

        return commands;
    }

    public static List<SCMDVariable> ConvertToVariablesList(string cmds)
    {
        JsonArray jsonArr = JsonSerializer.Deserialize<JsonArray>(cmds)!;
        List<SCMDVariable> commands = [];

        foreach (JsonNode cmd in jsonArr)
        {
            commands.Add((SCMDVariable)ConvertToCommand(cmd!.ToString()));
        }

        return commands;
    }

    public static Dictionary<string, ScriptParameter> ConvertToParameters(string prms)
    {
        JsonObject jsonObj = JsonSerializer.Deserialize<JsonObject>(prms)!;
        Dictionary<string, ScriptParameter> parameters = [];

        foreach (KeyValuePair<string, JsonNode> prm in jsonObj)
        {
            ScriptParameterInputType type = JsonSerializer.Deserialize<ScriptParameterInputType>(prm.Value["InputType"])!;

            parameters.Add(prm.Key, type switch
            {
                ScriptParameterInputType.Parameter => JsonSerializer.Deserialize<ScriptParameter>(prm.Value)!,
                ScriptParameterInputType.Options => JsonSerializer.Deserialize<ScriptOptions>(prm.Value)!,
                ScriptParameterInputType.Conditional => JsonSerializer.Deserialize<ScriptConditional>(prm.Value)!,
                ScriptParameterInputType.KeyValuePair => JsonSerializer.Deserialize<ScriptKeyValuePair>(prm.Value)!,
                _ => JsonSerializer.Deserialize<ScriptParameter>(prm.Value)!
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
            string propertyValue = property.Value.ToJsonString().Replace("\"", "");

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
            string propertyValue = property.Value.ToJsonString().Replace("\"", "");

            ScriptConditional cond = new(ScriptConditionalType.Equal, propertyValue);
            conditionals.Add(propertyKey, cond);
        }

        return conditionals;
    }
    #endregion

    #region Methods: Conversion From
    public static string ConvertFromLoadout(ScriptLoadout loadout)
    {
        string loadoutString = JsonSerializer.Serialize(loadout, SerializerOptions);
        JsonObject jsonObj = JsonSerializer.Deserialize<JsonObject>(loadoutString)!;

        string stancesString = ConvertFromStances(loadout.Stances);
        string triggersString = ConvertFromCommands(loadout.Triggers);
        string presetVariablesString = ConvertFromCommands(loadout.PresetVariables);

        jsonObj["Stances"] = JsonSerializer.Deserialize<JsonNode>(stancesString);
        jsonObj["Triggers"] = JsonSerializer.Deserialize<JsonNode>(triggersString);
        jsonObj["PresetVariables"] = JsonSerializer.Deserialize<JsonNode>(presetVariablesString);

        return JsonSerializer.Serialize(jsonObj, SerializerOptions);
    }

    public static string ConvertFromStances(List<ScriptStance> stances)
    {
        JsonArray jsonArr = [];

        foreach (ScriptStance stance in stances)
        {
            string stanceString = ConvertFromStance(stance);
            jsonArr.Add(JsonSerializer.Deserialize<JsonNode>(stanceString));
        }

        return JsonSerializer.Serialize(jsonArr, SerializerOptions);
    }

    public static string ConvertFromStance(ScriptStance stance)
    {
        string stancesString = JsonSerializer.Serialize(stance, SerializerOptions);
        JsonObject jsonObj = JsonSerializer.Deserialize<JsonObject>(stancesString)!;

        string commandsString = ConvertFromCommands(stance.Commands);
        jsonObj["Commands"] = JsonSerializer.Deserialize<JsonNode>(commandsString);

        return JsonSerializer.Serialize(jsonObj, SerializerOptions);
    }

    public static string ConvertFromCommand(ScriptCommand cmd)
    {
        JsonObject jsonObj = JsonSerializer.Deserialize<JsonObject>(JsonSerializer.Serialize(cmd))!;

        if (cmd is IStackable stackableCmd)
        {
            jsonObj["InternalCommands"] = JsonSerializer.Deserialize<JsonNode>(JsonSerializer.Serialize(stackableCmd.InternalCommands.Select(ConvertFromCommand))),
        }

        jsonObj["Parameters"] = JsonSerializer.Deserialize<JsonNode>(ConvertFromParameters(cmd.Parameters));
        jsonObj["SecondaryParameters"] = JsonSerializer.Deserialize<JsonNode>(ConvertFromSecondaryParameters(cmd.SecondaryParameters));

        return JsonSerializer.Serialize(jsonObj, SerializerOptions);
    }

    public static string ConvertFromCommands<T>(List<T> cmds)
    {
        JsonArray jsonArr = [];

        foreach (T cmd in cmds)
        {
            ScriptCommand pureCmd = (cmd as ScriptCommand)!;
            jsonArr.Add(JsonSerializer.Deserialize<JsonNode>(ConvertFromCommand(pureCmd)));
        }

        return JsonSerializer.Serialize(jsonArr, SerializerOptions);
    }

    public static string ConvertFromParameters(Dictionary<string, ScriptParameter> prms)
    {
        JsonObject jsonObj = [];

        foreach (KeyValuePair<string, ScriptParameter> prm in prms)
        {
            JsonObject prmValue = JsonSerializer.Deserialize<JsonObject>(prm.Value switch
            {
                ScriptConditional conditionalPrm => JsonSerializer.Serialize(conditionalPrm),
                ScriptOptions optionsPrm => JsonSerializer.Serialize(optionsPrm),
                ScriptKeyValuePair keyValuePairPrm => JsonSerializer.Serialize(keyValuePairPrm),
                ScriptParameter defaultPrm => JsonSerializer.Serialize(defaultPrm)
            })!;

            JsonNode value = prmValue[prmValue["Type"]!.ToString()]!;

            prmValue.Remove("String");
            prmValue.Remove("Integer");
            prmValue.Remove("Double");
            prmValue.Remove("Boolean");
            prmValue.Remove("Object");

            prmValue[prmValue["Type"]!.ToString()] = value;

            jsonObj.Add(prm.Key, prmValue);
        }

        return JsonSerializer.Serialize(jsonObj, SerializerOptions);
    }

    public static string ConvertFromSecondaryParameters(Dictionary<string, Dictionary<string, ScriptParameter>> prms)
    {
        JsonObject jsonObj = [];

        foreach (KeyValuePair<string, Dictionary<string, ScriptParameter>> prm in prms)
        {
            jsonObj.Add(prm.Key, JsonSerializer.Deserialize<JsonNode>(ConvertFromParameters(prm.Value)));
        }

        return JsonSerializer.Serialize(jsonObj, SerializerOptions);
    }
    #endregion

    #region Methods: Conditional
    public static ScriptConditionalType? GetCondition(string cond)
    {
        return cond switch
        {
            "==" => ScriptConditionalType.Equal,
            "!=" => ScriptConditionalType.NotEqual,
            "<=" => ScriptConditionalType.LessThanOrEqual,
            "<" => ScriptConditionalType.LessThan,
            ">=" => ScriptConditionalType.MoreThanOrEqual,
            ">" => ScriptConditionalType.MoreThan,
            "Include" => ScriptConditionalType.Include,
            "Exclude" => ScriptConditionalType.Exclude,
            _ => null
        };
    }

    public static string GetConditionString(ScriptConditionalType cond)
    {
        return cond switch
        {
            ScriptConditionalType.Equal => "==",
            ScriptConditionalType.NotEqual => "!=",
            ScriptConditionalType.LessThanOrEqual => "<=",
            ScriptConditionalType.LessThan => "<",
            ScriptConditionalType.MoreThanOrEqual => ">=",
            ScriptConditionalType.MoreThan => ">",
            ScriptConditionalType.Include => "Include",
            ScriptConditionalType.Exclude => "Exclude",
            _ => "Unknown"
        };
    }
    #endregion

}
