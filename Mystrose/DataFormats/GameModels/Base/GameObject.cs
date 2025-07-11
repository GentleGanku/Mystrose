namespace Mystrose.DataFormats.GameModels.Base;

public abstract class GameObject
{

    #region Fields
    [JsonIgnore]
    public bool IsSerializable
    {
        get => Properties.Count > 0;
    }
    #endregion

    #region Properties
    [JsonIgnore]
    private Dictionary<string, PropertyInfo> Properties
    {
        get;
        set;
    } = [];
    #endregion

    #region Methods
    public Dictionary<string, PropertyInfo> RefreshProperties<T>(T gameObject)
    {
        Properties.Clear();

        JsonObject jsonProperties = JsonSerializer.Deserialize<JsonObject>(JsonSerializer.Serialize(gameObject))!;
        PropertyInfo[] properties = [.. GetType().GetProperties()];

        int I = 0;
        foreach (KeyValuePair<string, JsonNode> kvp in jsonProperties)
        {
            PropertyInfo property = properties[I];

            Properties.Add(kvp.Key, property);

            I++;
        }

        return Properties;
    }

    public JsonSerializerOptions GetOptions()
    {
        return new()
        {
            Converters =
            {
                new StringIntConverter(),
                new StringDoubleConverter(),
                new StringBoolConverter()
            }
        };
    }

    public PropertyInfo? GetProperty(string key)
    {
        if (!IsSerializable || !Properties.TryGetValue(key, out PropertyInfo? value))
        {
            return null;
        }

        return value;
    }

    public void SetProperty(string key, JsonNode node)
    {
        if (!IsSerializable)
        {
            return;
        }

        PropertyInfo? propInfo = GetProperty(key);
        if (propInfo is null || !propInfo.CanWrite)
        {
            return;
        }

        Properties[key].SetValue(this, node.Deserialize(propInfo.PropertyType, GetOptions()));
    }

    public void SetProperties(JsonObject jsonObj)
    {
        if (!IsSerializable)
        {
            return;
        }

        foreach (KeyValuePair<string, JsonNode> jsonProp in jsonObj)
        {
            SetProperty(jsonProp.Key, jsonProp.Value);
        }
    }
    #endregion

}
