using Pomodorek.Models;

namespace Pomodorek.ViewModels {
    public class MainPageViewModel : BaseViewModel {

        private int seconds = 0;
        public int SecondsDisplayField {
            get => seconds;
            set {
                seconds = value;
                OnPropertyChanged();
            }
        }

        private int minutes = 0;
        public int MinutesDisplayField {
            get => minutes;
            set {
                minutes = value;
                OnPropertyChanged();
            }
        }

        private TimerModeEnum mode;
        public TimerModeEnum ModeDisplayField {
            get => mode;
            set {
                mode = value;
                OnPropertyChanged();
            }
        }

        private int sessionLength = 1;
        public int SessionLengthDisplayField {
            get => sessionLength;
            set {
                sessionLength = value;
                OnPropertyChanged();
            }
        }

        private int cyclesElapsed;
        public int CyclesElapsedDisplayField {
            get => cyclesElapsed;
            set {
                cyclesElapsed = value;
                OnPropertyChanged();
            }
        }
    }
}
