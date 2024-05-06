using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Mystrose.ScriptMachine.Enumerations;

/// <summary>
/// An enumeration that represents every script command type.
/// </summary>
[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum ScriptCommandType
{
    [EnumMember(Value = "Filler")]
    Filler,
    [EnumMember(Value = "Action")]
    Action,
    [EnumMember(Value = "Statement")]
    Statement,
    [EnumMember(Value = "Trigger")]
    Trigger,
    [EnumMember(Value = "List")]
    List,
    [EnumMember(Value = "Variable")]
    Variable
}
