namespace Mystrose.Utilities.Converters;

public class StringDoubleConverter : JsonConverter<double>
{

    public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            string stringValue = reader.GetString()!;
            if (double.TryParse(stringValue, out double doubleValue))
            {
                return doubleValue;
            }
        }
        else if (reader.TokenType == JsonTokenType.Number)
        {
            if (reader.TryGetInt32(out int intValue))
            {
                return intValue;
            }
            else if (reader.TryGetDouble(out double doubleValue))
            {
                return doubleValue;
            }
        }

        throw new JsonException("Unable to convert value to double.");
    }

    public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }

}
