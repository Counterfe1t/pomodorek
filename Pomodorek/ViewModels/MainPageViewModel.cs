using Pomodorek.Models;

namespace Pomodorek.ViewModels {
    public class MainPageViewModel : BaseViewModel {

        public int Seconds { get; set; }

        public int Minutes { get; set; }

        public TimerModeEnum Mode { get; set; }

        public int SessionLength { get; set; } = 1;

        public int SecondsDisplayField {
            get {
                return Seconds;
            }
            set {
                Seconds = value;
                OnPropertyChanged();
            }
        }

        public int MinutesDisplayField {
            get {
                return Minutes;
            }
            set {
                Minutes = value;
                OnPropertyChanged();
            }
        }

        public TimerModeEnum ModeDisplayField {
            get {
                return Mode;
            }
            set {
                Mode = value;
                OnPropertyChanged();
            }
        }

        public int SessionLengthDisplayField {
            get {
                return SessionLength;
            }
            set {
                SessionLength = value;
                OnPropertyChanged();
            }
        }
    }
}
