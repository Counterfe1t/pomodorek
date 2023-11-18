using Android.App;
using AndroidX.Core.App;
using AndroidApp = Android.App.Application;

namespace Pomodorek.Platforms.Android.Helpers;

public class NotificationHelper
{
    public static readonly string NotificaionChannelId = "General";

    public static global::Android.App.Notification BuildNotification(Models.Notification notification)
    {
        using var builder = new NotificationCompat.Builder(AndroidApp.Context, NotificaionChannelId)
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
}