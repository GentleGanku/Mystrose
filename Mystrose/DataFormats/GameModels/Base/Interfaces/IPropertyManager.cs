namespace Mystrose.DataFormats.GameModels.Base.Interfaces;

public interface IPropertyManager
{

    #region Manager
    Dictionary<string, PropertyInfo> Properties
    {
        get;
        set;
    }
    #endregion

    #region Methods
    PropertyInfo? GetProperty(string key);

    void SetProperty(string key, JsonNode node);

    void SetProperties(JsonObject jsonObj);

    void RefreshProperties();
    #endregion

}
