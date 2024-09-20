namespace Mystrose.Utilities.Converters;

public class StringBoolConverter : JsonConverter<bool>
{

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(bool);
    }

    public override bool Read(ref Utf8JsonReader reader, Type objectType, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            return reader.GetString() == "1";
        }
        else if (reader.TokenType == JsonTokenType.Number)
        {
            return reader.GetInt32() == 1;
        }
        else
        {
            return reader.GetBoolean();
        }

        throw new JsonException("Unable to convert value to boolean.");
    }

    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value ? 1 : 0);
    }

}
