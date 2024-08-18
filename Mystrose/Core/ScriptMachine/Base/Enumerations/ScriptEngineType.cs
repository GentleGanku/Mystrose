namespace Mystrose.Core.ScriptMachine.Base.Enumerations;

/// <summary>
/// An enumeration that represents every script list type.
/// </summary>
[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum ScriptEngineType
{
    [EnumMember(Value = "Regular")]
    Regular,
    [EnumMember(Value = "Combat")]
    Combat,
    [EnumMember(Value = "Synchronizer")]
    Synchronizer
}
