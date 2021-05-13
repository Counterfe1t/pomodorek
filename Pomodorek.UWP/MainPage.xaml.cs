using Pomodorek.Services;
using Pomodorek.UWP.Services;
using System;
using Windows.ApplicationModel.ExtendedExecution;
using Xamarin.Forms;

namespace Pomodorek.UWP {
    public sealed partial class MainPage {

        private ExtendedExecutionSession session = null;

        public MainPage() {
            InitializeComponent();
            BeginExtendedExecution();

            LoadApplication(new Pomodorek.App());

            DependencyService.Register<IDeviceNotificationService, WindowsNotificationService>();
            DependencyService.Register<IDeviceSoundService, WindowsSoundService>();
        }

        private async void BeginExtendedExecution() {
            var newSession = new ExtendedExecutionSession {
                Reason = ExtendedExecutionReason.Unspecified
            };
            session = newSession;
            _ = await session.RequestExtensionAsync();
        }
    }
}
