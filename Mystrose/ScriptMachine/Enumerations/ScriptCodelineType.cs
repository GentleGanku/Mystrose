using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Mystrose.ScriptMachine.Enumerations;

/// <summary>
/// An enumeration that represents every script list type.
/// </summary>
[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum ScriptCodelineType
{
    [EnumMember(Value = "Action")]
    Action,
    [EnumMember(Value = "Trigger")]
    Trigger,
    [EnumMember(Value = "Variable")]
    Variable,
    [EnumMember(Value = "Special Command")]
    SpecialCommand
}
