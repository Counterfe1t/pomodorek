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
    /// This object represents the session in progress.
    /// </summary>
    [ObservableProperty]
    private SessionModel _session;

    public bool IsRunning => State == TimerStateEnum.Running;
    
    public bool IsStopped => State == TimerStateEnum.Stopped;

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

        Session = _sessionService.GetSession();
    }

    [RelayCommand]
    void Start()
    {
        State = TimerStateEnum.Running;
        Session.TriggerAlarmAt = _dateTimeService.UtcNow.AddSeconds(Time).AddSeconds(1);

        _sessionService.StartInterval(Session);
        _timerService.Start(OnTick);
    }

    [RelayCommand]
    void Pause()
    {
        State = TimerStateEnum.Paused;

        _timerService.Stop(true);
        _sessionService.SaveSession(Session);
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
        Session = BaseSessionService.GetNewSession;

        if (IsStopped)
        {
            UpdateTimerUI();
            _sessionService.SaveSession(Session);
            return;
        }

        StopTimer(true);
    }

    public void UpdateTimerUI() => Time = _sessionService.GetIntervalLengthInSec(Session.CurrentInterval);

    public async Task CheckAndRequestPermissionsAsync() => await _permissionsService.CheckAndRequestPermissionsAsync();

    private void StopTimer(bool isStoppedManually)
    {
        State = TimerStateEnum.Stopped;

        _timerService.Stop(isStoppedManually);
        _sessionService.SaveSession(Session);
        UpdateTimerUI();
    }
    
    private void OnTick()
    {
        var secondsRemaining = (int)Session.TriggerAlarmAt.Subtract(_dateTimeService.UtcNow).TotalSeconds;
        if (secondsRemaining > 0)
        {
            Time = secondsRemaining;
            return;
        }

        _sessionService.FinishInterval(Session);
        StopTimer(false);
    }
}