using Pomodorek.Models;
using Pomodorek.Services;
using System;
using Xamarin.Forms;

namespace Pomodorek.ViewModels
{
    public class TimerViewModel : BaseViewModel
    {
        private readonly TimerModel _timer;
        private IDeviceNotificationService _notificationService;
        private IDeviceSoundService _soundService;

        #region Properties
        private short _time = 0;
        public short Time
        {
            get => _time;
            set
            {
                _time = value;
                OnPropertyChanged();
            }
        }

        private TimerModeEnum _mode = TimerModeEnum.Disabled;
        public TimerModeEnum Mode
        {
            get => _mode;
            set
            {
                _mode = value;
                OnPropertyChanged();
            }
        }

        private short _sessionLength = 2;
        public short SessionLength
        {
            get => _sessionLength;
            set
            {
                _sessionLength = value;
                OnPropertyChanged();
            }
        }

        private bool _isRunning = false;
        public bool IsRunning
        {
            get => _isRunning;
            set
            {
                _isRunning = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public TimerViewModel()
        {
            _timer = new TimerModel(HandleOnTickEvent);
        }

        public void StartSession()
        {
            if (IsRunning)
                return;
            IsRunning = true;
            SetTimer(Constants.FocusLength, TimerModeEnum.Focus);
            PlayStartSessionSound();
        }

        public void StopSession()
        {
            _timer.Stop();
            Time = 0;
            IsRunning = false;
        }

        #region Services
        private void PlayStartSessionSound()
        {
            _soundService = DependencyService.Get<IDeviceSoundService>();
            using (_soundService as IDisposable)
            {
                _soundService.PlayStartSound();
            }
        }

        private void DisplayNotification(string message)
        {
            _notificationService = DependencyService.Get<IDeviceNotificationService>();
            using (_notificationService as IDisposable)
            {
                _notificationService.DisplayNotification(message);
            }
        }

        private void DisplaySessionOverNotification()
        {
            _notificationService = DependencyService.Get<IDeviceNotificationService>();
            using (_notificationService as IDisposable)
            {
                _notificationService.DisplaySessionOverNotification(Constants.SessionOverNotificationMessage);
            }
        }
        #endregion

        private void SetTimer(short time, TimerModeEnum mode)
        {
            Mode = mode;
            Time = time;
            _timer.Start();
        }

        private void HandleOnTickEvent()
        {
            if (_time == 0)
            {
                _timer.Stop();
                HandleOnFinishedEvent();
            }
            else
                Time--;
        }

        private void HandleOnFinishedEvent()
        {
            switch (Mode)
            {
                case TimerModeEnum.Focus:
                    // stop execution if session is over
                    // if 4 sessions elapsed start long rest
                    if (--SessionLength <= 0)
                    {
                        StopSession();
                        DisplaySessionOverNotification();
                        break;
                    }

                    SetTimer(Constants.ShortRestLength, TimerModeEnum.ShortRest);
                    DisplayNotification(Constants.ShortRestNotificationMessage);
                    break;
                case TimerModeEnum.ShortRest:
                case TimerModeEnum.LongRest:
                    SetTimer(Constants.FocusLength, TimerModeEnum.Focus);
                    DisplayNotification(Constants.FocusNotificationMessage);
                    break;
                case TimerModeEnum.Disabled:
                default:
                    break;
            }
        }
    }
}
