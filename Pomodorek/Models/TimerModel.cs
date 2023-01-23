namespace Pomodorek.Models;

public class TimerModel
{
    private readonly Action _callback;
    private static CancellationTokenSource _token;

    public TimerModel(Action callback)
    {
        _callback = callback;
        _token = new CancellationTokenSource();
    }

    public void Start()
    {
        var token = _token;
        Task.Run(async () =>
        {
            while (!token.IsCancellationRequested)
            {
                await Task.Delay(1000);
                if (!token.IsCancellationRequested)
                    _callback.Invoke();
            }
        });
    }

    public void Stop() => Interlocked.Exchange(ref _token, new CancellationTokenSource()).Cancel();
}