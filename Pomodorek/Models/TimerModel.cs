namespace Pomodorek.Models
{
    public class TimerModel
    {
        private readonly IDispatcherTimer _timer;
        private readonly Action _callback;
        private static CancellationTokenSource _token;

        // todo: write unit tests
        public TimerModel(Action callback)
        {
            _timer = Application.Current.Dispatcher.CreateTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(1000);
            _callback = callback;
            _token = new CancellationTokenSource();
        }

        public void Start()
        {
            _timer.Tick += OnTickEvent;
            _timer.Start();
        }

        public void Stop()
        {
            // todo: stopping does not work od android
            _token.Cancel();
            Interlocked.Exchange(ref _token, new CancellationTokenSource());
            _timer.Stop();
            _timer.Tick -= OnTickEvent;
        }

        private void OnTickEvent(object e, EventArgs sender)
        {
            if (_token.IsCancellationRequested)
                _timer.Stop();
            _callback.Invoke();
        }
    }
}
