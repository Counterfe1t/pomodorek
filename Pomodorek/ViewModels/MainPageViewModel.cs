namespace Pomodorek.ViewModels;

public partial class MainPageViewModel : BaseViewModel
{
    private DateTime _triggerAlarmAt;

    [ObservableProperty]
    private int _seconds;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsRunning))]
    private TimerStateEnum _timerState = TimerStateEnum.Stopped;

    [ObservableProperty]
    private PomodorekSession _session;

    public bool IsRunning => TimerState == TimerStateEnum.Running;

    private readonly ITimerService _timerService;
    private readonly INotificationService _notificationService;
    private readonly ISettingsService _settingsService;
    private readonly IConfigurationService _configurationService;
    private readonly ISoundService _soundService;
    private readonly IDateTimeService _dateTimeService;
    private readonly IPermissionsService _permissionsService;

    private AppSettings AppSettings => _configurationService.GetAppSettings();

    public MainPageViewModel(
        ITimerService timerService,
        INotificationService notificationService,
        ISettingsService settingsService,
        IConfigurationService configurationService,
        ISoundService soundService,
        IDateTimeService dateTimeService,
        IPermissionsService permissionsService)
        : base(Constants.Pages.Pomodorek)
    {
        _timerService = timerService;
        _notificationService = notificationService;
        _settingsService = settingsService;
        _configurationService = configurationService;
        _soundService = soundService;
        _dateTimeService = dateTimeService;
        _permissionsService = permissionsService;

        Initialize();
    }

    // TODO: Handle pausing and resuming timer
    [RelayCommand]
    public void Start()
    {
        if (IsRunning)
            return;

        // TODO: Delegate this to session handler
        Session.IntervalsElapsed = 0;
        _settingsService.Set(Constants.Settings.IntervalsCount, Session.IntervalsCount);
        Task.Run(async () => await PlaySound(Constants.Sounds.SessionStart));

        SetTimer(IntervalEnum.Work);
    }

    // TODO: Show simple session summary
    [RelayCommand]
    public void Stop() => StopTimer();

    public async Task DisplayNotification(string message)
    {
        if (DeviceInfo.Platform == DevicePlatform.WinUI)
        {
            await _notificationService.DisplayNotificationAsync(new Notification
            {
                Content = message,
            });
        }
    }

    public async Task PlaySound(string fileName) => await _soundService.PlaySoundAsync(fileName);

    public async Task CheckAndRequestPermissionsAsync() =>
        await _permissionsService.CheckAndRequestPermissionsAsync();

    private void Initialize()
    {
        TimerState = TimerStateEnum.Stopped;
        Seconds = GetDurationInMin(IntervalEnum.Work) * Constants.OneMinuteInSec;
        Session = new()
        {
            Interval = IntervalEnum.Work,
            IntervalsCount = _settingsService.Get(Constants.Settings.IntervalsCount, AppSettings.DefaultSessionsCount),
        };
    }

    private void SetTimer(IntervalEnum interval)
    {
        Session.Interval = interval;

        var durationInMin = GetDurationInMin(interval);
        TimerState = TimerStateEnum.Running;
        Seconds = durationInMin * Constants.OneMinuteInSec;
        _triggerAlarmAt = _dateTimeService.Now.AddMinutes(GetDurationInMin(interval)).AddSeconds(1);

        if (DeviceInfo.Platform == DevicePlatform.Android)
        {
            _settingsService.Set(nameof(Notification), JsonSerializer.Serialize(new Notification
            {
                Id = 2137,
                Title = interval.ToString(),
                TriggerAlarmAt = _triggerAlarmAt,
                MaxProgress = Seconds,
                IsOngoing = true,
                OnlyAlertOnce = true
            }));
        }

        _timerService.Start(async () => await HandleOnTickEvent());
    }

    private void StopTimer()
    {
        _timerService.Stop();
        TimerState = TimerStateEnum.Stopped;
        Session.Interval = IntervalEnum.Work;
        Seconds = GetDurationInMin(IntervalEnum.Work) * Constants.OneMinuteInSec;
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
        await HandleOnFinishedEvent(Session.Interval);
    }

    // TODO: Does awaiting async calls delay the work of the timer?
    // TODO: Demand user input before starting another interval
    private async Task HandleOnFinishedEvent(IntervalEnum interval)
    {
        switch (interval)
        {
            case IntervalEnum.Work:
                Session.IntervalsElapsed++;

                if (Session.IsFinished)
                {
                    StopCommand.Execute(null);
                    await DisplayNotification(Constants.Messages.SessionOver);
                    await PlaySound(Constants.Sounds.SessionOver);
                    break;
                }

                if (Session.IsLongRest)
                {
                    await DisplayNotification(Constants.Messages.LongRest);
                    SetTimer(IntervalEnum.LongRest);
                    break;
                }

                await DisplayNotification(Constants.Messages.ShortRest);
                SetTimer(IntervalEnum.ShortRest);

                break;
            case IntervalEnum.ShortRest:
            case IntervalEnum.LongRest:
                await DisplayNotification(Constants.Messages.Focus);
                SetTimer(IntervalEnum.Work);
                break;
            default:
                break;
        }
    }

    private int GetDurationInMin(IntervalEnum interval) =>
        interval switch
        {
            IntervalEnum.Work =>
                _settingsService.Get(Constants.Settings.WorkLengthInMin, AppSettings.DefaultFocusLengthInMin),
            IntervalEnum.ShortRest =>
                _settingsService.Get(Constants.Settings.ShortRestLengthInMin, AppSettings.DefaultShortRestLengthInMin),
            IntervalEnum.LongRest =>
                _settingsService.Get(Constants.Settings.LongRestLengthInMin, AppSettings.DefaultLongRestLengthInMin),
            _ => 0
        };
}