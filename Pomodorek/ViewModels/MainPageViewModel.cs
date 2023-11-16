using System.Text.Json;

namespace Pomodorek.ViewModels;

// TODO: Use community MVVM source generators
public partial class MainPageViewModel : BaseViewModel
{
    private DateTime _triggerAlarmAt;

    // TODO: Change property so it represents state of the timer (running, paused, stopped)
    private bool _isRunning;
    public bool IsRunning
    {
        get => _isRunning;
        set => SetProperty(ref _isRunning, value);
    }

    private int _seconds;
    public int Seconds
    {
        get => _seconds;
        set => SetProperty(ref _seconds, value);
    }

    private TimerStatusEnum _status;
    public TimerStatusEnum Status
    {
        get => _status;
        set => SetProperty(ref _status, value);
    }

    private int _sessionsCount;
    public int SessionsCount
    {
        get => _sessionsCount;
        set => SetProperty(ref _sessionsCount, value);
    }

    private int _sessionsPassed;
    public int SessionsPassed
    {
        get => _sessionsPassed;
        set => SetProperty(ref _sessionsPassed, value);
    }

    private readonly ITimerService _timerService;
    private readonly INotificationService _notificationService;
    private readonly ISettingsService _settingsService;
    private readonly IConfigurationService _configurationService;
    private readonly ISoundService _soundService;
    private readonly IDateTimeService _dateTimeService;

    private AppSettings AppSettings => _configurationService.GetAppSettings();

    public MainPageViewModel(
        ITimerService timerService,
        INotificationService notificationService,
        ISettingsService settingsService,
        IConfigurationService configurationService,
        ISoundService soundService,
        IDateTimeService dateTimeService)
    {
        Title = Constants.Pages.Pomodorek;
        _timerService = timerService;
        _notificationService = notificationService;
        _settingsService = settingsService;
        _configurationService = configurationService;
        _soundService = soundService;
        _dateTimeService = dateTimeService;

        SessionsCount = _settingsService.Get(Constants.Settings.SessionsCount, AppSettings.DefaultSessionsCount);
    }

    // TODO: Handle pausing and resuming timer
    [RelayCommand]
    public void Start()
    {
        if (IsRunning)
            return;

        SessionsPassed = 0;
        SetTimer(TimerStatusEnum.Focus);
        
        _settingsService.Set(Constants.Settings.SessionsCount, SessionsCount);

        Task.Run(async () => await PlaySound(Constants.Sounds.SessionStart));
    }

    // TODO: Show simple session summary
    [RelayCommand]
    public void Stop() => StopTimer();

    public async Task DisplayNotification(string message)
    {
#if WINDOWS
        await _notificationService.DisplayNotificationAsync(new NotificationDto
        {
            Content = message,
        });
#endif
    }

    public async Task PlaySound(string fileName) => await _soundService.PlaySoundAsync(fileName);

    private void SetTimer(TimerStatusEnum status)
    {
        var durationInMin = GetDurationInMin(status);
        IsRunning = true;
        Status = status;
        Seconds = durationInMin * Constants.OneMinuteInSec;
        _triggerAlarmAt = _dateTimeService.Now.AddMinutes(GetDurationInMin(status)).AddSeconds(1);

#if ANDROID
        var notification = new NotificationDto
        {
            Id = 2137,
            Title = status.ToString(),
            TriggerAlarmAt = _triggerAlarmAt,
            MaxProgress = Seconds,
            IsOngoing = true,
            OnlyAlertOnce = true,
        };

        _settingsService.Set(nameof(notification), JsonSerializer.Serialize(notification));
#endif

        _timerService.Start(async () => await HandleOnTickEvent());
    }

    private void StopTimer()
    {
        _timerService.Stop();

        IsRunning = false;
        Status = TimerStatusEnum.Stopped;
        Seconds = 0;
    }

    private async Task HandleOnTickEvent()
    {
        var seconds = (int)(_triggerAlarmAt - _dateTimeService.Now).TotalSeconds;
        if (seconds > 0)
        {
            Seconds = seconds;
            return;
        }

        _timerService.Stop();
        await HandleOnFinishedEvent(Status);
    }

    // TODO: Does awaiting async calls delay the work of the timer?
    // TODO: Demand user input before starting another interval
    private async Task HandleOnFinishedEvent(TimerStatusEnum status)
    {
        switch (status)
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