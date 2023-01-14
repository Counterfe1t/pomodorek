using System.Diagnostics;

namespace Pomodorek.Services
{
    public static partial class NotificationService
    {
        static partial void DisplayNotificationHandler(string message)
        {
            Debug.WriteLine("Android: " + message);
        }
    }
}
