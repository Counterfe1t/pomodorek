using Pomodorek.Resources.Constants;
using Pomodorek.Resources.Enums;
using Pomodorek.Services;

namespace Pomodorek.ViewModels;

public class MainPageViewModel : BaseViewModel
{
#if ANDROID
    private readonly IForegroundService _foregroundService;
#endif
    private readonly ITimerService _timerService;
    private readonly INotificationService _notificationService;
    // TODO: Create sound service
    //private IDeviceSoundService _soundService;

    #region Properties

    private short _seconds = 0;
    public short Seconds
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

    public MainPageViewModel(
#if ANDROID
        IForegroundService foregroundService,
#endif
        ITimerService timer,
        INotificationService notificationService)
    {
#if ANDROID
        _foregroundService = foregroundService;
#endif
        _timerService = timer;
        _notificationService = notificationService;
        Title = "Pomodorek";
    }

    public void StartSession()
    {
        // TODO: Handle pausing timer
        if (IsRunning)
            return;
        IsRunning = true;
        SetTimer(Constants.FocusLength, TimerStatusEnum.Focus);
        SessionsElapsed = 0;
        //PlayStartSessionSound();
#if ANDROID
        _foregroundService.Start();
#endif
    }

    public void StopSession()
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

    private void SetTimer(short time, TimerStatusEnum mode)
    {
        Status = mode;
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
                    StopSession();
                    //DisplaySessionOverNotification();
                    DisplayNotification(Constants.SessionOverNotificationMessage);
                    break;
                }

                if (SessionsElapsed % 4 == 0)
                {
                    SetTimer(Constants.LongRestLength, TimerStatusEnum.LongRest);
                    DisplayNotification(Constants.LongRestNotificationMessage);
                    break;
                }

                SetTimer(Constants.ShortRestLength, TimerStatusEnum.ShortRest);
                DisplayNotification(Constants.ShortRestNotificationMessage);
                break;
            case TimerStatusEnum.ShortRest:
            case TimerStatusEnum.LongRest:
                SetTimer(Constants.FocusLength, TimerStatusEnum.Focus);
                DisplayNotification(Constants.FocusNotificationMessage);
                break;
            case TimerStatusEnum.Stopped:
            default:
                break;
        }
    }
}