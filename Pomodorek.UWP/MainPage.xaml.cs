using System;
using Windows.ApplicationModel.ExtendedExecution;

namespace Pomodorek.UWP {
    public sealed partial class MainPage {

        private ExtendedExecutionSession session = null;

        public MainPage() {
            InitializeComponent();
            BeginExtendedExecution();
            LoadApplication(new Pomodorek.App());
        }

        private async void BeginExtendedExecution() {
            var newSession = new ExtendedExecutionSession {
                Reason = ExtendedExecutionReason.Unspecified,
                Description = ""
            };
            session = newSession;
            _ = await session.RequestExtensionAsync();
        }
    }
}
