using Pomodorek.ViewModels;
using Xamarin.Forms;
using System;

namespace Pomodorek.Views
{
    public partial class MainPage : ContentPage
    {
        public TimerViewModel ViewModel { get; set; }

        public MainPage()
        {
            InitializeComponent();
            ViewModel = new TimerViewModel();
            BindingContext = ViewModel;
        }

        #region Events

        private void OnStartButtonClicked(object sender, EventArgs e)
        {
            ViewModel.StartSession();
        }

        private void OnStopButtonClicked(object sender, EventArgs e)
        {
            ViewModel.StopSession();
        }

        #endregion
    }
}
