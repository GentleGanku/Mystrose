namespace Mystrose.Utilities.Converters;

public class QuestRewardConverter : JsonConverter<List<BaseItem>>
{

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(List<BaseItem>);
    }

    public override List<BaseItem> Read(ref Utf8JsonReader reader, Type objectType, JsonSerializerOptions options)
    {
        return JsonSerializer.Deserialize<Dictionary<string, Dictionary<int, BaseItem>>>(reader.GetString()).Values.SelectMany(x => x.Values).ToList();
    }

    public override void Write(Utf8JsonWriter writer, List<BaseItem> value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

}
