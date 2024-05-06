using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Mystrose.ScriptMachine.Enumerations;

/// <summary>
/// An enumeration that represents every variable type in a Script Variable.
/// </summary>
[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum ScriptVariableType
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
