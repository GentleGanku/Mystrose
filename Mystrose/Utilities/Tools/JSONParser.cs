namespace Mystrose.Utilities.Tools;

public static class JSONParser
{

    #region Fields
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        WriteIndented = true
    };
    #endregion
    
    #region Methods
    public static string Serialize<T>(T obj)
    {
        return JsonSerializer.Serialize(obj, SerializerOptions);
    }
    
    public static T Deserialize<T>(string json)
    {
        return JsonSerializer.Deserialize<T>(json, SerializerOptions)!;
    }
    #endregion
    
}