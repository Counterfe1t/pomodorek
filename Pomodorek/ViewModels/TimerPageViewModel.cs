namespace Pomodorek.ViewModels;

public partial class TimerPageViewModel : BaseViewModel
{
    /// <summary>
    /// Timer's state (Stopped, Running, Paused).
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsRunning))]
    private TimerStateEnum _state = TimerStateEnum.Stopped;

    /// <summary>
    /// Seconds remaining until the end of the current interval.
    /// </summary>
    [ObservableProperty]
    private int _time;

    /// <summary>
    /// Precise date and time of the scheduled alarm.
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Alarm))]
    public DateTime _triggerAlarmAt;

    /// <summary>
    /// This object represents the session in progress.
    /// </summary>
    [ObservableProperty]
    private Session _session;

    public bool IsRunning => State == TimerStateEnum.Running;

    public string Alarm => TriggerAlarmAt.ToLocalTime().ToString("HH:mm");

    private readonly ITimerService _timerService;
    private readonly IDateTimeService _dateTimeService;
    private readonly IPermissionsService _permissionsService;
    private readonly ISessionService _sessionService;

    public TimerPageViewModel(
        ITimerService timerService,
        IDateTimeService dateTimeService,
        IPermissionsService permissionsService,
        ISessionService sessionService)
        : base(Constants.Pages.Pomodorek)
    {
        _timerService = timerService;
        _dateTimeService = dateTimeService;
        _permissionsService = permissionsService;
        _sessionService = sessionService;
        Initialize();
    }

    // TODO: Handle pausing and resuming timer
    [RelayCommand]
    public void Start()
    {
        if (IsRunning)
            return;

        StartTimer();
    }

    [RelayCommand]
    public void Stop()
    {
        Session = BaseSessionService.GetNewSession();
        StopTimer(true);
    }

    public void Initialize()
    {
        Session = BaseSessionService.GetNewSession();
        UpdateTimerUI();
    }

    public void UpdateTimerUI()
    {
        Time = _sessionService.GetIntervalLengthInMin(IntervalEnum.Work) * Constants.OneMinuteInSec;
    }

    public async Task CheckAndRequestPermissionsAsync() =>
        await _permissionsService.CheckAndRequestPermissionsAsync();

    private void StartTimer()
    {
        State = TimerStateEnum.Running;
        UpdateTimerUI();
        Session.TriggerAlarmAt = TriggerAlarmAt =
            _dateTimeService.Now
                .AddMinutes(_sessionService.GetIntervalLengthInMin(Session.CurrentInterval))
                .AddSeconds(1);
        
        _sessionService.StartInterval(Session);
        _timerService.Start(HandleOnTickEvent);
    }

    private void StopTimer(bool isCancelled)
    {
        _timerService.Stop(isCancelled);
        State = TimerStateEnum.Stopped;
        UpdateTimerUI();
    }

    private void HandleOnTickEvent()
    {
        var secondsRemaining = (int)TriggerAlarmAt.Subtract(_dateTimeService.Now).TotalSeconds;
        if (secondsRemaining > 0)
        {
            Time = secondsRemaining;
            return;
        }

        _sessionService.FinishInterval(Session);
        StopTimer(false);
    }
}