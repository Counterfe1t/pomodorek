using Pomodorek.ViewModels;
using Xamarin.Forms;

namespace Pomodorek {
    public partial class MainPage : ContentPage {

        public MainPageViewModel ViewModel { get; set; }

        public MainPage() {
            InitializeComponent();
            ViewModel = new MainPageViewModel();
            BindingContext = ViewModel;
        }

        #region Events

        private void OnStartButtonClicked(object sender, System.EventArgs e) {
            ViewModel.StartOrPauseTimer();
        }

        private void OnResetButtonClicked(object sender, System.EventArgs e) {
            ViewModel.ResetTimer();
        }

        #endregion
    }
}
