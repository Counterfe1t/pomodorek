using Pomodorek.Interfaces;

namespace Pomodorek.ViewModels;

public partial class TimerPageViewModel : BaseViewModel
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsRunning))]
    private TimerStateEnum _state = TimerStateEnum.Stopped;

    [ObservableProperty]
    private int _seconds;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Alarm))]
    public DateTime _triggerAlarmAt;

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
        Session = _sessionService.GetNewSession();
        StopTimer();
    }

    public void Initialize()
    {
        Session = _sessionService.GetNewSession();
        Seconds = _sessionService.GetIntervalLengthInMin(IntervalEnum.Work) * Constants.OneMinuteInSec;
    }

    public async Task CheckAndRequestPermissionsAsync() =>
        await _permissionsService.CheckAndRequestPermissionsAsync();

    private void StartTimer()
    {
        State = TimerStateEnum.Running;

        var intervalLengthInMin = _sessionService.GetIntervalLengthInMin(Session.CurrentInterval);
        Seconds = intervalLengthInMin * Constants.OneMinuteInSec;

        Session.TriggerAlarmAt = TriggerAlarmAt = _dateTimeService.Now.AddMinutes(intervalLengthInMin).AddSeconds(1);
        
        _sessionService.StartInterval(Session);
        _timerService.Start(HandleOnTickEvent);
    }

    private void StopTimer()
    {
        _timerService.Stop();
        State = TimerStateEnum.Stopped;
        Seconds = _sessionService.GetIntervalLengthInMin(Session.CurrentInterval) * Constants.OneMinuteInSec;
    }

    private void HandleOnTickEvent()
    {
        var seconds = (int)(TriggerAlarmAt - _dateTimeService.Now).TotalSeconds;
        if (seconds > 0)
        {
            Seconds = seconds;
            return;
        }

        _sessionService.FinishInterval(Session);
        StopTimer();
    }
}