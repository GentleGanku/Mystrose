namespace Mystrose.Core.ScriptMachine.Base.Enumerations;

/// <summary>
/// An enumeration that represents every value type available in a Script Parameter.
/// </summary>
[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum ScriptValueType
{
    [EnumMember(Value = "String")]
    String,
    [EnumMember(Value = "Integer")]
    Integer,
    [EnumMember(Value = "Double")]
    Double,
    [EnumMember(Value = "Boolean")]
    Boolean,
}
