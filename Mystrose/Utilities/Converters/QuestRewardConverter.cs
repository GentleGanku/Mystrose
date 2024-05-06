using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Text.Json;
using Mystrose.GameModels.Base;

namespace Warthorn.Utilities.Converters;

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
