namespace Mystrose.Network.Messages.Interfaces;

public abstract class MessageHandler<T>(Dictionary<string, Action<T>> handlers) where T : Message
{

    #region Fields
    protected readonly Dictionary<string, Action<T>> Handlers = new(handlers);
    #endregion

    #region Methods: Invoker
    public virtual void Invoke(T message)
    {
        if (!Handlers.TryGetValue(message.Command, out var handler))
        {
            return;
        }

        try
        {
            handler.Invoke(message);
        }
        catch (Exception ex)
        {
            HSVCLogger.Instance.LogOnException($"({nameof(message)} - {message.Command}) {ex}");
        }
    }
    #endregion

}
