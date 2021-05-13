using Pomodorek.Models;
using Pomodorek.ViewModels;
using System;
using Xamarin.Forms;

namespace Pomodorek.Logic {
    public class ApplicationTimer {

        #region Constants

        private const int _shortRestLength = 1; //5;
        private const int _longRestLength = 2; //20;
        private const int _workLength = 2; //25;

        #endregion

        #region Properties

        private MainPageViewModel ViewModel { get; set; }

        public bool IsEnabled { get; set; }

        public int Seconds { get; set; }

        public int Minutes { get; set; }

        public TimerModeEnum Mode { get; set; }

        public int SessionLength { get; set; } = 1;

        public int CyclesElapsed { get; set; }

        public int RestLength { get; set; }

        #endregion

        public ApplicationTimer(MainPageViewModel viewModel) {
            ViewModel = viewModel;
            RestoreDataToDefault();
        }

        public void StartOrPauseTimer() {
            if (IsEnabled) {
                IsEnabled = false;
                Device.BeginInvokeOnMainThread(() => {
                    ViewModel.IsEnabled = IsEnabled;
                });
                return;
            }

            if (Mode == TimerModeEnum.Disabled) {
                Mode = TimerModeEnum.Focus;
                ViewModel.PlayStartSound();
                Device.BeginInvokeOnMainThread(() => {
                    ViewModel.Mode = Mode;
                });
            }

            EnableTimer();
        }

        public void ResetTimer() {
            RestoreDataToDefault();
            UpdateView();
        }

        #region Private methods

        private void EnableTimer() {
            IsEnabled = true;
            Device.BeginInvokeOnMainThread(() => {
                ViewModel.IsEnabled = IsEnabled;
            });
            Device.StartTimer(TimeSpan.FromSeconds(1), () => HandleOnChooseTimerMode());
        }

        private void RestoreDataToDefault() {
            IsEnabled = false;
            Seconds = 0;
            Minutes = 0;
            Mode = TimerModeEnum.Disabled;
            CyclesElapsed = 0;
            RestLength = _shortRestLength;
        }

        private bool HandleOnChooseTimerMode() {
            return IsEnabled && (Mode == TimerModeEnum.Focus
                ? HandleOnFocusIntervalElapsed()
                : HandleOnRestIntervalElapsed());
        }

        private bool HandleOnFocusIntervalElapsed() {
            Seconds++;
            if (Seconds >= 4) {
                Minutes++;
                Seconds = 0;
            }

            if (Minutes >= _workLength) {
                Minutes = 0;
                CyclesElapsed++;

                Device.BeginInvokeOnMainThread(() => {
                    ViewModel.CyclesElapsed = CyclesElapsed;
                });

                if (CyclesElapsed >= SessionLength) {
                    RestoreDataToDefault();
                    UpdateView();
                    ViewModel.DisplaySessionOverNotification(Consts.SessionOverNotificationMessage);
                    return false;
                }

                if (CyclesElapsed % 4 == 0) {
                    RestLength = _longRestLength;
                }

                Mode =
                    RestLength == _shortRestLength
                        ? TimerModeEnum.Rest
                        : TimerModeEnum.LongRest;

                Device.BeginInvokeOnMainThread(() => {
                    ViewModel.Mode = Mode;
                });
                var message =
                    Mode == TimerModeEnum.Rest
                        ? Consts.ShortRestModeNotificationMessage
                        : Consts.LongRestModeNotificationMessage;
                ViewModel.DisplayNotification(message);
            }

            Device.BeginInvokeOnMainThread(() => {
                ViewModel.Seconds = Seconds;
                ViewModel.Minutes = Minutes;
            });

            return true;
        }

        private bool HandleOnRestIntervalElapsed() {
            Seconds++;
            if (Seconds >= 4) {
                Minutes++;
                Seconds = 0;
            }

            if (Minutes >= RestLength) {
                Minutes = 0;

                if (RestLength >= _longRestLength) {
                    RestLength = _shortRestLength;
                }

                Mode = TimerModeEnum.Focus;

                Device.BeginInvokeOnMainThread(() => {
                    ViewModel.Mode = Mode;
                });

                ViewModel.DisplayNotification(Consts.FocusModeNotificationMessage);
            }

            Device.BeginInvokeOnMainThread(() => {
                ViewModel.Seconds = Seconds;
                ViewModel.Minutes = Minutes;
            });

            return true;
        }

        private void UpdateView() {
            Device.BeginInvokeOnMainThread(() => {
                ViewModel.IsEnabled = IsEnabled;
                ViewModel.Seconds = Seconds;
                ViewModel.Minutes = Minutes;
                ViewModel.Mode = Mode;
                ViewModel.CyclesElapsed = CyclesElapsed;
            });
        }

        #endregion
    }
}
