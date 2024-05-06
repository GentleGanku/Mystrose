using System.Collections.Generic;
using System.Reflection;
using System.Text.Json.Nodes;

namespace Mystrose.GameModels.Base.Interfaces;

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
