using Mystrose.ScriptMachine.Enumerations;
using Mystrose.ScriptMachine.Inputs;
using Mystrose.ScriptMachine.Objects;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Mystrose.ScriptMachine;

public static class ScriptRepository
{

    #region Properties
    public static readonly JsonSerializerOptions CloneOptions = new()
    {
        PreferredObjectCreationHandling = JsonObjectCreationHandling.Populate
    };
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
                SCMDList listCmd => listCmd.Clone(),
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

    #region Methods: Conversion
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
