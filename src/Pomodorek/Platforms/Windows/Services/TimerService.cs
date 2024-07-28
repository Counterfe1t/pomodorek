namespace Pomodorek.Services;

public class TimerService : ITimerService
{
    private IDispatcherTimer _timer;

    private readonly Application _application;

    public TimerService(IApplicationService applicationService)
    {
        _application = applicationService.Application;
    }

    public void Start(Action callback)
    {
        _timer = _application.Dispatcher.CreateTimer();
        _timer.Tick += (sender, e) => callback?.Invoke();
        _timer.Interval = TimeSpan.FromMilliseconds(Constants.OneSecondInMs);
        _timer.Start();
    }

    public void Stop(bool isStoppedManually) => _timer?.Stop();
}