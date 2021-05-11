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

        #endregion

        public MainPageViewModel() {
            ApplicationTimer = new ApplicationTimer(this);
        }

        public void StartSession() {
            ApplicationTimer.StartSession();
        }

        public void PauseOrResumeSession() {
            //ApplicationTimer.PauseOrResumeSession();
        }

        public void StopSession() {
            ApplicationTimer.StopSession();
        }

        public void DisplayAlert(string title, string message, string cancel) {
            Application.Current.MainPage.DisplayAlert(title, message, cancel);
        }
    }
}
