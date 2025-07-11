namespace Mystrose.Utilities.Enumerations;

[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum SettingOption
{
    [EnumMember(Value = "isFirstTimeUser")]
    FirstTime,
    [EnumMember(Value = "hasMaximizedAppWindowOnStartup")]
    MaximizedMainWindow,
    [EnumMember(Value = "isHomeScreenSkippable")]
    SkippableHomeScreen,
    [EnumMember(Value = "enableNetworkDebugging")]
    DebugNetwork
}
