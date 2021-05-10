using Pomodorek.Models;
using Pomodorek.ViewModels;
using System;
using Xamarin.Forms;

namespace Pomodorek.Impl {
    public class ApplicationTimer {

        #region Constants

        private const int _interval = 1000;
        private const int _shortRestLength = 1; //5;
        private const int _longRestLength = 2; //20;
        private const int _workLength = 2; //25;

        #endregion

        #region Properties

        private MainPageViewModel ViewModel { get; set; }

        public bool Enabled { get; set; }

        public int Seconds { get; set; }

        public int Minutes { get; set; }

        public int SessionLength { get; set; } = 1;

        public int CyclesElapsed { get; set; }

        public int RestLength { get; set; }

        public TimerModeEnum Mode { get; set; }

        #endregion

        public ApplicationTimer(MainPageViewModel viewModel) {
            ViewModel = viewModel;
        }

        public void StartSession(int sessionLength) {
            if (Enabled) {
                return;
            }

            InitializeData(sessionLength);
            StartTimer();
        }

        public void PauseOrUnpauseTimer() {
            //Paused = !Paused;
            //if (!Paused) {
            //    StartTimer();
            //}
        }

        public void StopSession() {
            if (Enabled) {
                SetDataToDefault();
            }
        }

        #region Private methods

        private void StartTimer() {
            Device.StartTimer(TimeSpan.FromSeconds(1), () => HandleOnChooseTimerMode());
        }

        private void InitializeData(int sessionLength) {
            Enabled = true;
            SessionLength = sessionLength;
            RestLength = _shortRestLength;
            Seconds = 0;
            Minutes = 0;
            CyclesElapsed = 0;
            Mode = TimerModeEnum.Focus;

            Device.BeginInvokeOnMainThread(() => {
                ViewModel.ModeDisplayField = Mode;
                ViewModel.CyclesElapsedDisplayField = CyclesElapsed;
            });
        }

        private void SetDataToDefault() {
            Enabled = false;
            Seconds = 0;
            Minutes = 0;
            RestLength = _shortRestLength;
            CyclesElapsed = 0;
            Mode = TimerModeEnum.Disabled;

            Device.BeginInvokeOnMainThread(() => {
                ViewModel.CyclesElapsedDisplayField = CyclesElapsed;
                ViewModel.ModeDisplayField = Mode;
                ViewModel.SecondsDisplayField = Seconds;
                ViewModel.MinutesDisplayField = Minutes;
            });
        }

        private bool HandleOnChooseTimerMode() {
            return Enabled && (Mode == TimerModeEnum.Focus
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
                    ViewModel.CyclesElapsedDisplayField = CyclesElapsed;
                });

                if (CyclesElapsed >= SessionLength) {
                    Device.BeginInvokeOnMainThread(() => {
                        ViewModel.DisplayAlert(
                            Consts.SessionEndedAlertTitle,
                            Consts.SessionEndedAlertMessage,
                            Consts.SessionEndedAlertCancel);
                    });
                    SetDataToDefault();
                    return false;
                }

                if (CyclesElapsed % 4 == 0) {
                    RestLength = _longRestLength;
                }

                Mode = RestLength == _shortRestLength
                    ? TimerModeEnum.Rest
                    : TimerModeEnum.LongRest;

                Device.BeginInvokeOnMainThread(() => {
                    ViewModel.ModeDisplayField = Mode;
                });
            }

            Device.BeginInvokeOnMainThread(() => {
                ViewModel.SecondsDisplayField = Seconds;
                ViewModel.MinutesDisplayField = Minutes;
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
                    ViewModel.ModeDisplayField = Mode;
                });
            }

            Device.BeginInvokeOnMainThread(() => {
                ViewModel.SecondsDisplayField = Seconds;
                ViewModel.MinutesDisplayField = Minutes;
            });

            return true;
        }

        #endregion
    }
}
