using Pomodorek.Models;

namespace Pomodorek.ViewModels {
    public class MainPageViewModel : BaseViewModel {

        public int Seconds { get; set; }

        public int Minutes { get; set; }

        public TimerModeEnum Mode { get; set; }

        public int SessionLength { get; set; } = 1;

        public int CyclesElapsed { get; set; }

        public int SecondsDisplayField {
            get => Seconds;
            set {
                Seconds = value;
                OnPropertyChanged();
            }
        }

        public int MinutesDisplayField {
            get => Minutes;
            set {
                Minutes = value;
                OnPropertyChanged();
            }
        }

        public TimerModeEnum ModeDisplayField {
            get => Mode;
            set {
                Mode = value;
                OnPropertyChanged();
            }
        }

        public int SessionLengthDisplayField {
            get => SessionLength;
            set {
                SessionLength = value;
                OnPropertyChanged();
            }
        }

        public int CyclesElapsedDisplayField {
            get => CyclesElapsed;
            set {
                CyclesElapsed = value;
                OnPropertyChanged();
            }
        }
    }
}
