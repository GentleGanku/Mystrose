namespace Mystrose.DataRecords.Service;

public class Option
{

    #region Constructor
    public Option(string title, string context, bool isInteractable, object value)
    {
        Title = title;
        Context = context;
        IsInteractable = isInteractable;
        Value = value;
    }
    #endregion

    #region Properties
    [JsonIgnore]
    public string Title
    {
        get;
        private set;
    }

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

    [JsonPropertyName("value")]
    public object Value
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
        switch (value)
        {
            case int intValue:
                Value = intValue;
                break;
            case double doubleValue:
                Value = doubleValue;
                break;
            case bool boolValue:
                Value = boolValue;
                break;

            case string stringValue:
                if (int.TryParse(value.ToString(), out int parsedInt))
                {
                    Value = parsedInt;
                }
                else if (double.TryParse(value.ToString(), out double parsedDouble))
                {
                    Value = parsedDouble;
                }
                else if (bool.TryParse(value.ToString(), out bool parsedBool))
                {
                    Value = parsedBool;
                }
                else
                {
                    Value = stringValue;
                }
                break;
        }
    }
    #endregion

}
