namespace Pomodorek.ViewModels;

public partial class TimerPageViewModel : BaseViewModel
{
    /// <summary>
    /// Popup for displaying <see cref="SessionModel" /> details.
    /// </summary>
    private SessionDetailsPopup _popup;

    /// <summary>
    /// Timer's state (stopped, running or paused).
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsRunning))]
    [NotifyPropertyChangedFor(nameof(IsStopped))]
    private TimerStateEnum _state = TimerStateEnum.Stopped;

    /// <summary>
    /// Seconds remaining until the end of the current interval.
    /// </summary>
    [ObservableProperty]
    private int _secondsRemaining;

    /// <summary>
    /// Session currently in progress.
    /// </summary>
    [ObservableProperty]
    private SessionModel _session;

    public bool IsRunning => State == TimerStateEnum.Running;

    public bool IsStopped => State == TimerStateEnum.Stopped;

    private readonly ITimerService _timerService;
    private readonly IDateTimeService _dateTimeService;
    private readonly IPermissionsService _permissionsService;
    private readonly ISessionService _sessionService;
    private readonly IPopupService _popupService;
    private readonly IAlertService _alertService;

    public TimerPageViewModel(
        ITimerService timerService,
        IDateTimeService dateTimeService,
        IPermissionsService permissionsService,
        ISessionService sessionService,
        IPopupService popupService,
        IAlertService alertService)
        : base(Constants.Pages.Pomodorek)
    {
        _timerService = timerService;
        _dateTimeService = dateTimeService;
        _permissionsService = permissionsService;
        _sessionService = sessionService;
        _popupService = popupService;
        _alertService = alertService;

        Session = sessionService.GetSession();
    }

    [RelayCommand]
    private void Start()
    {
        State = TimerStateEnum.Running;
        Session.TriggerAlarmAt = _dateTimeService.UtcNow.AddSeconds(SecondsRemaining + 1);

        _sessionService.StartInterval(Session);
        _timerService.Start(OnTick);
    }

    [RelayCommand]
    private void Pause()
    {
        State = TimerStateEnum.Paused;

        _timerService.Stop(true);
        _sessionService.SaveSession(Session);
    }

    [RelayCommand]
    private void Stop()
    {
        if (IsStopped)
            return;

        StopTimer(true);
    }

    [RelayCommand]
    private async Task Reset()
    {
        if (!await _alertService.DisplayConfirmAsync(Title, Constants.Messages.ResetSession))
            return;

        Session = BaseSessionService.GetNewSession;

        if (IsStopped)
        {
            UpdateClock();

            _sessionService.SaveSession(Session);

            return;
        }

        StopTimer(true);
    }

    [RelayCommand]
    private void ShowSessionDetailsPopup() => _popup = _popupService.ShowSessionDetailsPopup();

    [RelayCommand]
    private void CloseSessionDetailsPopup() => _popupService.ClosePopup(_popup);

    public void UpdateClock(int? secondsRemaining = null)
    {
        if (!secondsRemaining.HasValue || secondsRemaining.Value < 0)
            SecondsRemaining = _sessionService.GetIntervalLengthInSec(Session.CurrentInterval);
        else
            SecondsRemaining = secondsRemaining.Value;
    }

    public async Task CheckAndRequestPermissionsAsync() =>
        await _permissionsService.CheckAndRequestPermissionsAsync();

    private void StopTimer(bool isStoppedManually)
    {
        State = TimerStateEnum.Stopped;

        _timerService.Stop(isStoppedManually);
        _sessionService.SaveSession(Session);

        UpdateClock();
    }

    private void OnTick()
    {
        var secondsRemaining = (int)Session.TriggerAlarmAt.Subtract(_dateTimeService.UtcNow).TotalSeconds;
        if (secondsRemaining > 0)
        {
            UpdateClock(secondsRemaining);
            return;
        }

        _sessionService.FinishInterval(Session);

        StopTimer(false);
    }
}