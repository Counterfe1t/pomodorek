using Pomodorek.Services;

namespace Pomodorek.Models;

public class TimerModel : ITimer
{
    private static CancellationTokenSource _token;

    public TimerModel()
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
                await Task.Delay(1000);
                if (!token.IsCancellationRequested)
                    callback.Invoke();
            }
        });
    }

    public void Stop() => Interlocked.Exchange(ref _token, new CancellationTokenSource()).Cancel();
}