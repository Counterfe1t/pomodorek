using System.Diagnostics;

namespace Pomodorek.Services;

public class NotificationService : INotificationService
{
    // todo: display push notification
    public async Task DisplayNotification(string message)
        => Debug.WriteLine("Android: " + message);
}
