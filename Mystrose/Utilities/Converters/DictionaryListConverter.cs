namespace Mystrose.Utilities.Converters;

public class DictionaryListConverter<TKey, TVal> : JsonConverter<List<TVal>>
{

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(List<TVal>);
    }

    public override List<TVal> Read(ref Utf8JsonReader reader, Type objectType, JsonSerializerOptions options)
    {
        return JsonSerializer.Deserialize<Dictionary<TKey, TVal>>(reader.GetString()).Values.ToList();
    }

    public override void Write(Utf8JsonWriter writer, List<TVal> value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

}
