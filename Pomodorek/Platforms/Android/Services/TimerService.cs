using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Pomodorek.Platforms.Android.Helpers;
using Pomodorek.Platforms.Android.Receivers;

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
        _notificationService = GetService<INotificationService>();
        _settingsService = GetService<ISettingsService>();
        _dateTimeService = GetService<IDateTimeService>();

        _token = new CancellationTokenSource();
    }

    public void Start(Action callback)
    {
        StartForegroundService();
        SetAlarm();

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

    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
    private void SetAlarm()
    {
        var serializedNotification = _settingsService.Get(nameof(Models.Notification), string.Empty);
        var notification = JsonSerializer.Deserialize<Models.Notification>(serializedNotification);

        var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var triggerAlarmAtMs = (long)notification.TriggerAlarmAt.ToUniversalTime().Subtract(unixEpoch).TotalMilliseconds;

        var intent = new Intent(MainActivity.ActivityCurrent, typeof(AlarmReceiver));
        var pendingIntent = PendingIntent.GetBroadcast(MainActivity.ActivityCurrent, 1, intent, PendingIntentFlags.Immutable);

        var alarmManager = (AlarmManager)MainActivity.ActivityCurrent.GetSystemService(AlarmService);

        if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
        {
            alarmManager.SetExactAndAllowWhileIdle(AlarmType.RtcWakeup, triggerAlarmAtMs, pendingIntent);
        }
        else if (Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat)
        {
            alarmManager.SetExact(AlarmType.RtcWakeup, triggerAlarmAtMs, pendingIntent);
        }
    }

    private void CancelAlarm()
    {
        var intent = new Intent(MainActivity.ActivityCurrent, typeof(AlarmReceiver));
        var pendingIntent = PendingIntent.GetBroadcast(MainActivity.ActivityCurrent, 1, intent, PendingIntentFlags.Immutable);

        var alarmManager = (AlarmManager)MainActivity.ActivityCurrent.GetSystemService(AlarmService);
        alarmManager.Cancel(pendingIntent);
    }

    public void Stop(bool isCancelled)
    {
        if (isCancelled)
            CancelAlarm();

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

    private TService GetService<TService>() => (TService)MauiApplication.Current.Services.GetService(typeof(TService));

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

        Task.Run(async () =>
        {
            var token = _token;
            var secondsRemaining = (int)notification.TriggerAlarmAt.Subtract(_dateTimeService.Now).TotalSeconds;

            while (secondsRemaining > 0 && !token.IsCancellationRequested)
            {
                await Task.Delay(Constants.OneSecondInMs);

                notification.Content = TimeConverter.FormatTime(secondsRemaining);
                notification.CurrentProgress = secondsRemaining;
                StartForeground(notification.Id, NotificationHelper.BuildNotification(notification));

                secondsRemaining = (int)notification.TriggerAlarmAt.Subtract(_dateTimeService.Now).TotalSeconds;
            }
        });
    }
}