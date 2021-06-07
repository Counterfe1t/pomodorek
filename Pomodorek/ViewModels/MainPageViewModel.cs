using Pomodorek.Models;
using Pomodorek.Services;
using System;
using Xamarin.Forms;

namespace Pomodorek.ViewModels {
    public class MainPageViewModel : BaseViewModel {

        private readonly ApplicationTimerModel _applicationTimer;

        private IDeviceNotificationService notificationService = null;

        private IDeviceSoundService soundService = null;

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

        public bool IsPaused {
            get => _applicationTimer.IsPaused;
            set {
                _applicationTimer.IsPaused = value;
                OnPropertyChanged();
            }
        }

        public double Progress {
            get => _applicationTimer.Progress;
            set {
                _applicationTimer.Progress = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public MainPageViewModel() {
            _applicationTimer = new ApplicationTimerModel(this);
        }

        public void StartOrPauseTimer() {
            _applicationTimer.StartOrPauseTimer();
        }

        public void ResetTimer() {
            _applicationTimer.ResetTimer();
        }

        public void DisplayNotification(string message) {
            notificationService = DependencyService.Get<IDeviceNotificationService>();
            using (notificationService as IDisposable) {
                Device.BeginInvokeOnMainThread(async () => {
                    await notificationService?.DisplayNotification(message);
                });
            }
        }
        
        public void DisplaySessionOverNotification(string message) {
            notificationService = DependencyService.Get<IDeviceNotificationService>();
            using (notificationService as IDisposable) {
                Device.BeginInvokeOnMainThread(async () => {
                    await notificationService?.DisplaySessionOverNotification(message);
                });
            }
        }

        public void PlayStartSound() {
            soundService = DependencyService.Get<IDeviceSoundService>();
            using (soundService as IDisposable) {
                Device.BeginInvokeOnMainThread(async () => {
                    await soundService?.PlayStartSound();
                });
            }
        }
    }
}
