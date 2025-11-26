namespace Mystrose.Core.ScriptMachine.Base.Enumerations;

/// <summary>
/// An enumeration that represents every status type for Script Codeline.
/// </summary>
[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum ScriptCodelineStatusType
{
    [EnumMember(Value = "Idle")]
    Idle = 001,
    [EnumMember(Value = "Standby")]
    Standby = 002,

    [EnumMember(Value = "Executing")]
    Executing = 101,

    [EnumMember(Value = "Canceled")]
    Canceled = 201,
    [EnumMember(Value = "Failed")]
    Failed = 202,
    [EnumMember(Value = "Succeed")]
    Succeed = 203
}
