using System;
using System.Threading;
using Xamarin.Forms;

namespace Pomodorek.Models
{
    public class TimerModel
    {
        private readonly Action _callback;
        private static CancellationTokenSource _cancellationToken;

        public TimerModel(Action callback)
        {
            _callback = callback;
            _cancellationToken = new CancellationTokenSource();
        }

        public void Start()
        {
            var token = _cancellationToken;
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                if (token.IsCancellationRequested)
                    return false;
                _callback.Invoke();
                return true;
            });
        }

        public void Stop()
        {
            _cancellationToken.Cancel();
            Interlocked.Exchange(ref _cancellationToken, new CancellationTokenSource());
        }
    }
}
