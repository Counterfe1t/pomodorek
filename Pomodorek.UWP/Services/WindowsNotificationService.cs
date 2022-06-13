using Microsoft.Toolkit.Uwp.Notifications;
using Pomodorek.Services;
using Pomodorek.UWP.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(WindowsNotificationService))]
namespace Pomodorek.UWP.Services
{
    public class WindowsNotificationService : IDeviceNotificationService
    {

        public async Task DisplayNotification(string message)
        {
            await Task.Run(() =>
            {
                new ToastContentBuilder()
                    .AddText(message)
                    .Show();
            });
        }

        public async Task DisplaySessionOverNotification(string message)
        {
            await Task.Run(() =>
            {
                new ToastContentBuilder()
                    .AddText(message)
                    .AddAudio(
                        new Uri("ms-appx:///Assets/Audio/timer_stop.wav"))
                    .Show();
            });
        }
    }
}
