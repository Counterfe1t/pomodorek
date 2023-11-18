using CommunityToolkit.Maui.Alerts;

namespace Pomodorek.Services;

public class NotificationService : INotificationService
{
    // TODO: If possible customize windows notification
    public async Task DisplayNotificationAsync(Notification notification)
    {
        await Toast.Make(notification.Content).Show();
    }
}