using Mystrose.ReadableModels.Base;
using Mystrose.ScriptMachine.Enumerations;
using Mystrose.ScriptMachine.Inputs;
using Mystrose.ScriptMachine.Objects;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Mystrose.ScriptMachine;

public static class ScriptRepository
{

    #region Properties
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        IncludeFields = true,
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
