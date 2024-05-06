using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mystrose.Utilities.Converters;

public class TrimConverter : JsonConverter<string>
{

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(string);
    }

    public override string Read(ref Utf8JsonReader reader, Type objectType, JsonSerializerOptions options)
    {
        return reader.GetString()?.Trim();
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }

}