using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using static Android.OS.PowerManager;

namespace Pomodorek.Services;

[Service]
public class ForegroundService : Service, IForegroundService
{
    private const string StartServiceAction = nameof(StartServiceAction);
    private const string StopServiceAction = nameof(StopServiceAction);

    public override IBinder OnBind(Intent intent)
    {
        throw new NotImplementedException();
    }

    [return: GeneratedEnum]
    public override StartCommandResult OnStartCommand(
        Intent intent,
        [GeneratedEnum] StartCommandFlags flags,
        int startId)
    {
        // TODO: Figure out a way to send notifications and update UI without using wake lock
        WakeLock wakeLock = null;
        if (intent.Action == StartServiceAction)
        {
            RegisterNotification();
            PowerManager powerManager = (PowerManager)GetSystemService(PowerService);
            wakeLock = powerManager.NewWakeLock(WakeLockFlags.Partial, "MyApp::MyWakelockTag");
            wakeLock.Acquire();
        }
        else if (intent.Action == StopServiceAction)
        {
            wakeLock?.Release();
            StopForeground(true);
            StopSelfResult(startId);
        }

        return StartCommandResult.NotSticky;
    }

    public void Start()
    {
        var startService = new Intent(MainActivity.ActivityCurrent, typeof(ForegroundService));
        startService.SetAction(StartServiceAction);
        MainActivity.ActivityCurrent.StartService(startService);
    }

    public void Stop()
    {
        var stopIntent = new Intent(MainActivity.ActivityCurrent, Class);
        stopIntent.SetAction(StopServiceAction);
        MainActivity.ActivityCurrent.StartService(stopIntent);
    }

    private void RegisterNotification()
    {
        var channel = new NotificationChannel(
            nameof(ForegroundService),
            nameof(ForegroundService),
            NotificationImportance.Max);

        var manager = (NotificationManager)MainActivity.ActivityCurrent.GetSystemService(NotificationService);
        manager.CreateNotificationChannel(channel);

        var notification = new Notification.Builder(this, nameof(ForegroundService))
            .SetOngoing(true)
            .Build();

        StartForeground(100, notification);
    }
}
