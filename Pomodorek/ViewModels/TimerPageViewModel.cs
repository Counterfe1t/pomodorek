namespace Pomodorek.ViewModels;

public partial class TimerPageViewModel : BaseViewModel
{
    /// <summary>
    /// Timer's state (Stopped, Running, Paused).
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsRunning))]
    [NotifyPropertyChangedFor(nameof(IsStopped))]
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
    /// This object represents the <see cref="SessionModel"/> in progress.
    /// </summary>
    [ObservableProperty]
    private SessionModel _session = BaseSessionService.GetNewSession();

    public bool IsRunning => State == TimerStateEnum.Running;
    
    public bool IsStopped => State == TimerStateEnum.Stopped;

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
    }

    [RelayCommand]
    void Start()
    {
        State = TimerStateEnum.Running;
        Session.TriggerAlarmAt = TriggerAlarmAt = _dateTimeService.UtcNow.AddSeconds(Time).AddSeconds(1);

        _sessionService.StartInterval(Session);
        _timerService.Start(HandleOnTickEvent);
    }

    [RelayCommand]
    void Pause()
    {
        _timerService.Stop(true);
        State = TimerStateEnum.Paused;
    }

    [RelayCommand]
    void Stop()
    {
        if (IsStopped)
            return;

        StopTimer(true);
    }

    [RelayCommand]
    void Reset()
    {
        Session = BaseSessionService.GetNewSession();
        Stop();
    }

    public void UpdateTimerUI() => Time = _sessionService.GetIntervalLengthInSec(Session.CurrentInterval);

    public async Task CheckAndRequestPermissionsAsync() => await _permissionsService.CheckAndRequestPermissionsAsync();

    private void StopTimer(bool isStoppedManually)
    {
        _timerService.Stop(isStoppedManually);
        State = TimerStateEnum.Stopped;
        UpdateTimerUI();
    }
    
    private void HandleOnTickEvent()
    {
        var secondsRemaining = (int)TriggerAlarmAt.Subtract(_dateTimeService.UtcNow).TotalSeconds;
        if (secondsRemaining > 0)
        {
            Time = secondsRemaining;
            return;
        }

        _sessionService.FinishInterval(Session);
        StopTimer(false);
    }
}