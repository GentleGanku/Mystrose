using System.Text.Json.Serialization;
using Mystrose.Utilities.Converters;

namespace Mystrose.GameModels.Network;

/// <summary>
/// A class that represents a server in the game.
/// </summary>
public class Server
{

    #region Properties
    /// <summary>
    /// The server's Network communication endpoint.
    /// </summary>
    /// <returns>
    /// An integer representing the server's port.
    /// </returns>
    [JsonPropertyName("iPort")]
    public int Port
    {
        get;
        set;
    } = -1;

    /// <summary>
    /// The server's Internet Protocol address.
    /// </summary>
    /// <returns>
    /// A string representing the server's own address.
    /// </returns>
    [JsonPropertyName("sIP")]
    public string IP
    {
        get;
        set;
    } = string.Empty;

    /// <summary>
    /// The server's name.
    /// </summary>
    /// <returns>
    /// A string representing the server's own name.
    /// </returns>
    [JsonPropertyName("sName")]
    public string Name
    {
        get;
        set;
    } = string.Empty;

    /// <summary>
    /// The server's main language representation.
    /// </summary>
    /// <returns>
    /// A string representing the server's main language type.
    /// </returns>
    [JsonPropertyName("sLang")]
    public string Language
    {
        get;
        set;
    } = string.Empty;

    /// <summary>
    /// The server's maximum player count.
    /// </summary>
    /// <returns>
    /// An integer representing the maximum player count for the server.
    /// </returns>
    [JsonPropertyName("iMax")]
    public int MaxCount
    {
        get;
        set;
    } = 0;

    /// <summary>
    /// The server's current player count.
    /// </summary>
    /// <returns>
    /// An integer representing the current player count for the server.
    /// </returns>
    [JsonPropertyName("bCount")]
    public int PlayerCount
    {
        get;
        set;
    } = 0;

    /// <summary>
    /// The server's tag of whether it is online.
    /// </summary>
    /// <returns>
    /// A boolean representing the server's tag for Online state.
    /// </returns>
    [JsonPropertyName("bOnline")]
    [JsonConverter(typeof(StringBoolConverter))]
    public bool IsOnline
    {
        get;
        set;
    } = false;

    /// <summary>
    /// The server's tag of whether it is chat-restricted.
    /// </summary>
    /// <returns>
    /// A boolean representing the server's tag for Chat Restriction state.
    /// </returns>
    [JsonPropertyName("iChat")]
    [JsonConverter(typeof(StringBoolConverter))]
    public bool IsChatRestricted
    {
        get;
        set;
    } = false;

    /// <summary>
    /// The server's tag of whether it is member-only.
    /// </summary>
    /// <returns>
    /// A boolean representing the server's tag for Upgrade state.
    /// </returns>
    [JsonPropertyName("bUpg")]
    [JsonConverter(typeof(StringBoolConverter))]
    public bool IsMemberOnly
    {
        get;
        set;
    } = false;
    #endregion

    #region Methods
    /// <summary>
    /// A method that returns the server's name.
    /// </summary>
    /// <returns>
    /// A string representing the server's name.
    /// </returns>
    public override string ToString()
    {
        return Name;
    }
    #endregion

}
