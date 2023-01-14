using CommunityToolkit.Maui.Alerts;
using System.Diagnostics;

namespace Pomodorek.Services
{
    public static partial class NotificationService
    {
        static partial void DisplayNotificationHandler(string message)
        {
            Debug.WriteLine("Windows: " + message);
            Toast.Make(message).Show();
        }
    }
}
