namespace Mystrose.Utilities.Tools;

public static class JSONParser
{

    #region Fields
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        WriteIndented = true
    };
    #endregion
    
    #region Methods: Serializer
    public static string Serialize<T>(T obj)
    {
        return JsonSerializer.Serialize(obj, SerializerOptions);
    }

    public static JsonNode SerializeToNode<T>(T obj)
    {
        return JsonSerializer.SerializeToNode(obj, SerializerOptions)!;
    }

    public static T Deserialize<T>(string json)
    {
        return JsonSerializer.Deserialize<T>(json, SerializerOptions)!;
    }
    #endregion

    #region Methods: Conversion
    public static Dictionary<string, string> ConvertToAttributes(object obj)
    {
        var serializedObj = JsonSerializer.Serialize(obj);
        var deserializedObj = JsonSerializer.Deserialize<JsonObject>(serializedObj);
        var attributes = new Dictionary<string, string>();

        if (deserializedObj is null)
        {
            return [];
        }

        foreach (var property in deserializedObj)
        {
            attributes.Add(property.Key.Replace("_", " "), property.Value!.ToString());
        }

        return attributes;
    }
    #endregion
    
}