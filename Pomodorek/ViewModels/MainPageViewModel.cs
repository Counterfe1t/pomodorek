using Pomodorek.Logic;
using Pomodorek.Models;
using Pomodorek.Services;
using System;
using Xamarin.Forms;

namespace Pomodorek.ViewModels {
    public class MainPageViewModel : BaseViewModel {

        private readonly ApplicationTimer _applicationTimer;

        private IDeviceNotificationService _deviceNotificationService = null;

        #region Properties

        public int Seconds {
            get => _applicationTimer.Seconds;
            set {
                _applicationTimer.Seconds = value;
                OnPropertyChanged();
            }
        }

        public int Minutes {
            get => _applicationTimer.Minutes;
            set {
                _applicationTimer.Minutes = value;
                OnPropertyChanged();
            }
        }

        public TimerModeEnum Mode {
            get => _applicationTimer.Mode;
            set {
                _applicationTimer.Mode = value;
                OnPropertyChanged();
            }
        }

        public int SessionLength {
            get => _applicationTimer.SessionLength;
            set {
                _applicationTimer.SessionLength = value;
                OnPropertyChanged();
            }
        }

        public int CyclesElapsed {
            get => _applicationTimer.CyclesElapsed;
            set {
                _applicationTimer.CyclesElapsed = value;
                OnPropertyChanged();
            }
        }

        public bool IsEnabled {
            get => _applicationTimer.IsEnabled;
            set {
                _applicationTimer.IsEnabled = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public MainPageViewModel() {
            _applicationTimer = new ApplicationTimer(this);
        }

        public void StartOrPauseTimer() {
            _applicationTimer.StartOrPauseTimer();
        }

        public void ResetTimer() {
            _applicationTimer.ResetTimer();
        }

        public void DisplayNotification(string message) {
            _deviceNotificationService = DependencyService.Get<IDeviceNotificationService>();
            using (_deviceNotificationService as IDisposable) {
                _deviceNotificationService.DisplayNotification(message);
            }
        }
    }
}
