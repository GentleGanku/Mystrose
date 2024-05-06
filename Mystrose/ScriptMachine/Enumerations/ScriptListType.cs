using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Mystrose.ScriptMachine.Enumerations;

/// <summary>
/// An enumeration that represents every script list type.
/// </summary>
[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum ScriptListType
{
    [EnumMember(Value = "Action")]
    Action,
    [EnumMember(Value = "Statement")]
    Statement
}
