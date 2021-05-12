using Microsoft.Toolkit.Uwp.Notifications;
using Pomodorek.Services;
using Pomodorek.UWP.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(WindowsNotificationService))]
namespace Pomodorek.UWP.Services {
    public class WindowsNotificationService : IDeviceNotificationService {

        public void DisplayNotification(string message) {
            new ToastContentBuilder()
                .AddText(message)
                .Show();
        }
    }
}
