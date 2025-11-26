namespace Mystrose.DataRecords.ReadableModels.Base;

public abstract class ReadableModel(object model, World world) : IReadableModel
{

    #region Properties: I/O
    [JsonIgnore]
    public virtual object Model
    {
        get;
        init;
    } = model;
    
    [JsonIgnore]
    public virtual World World
    {
        get;
        init;
    } = world;

    [JsonIgnore]
    public virtual Dictionary<string, object> KeyProperties
    {
        get => [];
    }
    #endregion

    #region Methods: Conversion
    public object ToObject()
    {
        return Model;
    }

    public abstract override string ToString();
    #endregion

}
