namespace Pomodorek.Services;

public class TimerService : ITimerService
{
    private const short _oneSecondInMs = 1000;

    private static CancellationTokenSource _token;

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
                await Task.Delay(_oneSecondInMs);
                if (!token.IsCancellationRequested)
                    callback.Invoke();
            }
        });
    }

    public void Stop() => Interlocked.Exchange(ref _token, new CancellationTokenSource()).Cancel();
}