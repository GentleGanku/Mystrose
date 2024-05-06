using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mystrose.Utilities.Converters;

public class IntStringConverter : JsonConverter<string>
{

    public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Number)
        {
            return reader.GetDouble().ToString();
        }
        else if (reader.TokenType == JsonTokenType.String)
        {
            return reader.GetString();
        }

        throw new JsonException("Unable to convert value to string.");
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }

}
