using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Mystrose.ScriptMachine.Enumerations;

/// <summary>
/// An enumeration that represents every script result type.
/// </summary>
[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum ScriptResultType
{
    [EnumMember(Value = "Failure")]
    Failure,
    [EnumMember(Value = "Success")]
    Success,
    [EnumMember(Value = "Error")]
    Error
}
