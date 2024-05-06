using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Mystrose.ScriptMachine.Enumerations;

/// <summary>
/// An enumeration that represents every value type available in a Script Parameter Input.
/// </summary>
[JsonStringEnumMemberConverterOptions(deserializationFailureFallbackValue: Parameter)]
[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum ScriptParameterInputType
{
    [EnumMember(Value = "Parameter")]
    Parameter,
    [EnumMember(Value = "Options")]
    Options,
    [EnumMember(Value = "Conditional")]
    Conditional,
    [EnumMember(Value = "KeyValuePair")]
    KeyValuePair,
}
