using Android.App;
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

    public static Android.App.Notification BuildNotification(Models.Notification notification)
    {
        using var builder = new NotificationCompat.Builder(Android.App.Application.Context, NotificaionChannelId)
            .SetPriority(NotificationCompat.PriorityMax)
            .SetChannelId(NotificaionChannelId)
            .SetContentTitle(notification.Title)
            .SetContentText(notification.Content)
            .SetSmallIcon(Resource.Drawable.ic_clock_black_24dp)
            .SetOngoing(notification.IsOngoing)
            .SetOnlyAlertOnce(notification.IsOngoing)
            .SetProgress(notification.MaxProgress, notification.CurrentProgress, false);

        return builder.Build();
    }

    public async Task DisplayNotificationAsync(Models.Notification notification) =>
        await Task.Run(() => {
            _notificationManager.Notify(notification.Id, BuildNotification(notification));
        });
}