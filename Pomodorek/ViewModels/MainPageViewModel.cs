namespace Pomodorek.ViewModels;

public partial class MainPageViewModel : BaseViewModel
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsRunning))]
    private TimerStateEnum _state = TimerStateEnum.Stopped;

    [ObservableProperty]
    private int _seconds;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Alarm))]
    private DateTime _triggerAlarmAt;

    [ObservableProperty]
    private Session _session;

    public bool IsRunning => State == TimerStateEnum.Running;

    public string Alarm => $"Alarm: {TriggerAlarmAt.ToLocalTime():HH:mm}";

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

        _settingsService.Set(Constants.Settings.IntervalsCount, Session.IntervalsCount);
        Task.Run(async () => await PlaySound(Constants.Sounds.SessionStart));

        SetTimer(Session.CurrentInterval);
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
        State = TimerStateEnum.Stopped;
        Seconds = GetIntervalLengthInMin(IntervalEnum.Work) * Constants.OneMinuteInSec;
        Session = new() { CurrentInterval = IntervalEnum.Work };
    }

    private void SetTimer(IntervalEnum interval)
    {
        State = TimerStateEnum.Running;
        Session.CurrentInterval = interval;

        var durationInMin = GetIntervalLengthInMin(interval);
        Seconds = durationInMin * Constants.OneMinuteInSec;
        TriggerAlarmAt = _dateTimeService.Now.AddMinutes(durationInMin).AddSeconds(1);

        if (DeviceInfo.Platform == DevicePlatform.Android)
        {
            _settingsService.Set(nameof(Notification), JsonSerializer.Serialize(new Notification
            {
                Id = 2137,
                Title = interval.ToString(),
                TriggerAlarmAt = TriggerAlarmAt,
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
        Initialize();
    }

    private async Task HandleOnTickEvent()
    {
        var seconds = (int)(TriggerAlarmAt - _dateTimeService.Now).TotalSeconds;
        if (seconds > 0)
        {
            Seconds = seconds;
            return;
        }

        _timerService.Stop();
        State = TimerStateEnum.Stopped;

        await HandleOnFinishedEvent(Session.CurrentInterval);

        Seconds = GetIntervalLengthInMin(Session.CurrentInterval) * Constants.OneMinuteInSec;
    }

    // TODO: Does awaiting async calls delay the work of the timer?
    // TODO: Demand user input before starting another interval
    private async Task HandleOnFinishedEvent(IntervalEnum interval)
    {
        Session.IntervalsCount++;

        switch (interval)
        {
            case IntervalEnum.Work:
                Session.WorkIntervalsCount++;

                if (Session.IsLongRest)
                {
                    Session.CurrentInterval = IntervalEnum.LongRest;
                    await DisplayNotification(Constants.Messages.LongRest);
                    break;
                }

                Session.CurrentInterval = IntervalEnum.ShortRest;
                await DisplayNotification(Constants.Messages.ShortRest);
                break;
            case IntervalEnum.ShortRest:
                Session.ShortRestIntervalsCount++;
                Session.CurrentInterval = IntervalEnum.Work;
                await DisplayNotification(Constants.Messages.Focus);
                break;
            case IntervalEnum.LongRest:
                Session.LongRestIntervalsCount++;
                Session.CurrentInterval = IntervalEnum.Work;
                await DisplayNotification(Constants.Messages.Focus);
                break;
            default:
                break;
        }

        _ = PlaySound(Constants.Sounds.SessionOver);
    }

    private int GetIntervalLengthInMin(IntervalEnum interval) =>
        interval switch
        {
            IntervalEnum.Work =>
                _settingsService.Get(Constants.Settings.WorkLengthInMin, AppSettings.DefaultWorkLengthInMin),
            IntervalEnum.ShortRest =>
                _settingsService.Get(Constants.Settings.ShortRestLengthInMin, AppSettings.DefaultShortRestLengthInMin),
            IntervalEnum.LongRest =>
                _settingsService.Get(Constants.Settings.LongRestLengthInMin, AppSettings.DefaultLongRestLengthInMin),
            _ => 0
        };
}