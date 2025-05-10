namespace Pomodorek.ViewModels;

public partial class TimerPageViewModel : BaseViewModel
{
    private readonly ITimerService _timerService;
    private readonly ITimeProvider _timeProvider;
    private readonly IPermissionsService _permissionsService;
    private readonly ISessionService _sessionService;
    private readonly IPopupService _popupService;
    private readonly IAlertService _alertService;

    /// <summary>
    /// Popup for displaying <see cref="SessionModel" /> details.
    /// </summary>
    private SessionDetailsPopup? _popup;

    /// <summary>
    /// Timer's state (stopped, running or paused).
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsRunning))]
    [NotifyPropertyChangedFor(nameof(IsStopped))]
    [NotifyPropertyChangedFor(nameof(IsPaused))]
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

    public bool IsPaused => State == TimerStateEnum.Paused;

    public TimerPageViewModel(
        ITimerService timerService,
        ITimeProvider timeProvider,
        IPermissionsService permissionsService,
        ISessionService sessionService,
        IPopupService popupService,
        IAlertService alertService,
        INavigationService navigationService)
        : base(AppResources.TimerPage_Title, navigationService)
    {
        _timerService = timerService;
        _timeProvider = timeProvider;
        _permissionsService = permissionsService;
        _sessionService = sessionService;
        _popupService = popupService;
        _alertService = alertService;

        Session = sessionService.GetSession();
    }

    public void UpdateClock(int? secondsRemaining = null)
    {
        if (!secondsRemaining.HasValue || secondsRemaining.Value < 0)
            SecondsRemaining = _sessionService.GetIntervalLengthInSec(Session.CurrentInterval);
        else
            SecondsRemaining = secondsRemaining.Value;
    }

    public async Task CheckAndRequestPermissionsAsync()
        => await _permissionsService.CheckAndRequestPermissionsAsync();

    [RelayCommand]
    private void Start()
    {
        State = TimerStateEnum.Running;

        // One second is added to display full interval length throughout the first second of the interval.
        Session.TriggerAlarmAt = _timeProvider.UtcNow.AddSeconds(SecondsRemaining + 1);

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
        // Do not stop the timer if it is already stopped.
        if (IsStopped)
            return;

        StopTimer(true);
    }

    [RelayCommand]
    private async Task Reset()
    {
        // Prompt user with confirm dialog before resetting the session.
        if (!await _alertService.DisplayConfirmAsync(Title, Constants.Messages.ResetSession))
            return;

        // Reset session to default.
        Session = SessionModel.Create();

        // Do not stop the timer if it is already stopped.
        if (IsStopped)
        {
            UpdateClock();
            _sessionService.SaveSession(Session);
        }
        else
            StopTimer(true);
    }

    [RelayCommand]
    private void ShowSessionDetailsPopup()
    {
        _popup ??= _popupService.ShowSessionDetailsPopup();
    }

    [RelayCommand]
    private void CloseSessionDetailsPopup()
    {
        _popupService.ClosePopup(_popup);
        _popup = null;
    }

    private void StopTimer(bool isStoppedManually)
    {
        State = TimerStateEnum.Stopped;

        _timerService.Stop(isStoppedManually);
        _sessionService.SaveSession(Session);
        UpdateClock();
    }

    private void OnTick()
    {
        // Calculate remaining seconds until the end of current interval.
        var secondsRemaining = (int)Session.TriggerAlarmAt.Subtract(_timeProvider.UtcNow).TotalSeconds;

        if (secondsRemaining > 0)
            UpdateClock(secondsRemaining);
        else
        {
            _sessionService.FinishInterval(Session);
            StopTimer(false);
        }
    }
}