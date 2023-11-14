namespace Pomodorek.Services;

public class MessageService : IMessageService
{
    private readonly WeakReferenceMessenger _messenger;

    
    public MessageService(WeakReferenceMessenger messenger)
    {
        _messenger = messenger;
    }

    // TODO: Use generic type parameters
    public void Send(string message) => _messenger.Send(message);

    public void Register(Action<string> callback) =>
        _messenger.Register<string>(this, (recipient, handler) => callback.Invoke(handler));
}