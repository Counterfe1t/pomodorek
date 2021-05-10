using Pomodorek.Impl;
using Pomodorek.Models;
using Xamarin.Forms;

namespace Pomodorek.ViewModels {
    public class MainPageViewModel : BaseViewModel {

        private readonly ApplicationTimer ApplicationTimer;

        #region Properties

        public int SecondsDisplayField {
            get => ApplicationTimer.Seconds;
            set {
                ApplicationTimer.Seconds = value;
                OnPropertyChanged();
            }
        }

        public int MinutesDisplayField {
            get => ApplicationTimer.Minutes;
            set {
                ApplicationTimer.Minutes = value;
                OnPropertyChanged();
            }
        }

        public TimerModeEnum ModeDisplayField {
            get => ApplicationTimer.Mode;
            set {
                ApplicationTimer.Mode = value;
                OnPropertyChanged();
            }
        }

        public int SessionLengthDisplayField {
            get => ApplicationTimer.SessionLength;
            set {
                ApplicationTimer.SessionLength = value;
                OnPropertyChanged();
            }
        }

        public int CyclesElapsedDisplayField {
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
            ApplicationTimer.StartSession(SessionLengthDisplayField);
        }

        public void PauseOrUnpauseTimer() {
            ApplicationTimer.PauseOrUnpauseTimer();
        }

        public void StopSession() {
            ApplicationTimer.StopSession();
        }

        public void DisplayAlert(string title, string message, string cancel) {
            Application.Current.MainPage.DisplayAlert(title, message, cancel);
        }
    }
}
