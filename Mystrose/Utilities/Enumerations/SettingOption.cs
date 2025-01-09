namespace Mystrose.Utilities.Enumerations;

[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum SettingOption
{
    [EnumMember(Value = "First-Time User")]
    FirstTime,
    [EnumMember(Value = "Maximized App Window on Startup")]
    MaximizedMainWindow,
    [EnumMember(Value = "Home Skip")]
    SkippableHome,
    [EnumMember(Value = "Network Debugging")]
    DebugNetwork
}
