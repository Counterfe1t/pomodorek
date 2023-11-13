namespace Pomodorek.Services;

public interface IMessageService
{
    void Send(string message);

    void Register(Action<string> callback);
}