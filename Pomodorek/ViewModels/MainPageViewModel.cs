namespace Pomodorek.ViewModels {
    public class MainPageViewModel : BaseViewModel {

        public int Seconds { get; set; }

        public int Minutes { get; set; }

        public string Mode { get; set; }

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

        public string ModeDisplayField {
            get {
                return Mode;
            }
            set {
                Mode = value;
                OnPropertyChanged();
            }
        }
    }
}
