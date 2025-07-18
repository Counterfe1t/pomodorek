namespace Pomodorek.ViewModels;

public partial class TimerPageViewModel : ViewModelBase
{
    private readonly ITimerService _timerService;
    private readonly ITimeProvider _timeProvider;
    private readonly IPermissionsService _permissionsService;
    private readonly ISessionService _sessionService;
    private readonly IPopupService _popupService;
    private readonly IAlertService _alertService;

    // TODO: Refactor the way popups are handled in the application.
    /// <summary>
    /// Popup for displaying <see cref="SessionModel" /> details.
    /// </summary>
    private SessionDetailsPopup? _popup;

    private int _secondsRemaining = 0;
    private SessionModel _session = new();
    private TimerStateEnum _state = TimerStateEnum.Stopped;

    /// <summary>
    /// Seconds remaining until the end of the current interval.
    /// </summary>
    public int SecondsRemaining
    {
        get => _secondsRemaining;
        set => SetProperty(ref _secondsRemaining, value);
    }

    /// <summary>
    /// Session currently in progress.
    /// </summary>
    public SessionModel Session
    {
        get => _session;
        set => SetProperty(ref _session, value);
    }

    /// <summary>
    /// Timer's state (stopped, running or paused).
    /// </summary>
    public TimerStateEnum State
    {
        get => _state;
        set
        {
            if (SetProperty(ref _state, value))
            {
                OnPropertyChanged(nameof(IsRunning));
                OnPropertyChanged(nameof(IsStopped));
                OnPropertyChanged(nameof(IsPaused));
            }
        }
    }

    public bool IsRunning
        => State == TimerStateEnum.Running;

    public bool IsStopped
        => State == TimerStateEnum.Stopped;

    public bool IsPaused
        => State == TimerStateEnum.Paused;

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

    protected override async Task InitializeAsync()
    {
        // TODO: Check permissions only once, when the app starts.
        await _permissionsService.CheckAndRequestPermissionsAsync();

        // Update clock when the page is shown, unless the timer is running.
        if (IsStopped)
            UpdateClock();
    }

    [RelayCommand]
    private void Start()
    {
        if (IsRunning)
            return;

        State = TimerStateEnum.Running;

        // One second is added to display full interval length throughout the first second of the interval.
        Session.TriggerAlarmAt = _timeProvider.UtcNow.AddSeconds(SecondsRemaining + 1);

        _sessionService.StartInterval(Session);
        _timerService.Start(OnTick);
    }

    [RelayCommand]
    private void Pause()
    {
        if (IsPaused)
            return;

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
        // Prompt user with confirm dialog before resetting the session.
        if (!await _alertService.DisplayConfirmAsync(Title, AppResources.TimerPage_ResetSessionText))
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
        {
            StopTimer(true);
        }
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
        {
            UpdateClock(secondsRemaining);
        }
        else
        {
            _sessionService.FinishInterval(Session);
            StopTimer(false);
        }
    }

    private void UpdateClock(int? secondsRemaining = null)
    {
        SecondsRemaining = !secondsRemaining.HasValue || secondsRemaining.Value < 0
            ? _sessionService.GetIntervalLengthInSec(Session.CurrentInterval)
            : secondsRemaining.Value;
    }
}