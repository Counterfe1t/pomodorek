using Pomodorek.Services;
using Pomodorek.UWP.Services;
using System;
using Windows.ApplicationModel.ExtendedExecution;
using Xamarin.Forms;

namespace Pomodorek.UWP
{
    public sealed partial class MainPage
    {
        private ExtendedExecutionSession _session;

        public MainPage()
        {
            InitializeComponent();
            BeginExtendedExecution();
            LoadApplication(new Pomodorek.App());
            DependencyService.Register<IDeviceNotificationService, WindowsNotificationService>();
            DependencyService.Register<IDeviceSoundService, WindowsSoundService>();
        }

        // todo: figure out why this is necessary
        private async void BeginExtendedExecution()
        {
            var newSession = new ExtendedExecutionSession
            {
                Reason = ExtendedExecutionReason.Unspecified
            };
            _session = newSession;
            _ = await _session.RequestExtensionAsync();
        }
    }
}
