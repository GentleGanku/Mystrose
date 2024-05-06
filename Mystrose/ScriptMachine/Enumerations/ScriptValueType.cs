using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Mystrose.ScriptMachine.Enumerations;

/// <summary>
/// An enumeration that represents every value type available in a Script Parameter.
/// </summary>
[JsonStringEnumMemberConverterOptions(deserializationFailureFallbackValue: Object)]
[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum ScriptValueType
{
    [EnumMember(Value = "Object")]
    Object,
    [EnumMember(Value = "String")]
    String,
    [EnumMember(Value = "Integer")]
    Integer,
    [EnumMember(Value = "Double")]
    Double,
    [EnumMember(Value = "Boolean")]
    Boolean
}
