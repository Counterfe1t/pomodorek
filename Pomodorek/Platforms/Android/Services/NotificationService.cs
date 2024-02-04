using Android.App;
using Android.Content;
using AndroidX.Core.App;

namespace Pomodorek.Services;

public class NotificationService : INotificationService
{
    private readonly NotificationManager _notificationManager;

    public static readonly string NotificaionChannelId = "General";

    public NotificationService()
    {
        _notificationManager = NotificationManager.FromContext(Android.App.Application.Context);
    }

    public static Android.App.Notification BuildNotification(Models.NotificationModel notification)
    {
        var intent = new Intent(Android.App.Application.Context, typeof(MainActivity));
        var contentIntent = PendingIntent.GetActivity(Android.App.Application.Context, 0, intent, PendingIntentFlags.Immutable);

        using var builder = new NotificationCompat.Builder(Android.App.Application.Context, NotificaionChannelId)
            .SetPriority(NotificationCompat.PriorityMax)
            .SetChannelId(NotificaionChannelId)
            .SetContentTitle(notification.Title)
            .SetContentText(notification.Content)
            .SetSmallIcon(Resource.Drawable.ic_clock_black_24dp)
            .SetOngoing(notification.IsOngoing)
            .SetOnlyAlertOnce(notification.IsOngoing)
            .SetProgress(notification.MaxProgress, notification.CurrentProgress, false)
            .SetContentIntent(contentIntent);

        return builder.Build();
    }

    public async Task DisplayNotificationAsync(Models.NotificationModel notification) =>
        await Task.Run(() =>
        {
            _notificationManager.Notify(notification.Id, BuildNotification(notification));
        });
}