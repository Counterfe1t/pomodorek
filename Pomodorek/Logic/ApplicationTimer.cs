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
                    ViewModel.IsEnabledViewModel = IsEnabled;
                });
                return;
            }

            if (Mode == TimerModeEnum.Disabled) {
                Mode = TimerModeEnum.Focus;
                Device.BeginInvokeOnMainThread(() => {
                    ViewModel.ModeViewModel = Mode;
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
                ViewModel.IsEnabledViewModel = IsEnabled;
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
                    ViewModel.CyclesElapsedViewModel = CyclesElapsed;
                });

                if (CyclesElapsed >= SessionLength) {
                    RestoreDataToDefault();

                    UpdateView();
                    Device.BeginInvokeOnMainThread(() => {
                        ViewModel.DisplayAlert(
                            Consts.SessionEndedAlertTitle,
                            Consts.SessionEndedAlertMessage,
                            Consts.SessionEndedAlertCancel);
                    });

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
                    ViewModel.ModeViewModel = Mode;
                });
            }

            Device.BeginInvokeOnMainThread(() => {
                ViewModel.SecondsViewModel = Seconds;
                ViewModel.MinutesViewModel = Minutes;
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
                    ViewModel.ModeViewModel = Mode;
                });
            }

            Device.BeginInvokeOnMainThread(() => {
                ViewModel.SecondsViewModel = Seconds;
                ViewModel.MinutesViewModel = Minutes;
            });

            return true;
        }

        private void UpdateView() {
            Device.BeginInvokeOnMainThread(() => {
                ViewModel.IsEnabledViewModel = IsEnabled;
                ViewModel.SecondsViewModel = Seconds;
                ViewModel.MinutesViewModel = Minutes;
                ViewModel.ModeViewModel = Mode;
                ViewModel.CyclesElapsedViewModel = CyclesElapsed;
            });
        }

        #endregion
    }
}
