using Pomodorek.Logic;
using Pomodorek.Models;
using Xamarin.Forms;

namespace Pomodorek.ViewModels {
    public class MainPageViewModel : BaseViewModel {

        private readonly ApplicationTimer ApplicationTimer;

        #region Properties

        public int SecondsViewModel {
            get => ApplicationTimer.Seconds;
            set {
                ApplicationTimer.Seconds = value;
                OnPropertyChanged();
            }
        }

        public int MinutesViewModel {
            get => ApplicationTimer.Minutes;
            set {
                ApplicationTimer.Minutes = value;
                OnPropertyChanged();
            }
        }

        public TimerModeEnum ModeViewModel {
            get => ApplicationTimer.Mode;
            set {
                ApplicationTimer.Mode = value;
                OnPropertyChanged();
            }
        }

        public int SessionLengthViewModel {
            get => ApplicationTimer.SessionLength;
            set {
                ApplicationTimer.SessionLength = value;
                OnPropertyChanged();
            }
        }

        public int CyclesElapsedViewModel {
            get => ApplicationTimer.CyclesElapsed;
            set {
                ApplicationTimer.CyclesElapsed = value;
                OnPropertyChanged();
            }
        }

        public bool IsEnabledViewModel {
            get => ApplicationTimer.IsEnabled;
            set {
                ApplicationTimer.IsEnabled = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public MainPageViewModel() {
            ApplicationTimer = new ApplicationTimer(this);
        }

        public void StartOrPauseTimer() {
            ApplicationTimer.StartOrPauseTimer();
        }

        public void ResetTimer() {
            ApplicationTimer.ResetTimer();
        }

        public void DisplayAlert(string title, string message, string cancel) {
            Application.Current.MainPage.DisplayAlert(title, message, cancel);
        }
    }
}
