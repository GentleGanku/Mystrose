namespace Mystrose.Core.ScriptMachine.Base.Enumerations;

/// <summary>
/// An enumeration that represents every Script Parameter Type.
/// </summary>
[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum ScriptParameterType
{
    [EnumMember(Value = "Primary Parameter")]
    Primary,
    [EnumMember(Value = "Secondary Parameter")]
    Secondary,
    [EnumMember(Value = "Optional Parameter")]
    Optional,
}
