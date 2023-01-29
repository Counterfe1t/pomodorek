using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;

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
        if (intent.Action == StartServiceAction)
            RegisterNotification();
        else if (intent.Action == StopServiceAction)
        {
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
