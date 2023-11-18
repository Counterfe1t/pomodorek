using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Pomodorek.Platforms.Android.Helpers;

namespace Pomodorek.Services;

[Service]
public class TimerService : Service, ITimerService
{
    private CancellationTokenSource _token;

    private readonly INotificationService _notificationService;
    private readonly ISettingsService _settingsService;
    private readonly IDateTimeService _dateTimeService;

    public TimerService()
    {
        _notificationService = (INotificationService)MauiApplication.Current.Services.GetService(typeof(INotificationService));
        _settingsService = (ISettingsService)MauiApplication.Current.Services.GetService(typeof(ISettingsService));
        _dateTimeService = (IDateTimeService)MauiApplication.Current.Services.GetService(typeof(IDateTimeService));
     
        _token = new CancellationTokenSource();
    }

    public void Start(Action callback)
    {
        StartForegroundService();

        var token = _token;
        Task.Run(async () =>
        {
            while (!token.IsCancellationRequested)
            {
                await Task.Delay(Constants.OneSecondInMs);

                if (token.IsCancellationRequested)
                    return;

                callback.Invoke();
            }
        });
    }

    public void Stop()
    {
        StopForegroundService();

        Interlocked.Exchange(ref _token, new CancellationTokenSource()).Cancel();
    }

    public override IBinder OnBind(Intent intent)
    {
        throw new NotImplementedException();
    }

    [return: GeneratedEnum]
    public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
    {
        DisplayProgressNotification();

        return StartCommandResult.NotSticky;
    }

    private void StartForegroundService()
    {
        var startIntent = new Intent(MainActivity.ActivityCurrent, typeof(TimerService));
        MainActivity.ActivityCurrent.StartService(startIntent);
    }

    private void StopForegroundService()
    {
        var stopIntent = new Intent(MainActivity.ActivityCurrent, typeof(TimerService));
        MainActivity.ActivityCurrent.StopService(stopIntent);
    }

    private void DisplayProgressNotification()
    {
        var serializedNotification = _settingsService.Get(nameof(Models.Notification), string.Empty);
        var notification = JsonSerializer.Deserialize<Models.Notification>(serializedNotification);

        notification.Content = "Timer is running";
        notification.IsOngoing = true;
        notification.OnlyAlertOnce = true;
        StartForeground(notification.Id, NotificationHelper.BuildNotification(notification));

        // update ongoing pregress notification every second
        Task.Run(async () =>
        {
            var token = _token;
            var seconds = (int)(notification.TriggerAlarmAt - _dateTimeService.Now).TotalSeconds;

            while (seconds >= 0 && !token.IsCancellationRequested)
            {
                await Task.Delay(1000);

                notification.Content = TimeConverter.FormatTime(seconds);
                notification.CurrentProgress = seconds;
                StartForeground(notification.Id, NotificationHelper.BuildNotification(notification));

                seconds = (int)(notification.TriggerAlarmAt - _dateTimeService.Now).TotalSeconds;
            }

            StopForegroundService();

            if (token.IsCancellationRequested)
                return;

            notification.Title = "Alarm";
            notification.Content = "Session ended";
            notification.OnlyAlertOnce = false;
            notification.IsOngoing = false;
            notification.MaxProgress = 0;
            notification.CurrentProgress = 0;

            await _notificationService.DisplayNotificationAsync(notification);
        });
    }
}