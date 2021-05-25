using Pomodorek.ViewModels;
using System;
using Xamarin.Forms;

namespace Pomodorek.Models {
    public class ApplicationTimerModel {

        #region Properties

        public bool IsEnabled { get; set; }

        public bool IsPaused { get; set; }

        public int Seconds { get; set; }

        public int Minutes { get; set; }

        public TimerModeEnum Mode { get; set; }

        public int SessionLength { get; set; } = 1;

        public int CyclesElapsed { get; set; }

        public int RestLength { get; set; }

        private Guid SessionId { get; set; }

        private MainPageViewModel ViewModel { get; set; }

        #endregion

        public ApplicationTimerModel(MainPageViewModel viewModel) {
            ViewModel = viewModel;
            RestoreDataToDefault();
        }

        public void StartOrPauseTimer() {

            if (!IsPaused) {
                ViewModel.IsPaused = true;
                return;
            }

            if (Mode == TimerModeEnum.Disabled) {
                ViewModel.Mode = TimerModeEnum.Focus;
                ViewModel.PlayStartSound();
            }

            StartTimer();
        }

        public void ResetTimer() {
            RestoreDataToDefault();
            UpdateView();
        }

        #region Private methods

        private void StartTimer() {
            var sessionId = SessionId = Guid.NewGuid();
            ViewModel.IsEnabled = true;
            ViewModel.IsPaused = false;

            Device.StartTimer(TimeSpan.FromSeconds(1), () => {
                if (sessionId != SessionId) {
                    return false;
                }

                return HandleOnChooseTimerMode();
            });
        }

        private void RestoreDataToDefault() {
            SessionId = Guid.Empty;
            IsEnabled = false;
            IsPaused = true;
            Seconds = Minutes = CyclesElapsed = 0;
            Mode = TimerModeEnum.Disabled;
            RestLength = Consts.ShortRestLength;
        }

        private bool HandleOnChooseTimerMode() =>
            IsEnabled
            && !IsPaused
            && (Mode == TimerModeEnum.Focus
                ? HandleOnFocusIntervalElapsed()
                : HandleOnRestIntervalElapsed());

        private bool HandleOnFocusIntervalElapsed() {
            Seconds++;
            if (Seconds >= 4) {
                Minutes++;
                Seconds = 0;
            }

            if (Minutes >= Consts.FocusLength) {
                Minutes = 0;
                ViewModel.CyclesElapsed = ++CyclesElapsed;

                if (CyclesElapsed >= SessionLength) {
                    RestoreDataToDefault();
                    UpdateView();
                    ViewModel.DisplaySessionOverNotification(Consts.SessionOverNotificationMessage);
                    return false;
                }

                if (CyclesElapsed % 4 == 0) {
                    RestLength = Consts.LongRestLength;
                }

                ViewModel.Mode =
                    RestLength == Consts.ShortRestLength
                        ? TimerModeEnum.Rest
                        : TimerModeEnum.LongRest;
                var message =
                    Mode == TimerModeEnum.Rest
                        ? Consts.ShortRestModeNotificationMessage
                        : Consts.LongRestModeNotificationMessage;
                ViewModel.DisplayNotification(message);
            }

            ViewModel.Seconds = Seconds;
            ViewModel.Minutes = Minutes;

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

                if (RestLength >= Consts.LongRestLength) {
                    RestLength = Consts.ShortRestLength;
                }

                ViewModel.Mode = TimerModeEnum.Focus;
                ViewModel.DisplayNotification(Consts.FocusModeNotificationMessage);
            }

            ViewModel.Seconds = Seconds;
            ViewModel.Minutes = Minutes;

            return true;
        }

        private void UpdateView() {
            ViewModel.IsEnabled = IsEnabled;
            ViewModel.IsPaused = IsPaused;
            ViewModel.Seconds = Seconds;
            ViewModel.Minutes = Minutes;
            ViewModel.Mode = Mode;
            ViewModel.CyclesElapsed = CyclesElapsed;
        }

        #endregion
    }
}
