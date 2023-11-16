using CommunityToolkit.Maui.Alerts;

namespace Pomodorek.Services;

public class NotificationService : INotificationService
{
    // TODO: If possible customize windows notification
    public async Task DisplayNotificationAsync(NotificationDto notification)
    {
        await Toast.Make(notification.Content).Show();
    }
}