namespace Mystrose.Core.Settings.Base;

public struct Option
{

    #region Constructor
    public Option(string context, bool isInteractable, object value)
    {
        Context = context;
        IsInteractable = isInteractable;
        Value = value;
    }
    #endregion

    #region Properties
    [JsonIgnore]
    public string Context
    {
        get;
        private set;
    }

    [JsonIgnore]
    public bool IsInteractable
    {
        get;
        private set;
    }

    public object? Value
    {
        get;
        private set;
    }
    #endregion

    #region Methods
    public T Get<T>()
    {
        return (T)Value;
    }

    public void Set(object value)
    {
        Value = value switch
        {
            string stringValue => stringValue,
            int intValue => intValue,
            double doubleValue => doubleValue,
            bool boolValue => boolValue,
            _ => null
        };
    }
    #endregion

}
