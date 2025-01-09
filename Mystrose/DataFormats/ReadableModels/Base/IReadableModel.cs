namespace Mystrose.DataFormats.ReadableModels.Base;

public interface IReadableModel
{

    Dictionary<string, object> KeyProperties
    {
        get;
        init;
    }
    
    string ToString();
    
}