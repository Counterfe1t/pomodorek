using Pomodorek.Interfaces;

namespace Pomodorek.Services;

public class TimerService : ITimerService
{
    private CancellationTokenSource _token;

    public TimerService()
    {
        _token = new CancellationTokenSource();
    }

    public void Start(Action callback)
    {
        var token = _token;
        Task.Run(async () =>
        {
            while (!token.IsCancellationRequested)
            {
                await Task.Delay(Constants.OneSecondInMs);

                if (token.IsCancellationRequested)
                    return;

                callback?.Invoke();
            }
        });
    }

    public void Stop(bool isStoppedManually) =>
        Interlocked.Exchange(ref _token, new CancellationTokenSource()).Cancel();
}