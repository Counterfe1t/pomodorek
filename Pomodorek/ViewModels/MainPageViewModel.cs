namespace Pomodorek.ViewModels;

public class MainPageViewModel : BaseViewModel
{
    private int _seconds;
    private TimerStatusEnum _status;
    private bool _isRunning;
    private int _sessionsCount;
    private int _sessionsPassed;
    private DateTime _startTime;

    #region Properties

    public int Seconds
    {
        get => _seconds;
        set => SetProperty(ref _seconds, value);
    }

    public TimerStatusEnum Status
    {
        get => _status;
        set => SetProperty(ref _status, value);
    }

    // TODO: Change property so it represents state of the timer (running, paused, stopped)
    public bool IsRunning
    {
        get => _isRunning;
        set => SetProperty(ref _isRunning, value);
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

    private AppSettings AppSettings => _configurationService.GetAppSettings();

    public ICommand StartCommand { get; }
    public ICommand StopCommand { get; }

    public MainPageViewModel(
        ITimerService timerService,
        INotificationService notificationService,
        ISettingsService settingsService,
        IConfigurationService configurationService,
        ISoundService soundService,
        IMessageService messageService)
    {
        Title = Constants.Pages.Pomodorek;
        _timerService = timerService;
        _notificationService = notificationService;
        _settingsService = settingsService;
        _configurationService = configurationService;
        _soundService = soundService;
        _messageService = messageService;

        StartCommand = new Command(StartSession);
        StopCommand = new Command(StopSession);

        SessionsCount = _settingsService.Get(Constants.Settings.SessionsCount, AppSettings.DefaultSessionsCount);

        _messageService.Register((message) =>
        {
            if (message != Constants.AppLifecycleEvents.Resumed || !IsRunning)
                return;

            Seconds = CalculateSecondsLeft();
        });
    }

    public void DisplayNotification(string message) => _notificationService.DisplayNotification(message);

    private void StartSession()
    {
        // TODO: Handle pausing and resuming timer
        if (IsRunning)
            return;

        IsRunning = true;
        SessionsPassed = 0;
        SetTimer(TimerStatusEnum.Focus);
        _settingsService.Set(Constants.Settings.SessionsCount, SessionsCount);
        _soundService.PlaySoundAsync(Constants.Sounds.SessionStart);
    }

    // TODO: Show simple session summary
    private void StopSession()
    {
        _timerService.Stop();
        Seconds = 0;
        IsRunning = false;
        Status = TimerStatusEnum.Stopped;
    }

    private void SetTimer(TimerStatusEnum status)
    {
        Status = status;
        Seconds = GetDurationInMin(status) * Constants.SixtySeconds;

        _startTime = DateTime.Now;
        _timerService.Start(async () => await HandleOnTickEvent());
    }

    private async Task HandleOnTickEvent()
    {
        if (Seconds <= 0)
        {
            _timerService.Stop();
            await HandleOnFinishedEvent();

            return;
        }

        --Seconds;
    }

    private int CalculateSecondsLeft() =>
        GetDurationInMin(Status) - (int)((DateTime.Now.Ticks - _startTime.Ticks) / Constants.OneSecondInTicks);

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
                    DisplayNotification(Constants.Messages.SessionOver);
                    await _soundService.PlaySoundAsync(Constants.Sounds.SessionOver);
                    break;
                }

                // if four sessions passed trigger long rest
                if (SessionsPassed % 4 == 0)
                {
                    DisplayNotification(Constants.Messages.LongRest);
                    SetTimer(TimerStatusEnum.LongRest);
                    break;
                }

                DisplayNotification(Constants.Messages.ShortRest);
                SetTimer(TimerStatusEnum.ShortRest);

                break;
            case TimerStatusEnum.ShortRest:
            case TimerStatusEnum.LongRest:
                DisplayNotification(Constants.Messages.Focus);
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