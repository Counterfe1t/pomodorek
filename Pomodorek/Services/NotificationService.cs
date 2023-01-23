namespace Pomodorek.Services;

public static partial class NotificationService
{
    public static void DisplayNotification(string message) => DisplayNotificationHandler(message);

    static partial void DisplayNotificationHandler(string message);
}