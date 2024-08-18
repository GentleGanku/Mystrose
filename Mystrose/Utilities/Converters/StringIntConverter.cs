namespace Mystrose.Utilities.Converters;

public class StringIntConverter : JsonConverter<int>
{

    public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            string stringValue = reader.GetString();
            if (int.TryParse(stringValue, out int value))
            {
                return value;
            }
        }
        else if (reader.TokenType == JsonTokenType.Number)
        {
            return reader.GetInt32();
        }

        throw new JsonException("Unable to convert value to int.");
    }

    public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }

}
