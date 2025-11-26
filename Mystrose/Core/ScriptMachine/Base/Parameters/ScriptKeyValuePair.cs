namespace Mystrose.Core.ScriptMachine.Base.Parameters;

/// <summary>
/// An inheritor class that represents a key value pair object.
/// </summary>
public class ScriptKeyValuePair : ScriptParameter
{

    #region Constructors
    public ScriptKeyValuePair(string key, object value, string hint = "") : base(value, hint)
    {
        Key = key;
    }
    #endregion

    #region Private Fields
    private string _key = string.Empty;
    #endregion

    #region Properties
    /// <summary>
    /// The parameter's input type.
    /// </summary>
    /// <returns>
    /// An enumeration representing the parameter's input type.
    /// </returns>
    public override ScriptParameterInputType InputType
    {
        get => ScriptParameterInputType.KeyValuePair;
    }

    /// <summary>
    /// The parameter's key.
    /// </summary>
    /// <returns>
    /// A string representing the parameter's specific key.
    /// </returns>
    public string Key
    {
        get => _key;
        protected set => SetKey(value);
    }
    #endregion

    #region Methods
    public void SetKey(string key)
    {
        _key = key;
    }

    public void SetValue(object value)
    {
        Set(value);
    }
    #endregion

    #region Methods: Overrides
    public override void Empty()
    {
        Key = string.Empty;

        base.Empty();
    }
    #endregion

}
