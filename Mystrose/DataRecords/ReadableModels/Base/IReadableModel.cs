namespace Mystrose.DataRecords.ReadableModels.Base;

public interface IReadableModel
{

    #region Properties: Attributes
    Dictionary<string, object> KeyProperties
    {
        get;
    }
    #endregion

    #region Methods: Conversion
    object ToObject();
    
    string ToString();
    #endregion

}