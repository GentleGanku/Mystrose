namespace Mystrose.DataFormats.Modules;

public struct Response<T>
{

    #region Constructor
    public Response(bool isSuccess, string message, T output)
    {
        IsSuccess = isSuccess;
        Message = message;
        Output = output;
    }
    #endregion

    #region Properties
    public bool IsSuccess
    {
        get;
        private set;
    }

    public string Message
    {
        get;
        private set;
    }

    public T Output
    {
        get;
        private set;
    }
    #endregion

}
