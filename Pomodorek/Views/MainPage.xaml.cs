using Pomodorek.Impl;
using Pomodorek.ViewModels;
using Xamarin.Forms;

namespace Pomodorek {
    public partial class MainPage : ContentPage {

        public MainPageViewModel ViewModel { get; set; }

        public ApplicationTimer ApplicationTimer { get; set; }

        public MainPage() {
            InitializeComponent();

            ViewModel = new MainPageViewModel();
            BindingContext = ViewModel;
            ApplicationTimer = new ApplicationTimer(ViewModel);
        }

        #region Events
        private void StartButton_Clicked(object sender, System.EventArgs e) {
            var sessionLength = ViewModel.SessionLengthDisplayField;
            ApplicationTimer.StartNewSession(sessionLength);
        }

        private void PauseButton_Clicked(object sender, System.EventArgs e) {
            ApplicationTimer.PauseUnpauseTimer();
        }

        private void StopButton_Clicked(object sender, System.EventArgs e) {
            ApplicationTimer.StopTimer();
        }
        #endregion
    }
}
