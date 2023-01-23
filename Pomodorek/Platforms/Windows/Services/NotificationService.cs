using CommunityToolkit.Maui.Alerts;

namespace Pomodorek.Services;

public class NotificationService : INotificationService
{
    public async Task DisplayNotification(string message)
        => await Toast.Make(message).Show();
}