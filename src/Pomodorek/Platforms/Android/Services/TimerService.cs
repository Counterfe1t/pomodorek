using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Pomodorek.Platforms.Android.Receivers;
using ServiceProvider = Pomodorek.Platforms.Android.Services.ServiceProvider;

namespace Pomodorek.Services;

[Service]
[SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
public class TimerService : Service, ITimerService
{
    private CancellationTokenSource _token;

    private readonly INotificationService _notificationService;
    private readonly ISettingsService _settingsService;
    private readonly ITimeProvider _timeProvider;

    private static MainActivity MainActivity
        => MainActivity.ActivityCurrent
        ?? throw new Exception("MainActivity is not initialized.");

    public TimerService()
    {
        _notificationService = ServiceProvider.GetService<INotificationService>();
        _settingsService = ServiceProvider.GetService<ISettingsService>();
        _timeProvider = ServiceProvider.GetService<ITimeProvider>();

        _token = new CancellationTokenSource();
    }

    public void Start(Action callback)
    {
        StartForegroundService();
        ScheduleAlarm();

        var token = _token;
        Task.Run(async () =>
        {
            while (!token.IsCancellationRequested)
            {
                await Task.Delay(Constants.OneSecondInMs);

                if (token.IsCancellationRequested)
                    return;

                callback?.Invoke();
            }
        });
    }

    public void Stop(bool isStoppedManually)
    {
        if (isStoppedManually)
            CancelAlarm();

        StopForegroundService();

        Interlocked.Exchange(ref _token, new CancellationTokenSource()).Cancel();
    }

    public override IBinder OnBind(Intent? intent)
    {
        throw new NotImplementedException();
    }

    [return: GeneratedEnum]
    public override StartCommandResult OnStartCommand(Intent? intent, [GeneratedEnum] StartCommandFlags flags, int startId)
    {
        DisplayProgressNotification();

        return StartCommandResult.NotSticky;
    }

    private void StartForegroundService()
    {
        var intent = new Intent(MainActivity, typeof(TimerService));
        MainActivity.StartService(intent);
    }

    private void StopForegroundService()
    {
        var intent = new Intent(MainActivity, typeof(TimerService));
        MainActivity.StopService(intent);
    }

    private void DisplayProgressNotification()
    {
        var serializedNotification = _settingsService.Get(nameof(NotificationModel), string.Empty);
        if (string.IsNullOrWhiteSpace(serializedNotification))
        {
            // TODO Add error logging
            return;
        }

        var notification = JsonSerializer.Deserialize<NotificationModel>(serializedNotification);
        if (notification is null)
        {
            // TODO Add error logging
            return;
        }

        notification.Content = Constants.Messages.TimerIsRunning;
        notification.IsOngoing = true;
        notification.OnlyAlertOnce = true;
        notification.TriggerAlarmAt = notification.TriggerAlarmAt.AddSeconds(-1);
        StartForeground(notification.Id, Services.NotificationService.BuildNotification(notification));

        Task.Run(async () =>
        {
            var token = _token;
            var secondsRemaining = (int)notification.TriggerAlarmAt.Subtract(_timeProvider.UtcNow).TotalSeconds;

            while (secondsRemaining > 0 && !token.IsCancellationRequested)
            {
                await Task.Delay(Constants.OneSecondInMs);

                notification.Content = TimeConverter.FormatTime(secondsRemaining);
                notification.CurrentProgress = secondsRemaining;
                StartForeground(notification.Id, Services.NotificationService.BuildNotification(notification));

                secondsRemaining = (int)notification.TriggerAlarmAt.Subtract(_timeProvider.UtcNow).TotalSeconds;
            }
        });
    }

    private void ScheduleAlarm()
    {
        var serializedNotification = _settingsService.Get(nameof(NotificationModel), string.Empty);
        if (string.IsNullOrWhiteSpace(serializedNotification))
        {
            // TODO Add error logging
            return;
        }

        var notification = JsonSerializer.Deserialize<NotificationModel>(serializedNotification);
        if (notification is null)
        {
            // TODO Add error logging
            return;
        }

        var triggerAlarmAtMs = (long)notification
            .TriggerAlarmAt
            .ToUniversalTime()
            .Subtract(_timeProvider.UnixEpoch)
            .TotalMilliseconds;

        var intent = new Intent(MainActivity, typeof(AlarmReceiver));
        var pendingIntent = PendingIntent.GetBroadcast(MainActivity, 1, intent, PendingIntentFlags.Immutable);
        var alarmManager = (AlarmManager?)MainActivity.GetSystemService(AlarmService);

        if (pendingIntent is null)
        {
            // TODO Add error logging
            return;
        }

        if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            alarmManager?.SetExactAndAllowWhileIdle(AlarmType.RtcWakeup, triggerAlarmAtMs, pendingIntent);
        else if (Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat)
            alarmManager?.SetExact(AlarmType.RtcWakeup, triggerAlarmAtMs, pendingIntent);
    }

    private void CancelAlarm()
    {
        var intent = new Intent(MainActivity, typeof(AlarmReceiver));
        var pendingIntent = PendingIntent.GetBroadcast(MainActivity, 1, intent, PendingIntentFlags.Immutable);
        if (pendingIntent is null)
        {
            // TODO Add error logging
            return;
        }

        var alarmManager = (AlarmManager?)MainActivity.GetSystemService(AlarmService);
        alarmManager?.Cancel(pendingIntent);
    }
}