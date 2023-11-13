using CommunityToolkit.Maui.Alerts;

namespace Pomodorek.Services;

public class NotificationService : INotificationService
{
    public void DisplayNotification(string message) => Toast.Make(message).Show();
}