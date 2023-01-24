namespace Pomodorek.Services;

public interface ITimer
{
    void Start(Action callback);
    void Stop();
}
