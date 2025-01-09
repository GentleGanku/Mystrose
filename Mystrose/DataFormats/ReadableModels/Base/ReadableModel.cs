namespace Mystrose.DataFormats.ReadableModels.Base;

public abstract class ReadableModel<T>(T model, World world) : IReadableModel where T : class
{

    #region Properties
    [JsonIgnore]
    protected T Model
    {
        get;
        init;
    } = model;
    
    [JsonIgnore]
    protected World World
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
    public abstract override string ToString();
    #endregion

}
