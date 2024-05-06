using System.Text.Json;
using System.Text.Json.Nodes;

namespace Mystrose.GameModels.SkillObjects;

/// <summary>
/// A class that represents an event message (fielding a yellow text) in the game.
/// </summary>
public class EventMessage
{

    #region Constructor
    public EventMessage()
    {
        Sign = "";
        Value = "";
    }

    public EventMessage(string sign, string value)
    {
        Sign = sign;
        Value = value;
    }

    public EventMessage(string sign, JsonNode jsonValue)
    {
        Sign = sign;
        Value = jsonValue.Deserialize<string>();
    }
    #endregion

    #region Properties
    /// <summary>
    /// The sign header of the message.
    /// </summary>
    public string Sign
    {
        get;
        set;
    } = string.Empty;

    /// <summary>
    /// The code value of the message.
    /// </summary>
    public string? Value
    {
        get;
        set;
    } = string.Empty;
    #endregion

}
