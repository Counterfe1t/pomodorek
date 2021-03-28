using System;
using System.Collections.Generic;
using System.Text;

namespace Pomodorek.ViewModels {
    public class MainPageViewModel : BaseViewModel {

        public string Seconds { get; set; } = "00";

        public string Minutes { get; set; } = "00";

        public string Mode { get; set; }

        public string SecondsDisplayField {
            get {
                return Seconds;
            }
            set {
                Seconds = value;
                OnPropertyChanged();
            }
        }

        public string MinutesDisplayField {
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
