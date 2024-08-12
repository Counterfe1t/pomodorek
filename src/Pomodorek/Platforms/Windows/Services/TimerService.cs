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
        if (_timer is not null && _timer.IsRunning)
            return;

        _timer = _application.Dispatcher.CreateTimer();
        _timer.IsRepeating = true;
        _timer.Tick += (sender, e) => callback?.Invoke();
        _timer.Interval = TimeSpan.FromMilliseconds(Constants.OneSecondInMs);
        _timer.Start();
    }

    public void Stop(bool isStoppedManually)
    {
        _timer?.Stop();
        _timer = null;
    }
}