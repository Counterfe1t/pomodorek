namespace Pomodorek.Services;

public interface ITimerService
{
    void Start(Action callback);
    void Stop();
}
