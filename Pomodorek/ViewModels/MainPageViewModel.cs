namespace Pomodorek.ViewModels;

public partial class MainPageViewModel : BaseViewModel
{
    private bool _isRunning;
    private TimerStatusEnum _status;
    private DateTime _startTime;
    private int _seconds;
    private int _sessionsCount;
    private int _sessionsPassed;

    #region Properties

    // TODO: Make these properties observable
    // TODO: Change property so it represents state of the timer (running, paused, stopped)
    public bool IsRunning
    {
        get => _isRunning;
        set => SetProperty(ref _isRunning, value);
    }

    public TimerStatusEnum Status
    {
        get => _status;
        set => SetProperty(ref _status, value);
    }

    public int Seconds
    {
        get => _seconds;
        set => SetProperty(ref _seconds, value);
    }

    public int SessionsCount
    {
        get => _sessionsCount;
        set => SetProperty(ref _sessionsCount, value);
    }

    public int SessionsPassed
    {
        get => _sessionsPassed;
        set => SetProperty(ref _sessionsPassed, value);
    }

    #endregion

    private readonly ITimerService _timerService;
    private readonly INotificationService _notificationService;
    private readonly ISettingsService _settingsService;
    private readonly IConfigurationService _configurationService;
    private readonly ISoundService _soundService;
    private readonly IMessageService _messageService;
    private readonly IDateTimeService _dateTimeService;

    private AppSettings AppSettings => _configurationService.GetAppSettings();

    public MainPageViewModel(
        ITimerService timerService,
        INotificationService notificationService,
        ISettingsService settingsService,
        IConfigurationService configurationService,
        ISoundService soundService,
        IMessageService messageService,
        IDateTimeService dateTimeService)
    {
        Title = Constants.Pages.Pomodorek;
        _timerService = timerService;
        _notificationService = notificationService;
        _settingsService = settingsService;
        _configurationService = configurationService;
        _soundService = soundService;
        _messageService = messageService;
        _dateTimeService = dateTimeService;

        SessionsCount = _settingsService.Get(Constants.Settings.SessionsCount, AppSettings.DefaultSessionsCount);

        _messageService.Register((message) =>
        {
            if (message != Constants.AppLifecycleEvents.Resumed || !IsRunning)
                return;

            Seconds = CalculateSecondsLeft();
        });
    }

    [RelayCommand]
    public void Start()
    {
        // TODO: Handle pausing and resuming timer
        if (IsRunning)
            return;

        SessionsPassed = 0;
        SetTimer(TimerStatusEnum.Focus);
        
        _settingsService.Set(Constants.Settings.SessionsCount, SessionsCount);

        Task.Run(async () => await PlaySound(Constants.Sounds.SessionStart));
    }

    [RelayCommand]
    public void Stop()
    {
        _timerService.Stop();
        IsRunning = false;
        Status = TimerStatusEnum.Stopped;
        Seconds = 0;
        // TODO: Show simple session summary
    }

    public async Task DisplayNotification(string message) => await _notificationService.DisplayNotificationAsync(message);

    public async Task PlaySound(string fileName) => await _soundService.PlaySoundAsync(fileName);

    private void SetTimer(TimerStatusEnum status)
    {
        IsRunning = true;
        Status = status;
        Seconds = GetDurationInMin(status) * Constants.OneMinuteInSeconds;

        _startTime = _dateTimeService.Now;
        _timerService.Start(async () => await HandleOnTickEvent());
    }

    private async Task HandleOnTickEvent()
    {
        if (Seconds > 0)
        {
            --Seconds;
            return;
        }

        _timerService.Stop();
        await HandleOnFinishedEvent();
    }

    private int CalculateSecondsLeft()
    {
        var durationInSeconds = GetDurationInMin(Status) * Constants.OneMinuteInSeconds;
        var secondsElapsed = (int)((_dateTimeService.Now.Ticks - _startTime.Ticks) / Constants.OneSecondInTicks);

        return durationInSeconds - secondsElapsed;
    }

    // TODO: Does awaiting async calls delay the work of the timer?
    // TODO: Demand user input before starting another interval
    private async Task HandleOnFinishedEvent()
    {
        switch (Status)
        {
            case TimerStatusEnum.Focus:
                if (++SessionsPassed >= SessionsCount)
                {
                    StopCommand.Execute(null);
                    await DisplayNotification(Constants.Messages.SessionOver);
                    await PlaySound(Constants.Sounds.SessionOver);
                    break;
                }

                // if four sessions passed trigger long rest
                if (SessionsPassed % 4 == 0)
                {
                    await DisplayNotification(Constants.Messages.LongRest);
                    SetTimer(TimerStatusEnum.LongRest);
                    break;
                }

                await DisplayNotification(Constants.Messages.ShortRest);
                SetTimer(TimerStatusEnum.ShortRest);

                break;
            case TimerStatusEnum.ShortRest:
            case TimerStatusEnum.LongRest:
                await DisplayNotification(Constants.Messages.Focus);
                SetTimer(TimerStatusEnum.Focus);
                break;
            case TimerStatusEnum.Stopped:
            default:
                break;
        }
    }

    private int GetDurationInMin(TimerStatusEnum status) =>
        status switch
        {
            TimerStatusEnum.Focus =>
                _settingsService.Get(Constants.Settings.FocusLengthInMin, AppSettings.DefaultFocusLengthInMin),
            TimerStatusEnum.ShortRest =>
                _settingsService.Get(Constants.Settings.ShortRestLengthInMin, AppSettings.DefaultShortRestLengthInMin),
            TimerStatusEnum.LongRest =>
                _settingsService.Get(Constants.Settings.LongRestLengthInMin, AppSettings.DefaultLongRestLengthInMin),
            _ => 0,
        };
}