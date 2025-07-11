namespace Mystrose.DataFormats.ReadableModels.Base;

public abstract class ReadableModel<T>(T model, World world) : IReadableModel where T : class
{

    #region Properties
    [JsonIgnore]
    public T Model
    {
        get;
        init;
    } = model;
    
    [JsonIgnore]
    public World World
    {
        get;
        init;
    } = world;

    [JsonIgnore] 
    public Dictionary<string, object> KeyProperties
    {
        get;
        init;
    }
    #endregion
    
    #region Abstract Methods
    public object ToObject()
    {
        return Model;
    }
    
    public abstract override string ToString();
    #endregion

}
