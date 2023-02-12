using Pomodorek.Models;
using Pomodorek.Resources.Constants;
using Pomodorek.Resources.Enums;
using Pomodorek.Services;
using System.Windows.Input;

namespace Pomodorek.ViewModels;

public class MainPageViewModel : BaseViewModel
{
#if ANDROID
    private readonly IForegroundService _foregroundService;
#endif
    private readonly ITimerService _timerService;
    private readonly INotificationService _notificationService;
    private readonly ISettingsService _settingsService;
    private readonly IConfigurationService _configurationService;

    private AppSettings AppSettings => _configurationService.GetAppSettings();

    // TODO: Create sound service
    //private IDeviceSoundService _soundService;

    #region Properties

    private int _seconds = 0;
    public int Seconds
    {
        get => _seconds;
        set => SetProperty(ref _seconds, value);
    }

    private TimerStatusEnum _status = TimerStatusEnum.Stopped;
    public TimerStatusEnum Status
    {
        get => _status;
        set => SetProperty(ref _status, value);
    }

    // TODO: Change property so it represents state of the timer (running, paused, stopped)
    private bool _isRunning = false;
    public bool IsRunning
    {
        get => _isRunning;
        set => SetProperty(ref _isRunning, value);
    }

    private short _sessionLength = 2;
    public short SessionLength
    {
        get => _sessionLength;
        set => SetProperty(ref _sessionLength, value);
    }

    private short _sessionsElapsed;
    public short SessionsElapsed
    {
        get => _sessionsElapsed;
        set => SetProperty(ref _sessionsElapsed, value);
    }

    #endregion

    public ICommand StartCommand { get; private set; }
    public ICommand StopCommand { get; private set; }

    public MainPageViewModel(
#if ANDROID
        IForegroundService foregroundService,
#endif
        ITimerService timerService,
        INotificationService notificationService,
        ISettingsService settingsService,
        IConfigurationService configurationService)
    {
#if ANDROID
        _foregroundService = foregroundService;
#endif
        Title = Constants.PageTitles.Pomodorek;
        _timerService = timerService;
        _notificationService = notificationService;
        _settingsService = settingsService;
        _configurationService = configurationService;
        StopCommand = new Command(StopSession);
        StartCommand = new Command(StartSession);
    }

    private void StartSession()
    {
        // TODO: Handle pausing timer
        if (IsRunning)
            return;
        IsRunning = true;
        SetTimer(
            TimerStatusEnum.Focus,
            _settingsService.Get(
                Constants.FocusLengthInMin,
                AppSettings.DefaultFocusLengthInMin) * Constants.OneMinuteInSec);
        SessionsElapsed = 0;
        //PlayStartSessionSound();
#if ANDROID
        _foregroundService.Start();
#endif
    }

    private void StopSession()
    {
        _timerService.Stop();
        Seconds = 0;
        IsRunning = false;
        Status = TimerStatusEnum.Stopped;
#if ANDROID
        _foregroundService.Stop();
#endif
    }

    #region Services
    //private void PlayStartSessionSound()
    //{
    //    _soundService = DependencyService.Get<IDeviceSoundService>();
    //    using (_soundService as IDisposable)
    //    {
    //        _soundService.PlayStartSound();
    //    }
    //}

    public async Task DisplayNotification(string message) =>
        await _notificationService.DisplayNotification(message);

    //private void DisplaySessionOverNotification()
    //{
    //    _notificationService = DependencyService.Get<IDeviceNotificationService>();
    //    using (_notificationService as IDisposable)
    //    {
    //        _notificationService.DisplaySessionOverNotification(Constants.SessionOverNotificationMessage);
    //    }
    //}
    #endregion

    private void SetTimer(TimerStatusEnum status, int time)
    {
        Status = status;
        Seconds = time;
        _timerService.Start(HandleOnTickEvent);
    }

    private void HandleOnTickEvent()
    {
        if (Seconds == 0)
        {
            _timerService.Stop();
            HandleOnFinishedEvent();
            return;
        }
        Seconds--;
    }

    // TODO: Figure out a way to get rid of these warnings
    private void HandleOnFinishedEvent()
    {
        switch (Status)
        {
            case TimerStatusEnum.Focus:
                if (++SessionsElapsed >= SessionLength)
                {
                    StopCommand.Execute(null);
                    //DisplaySessionOverNotification();
                    DisplayNotification(Constants.NotificationMessages.SessionOver);
                    break;
                }

                if (SessionsElapsed % 4 == 0)
                {
                    SetTimer(
                        TimerStatusEnum.LongRest,
                        _settingsService.Get(
                            Constants.LongRestLengthInMin,
                            AppSettings.DefaultFocusLengthInMin) * 60);
                    DisplayNotification(Constants.NotificationMessages.LongRest);
                    break;
                }

                SetTimer(
                    TimerStatusEnum.ShortRest,
                    _settingsService.Get(
                        Constants.ShortRestLengthInMin,
                        AppSettings.DefaultShortRestLengthInMin) * 60);
                DisplayNotification(Constants.NotificationMessages.ShortRest);
                break;
            case TimerStatusEnum.ShortRest:
            case TimerStatusEnum.LongRest:
                SetTimer(
                    TimerStatusEnum.Focus,
                    _settingsService.Get(
                        Constants.FocusLengthInMin,
                        AppSettings.DefaultFocusLengthInMin) * 60);
                DisplayNotification(Constants.NotificationMessages.Focus);
                break;
            case TimerStatusEnum.Stopped:
            default:
                break;
        }
    }
}