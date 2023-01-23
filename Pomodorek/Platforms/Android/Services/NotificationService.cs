using System.Diagnostics;

namespace Pomodorek.Services;

public class NotificationService : INotificationService
{
    public async Task DisplayNotification(string message)
        => Debug.WriteLine("Android: " + message);
}
