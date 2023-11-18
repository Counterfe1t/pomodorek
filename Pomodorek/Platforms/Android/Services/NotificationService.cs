using Android.App;
using Pomodorek.Platforms.Android.Helpers;

namespace Pomodorek.Services;

public class NotificationService : INotificationService
{
    private readonly NotificationManager _notificationManager;

    public NotificationService()
    {
        _notificationManager = NotificationManager.FromContext(Android.App.Application.Context);
    }

    public async Task DisplayNotificationAsync(Models.Notification notification) =>
        await Task.Run(() => {
            _notificationManager.Notify(notification.Id, NotificationHelper.BuildNotification(notification));
        });
}