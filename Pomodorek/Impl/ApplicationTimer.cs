using Pomodorek.Models;
using Pomodorek.ViewModels;
using System.Timers;

namespace Pomodorek.Impl {
    public class ApplicationTimer {

        #region Constants
        private const int _interval = 1000;
        private const int _shortRestLength = 1; //5;
        private const int _longRestLength = 2; //35;
        private const int _workLength = 2; //25;
        #endregion

        #region Properties
        public MainPageViewModel ViewModel { get; set; }

        public Timer SystemTimer { get; set; }

        public int Seconds { get; set; }

        public int Minutes { get; set; }

        public int SessionLength { get; set; }

        public int CyclesElapsed { get; set; }

        public int RestLength { get; set; }
        #endregion

        public ApplicationTimer(MainPageViewModel viewModel) {
            ViewModel = viewModel;
        }

        public void StartNewSession(int sessionLength) {
            if (SystemTimer != null && SystemTimer.Enabled) {
                return;
            }
            InitializeData(sessionLength);
        }

        public void PauseUnpauseTimer() {
            if (SystemTimer == null) {
                return;
            }
            SystemTimer.Enabled = !SystemTimer.Enabled;
        }

        public void StopTimer() {
            DisableTimerAndSetDataToDefault();
        }

        #region Events
        private void OnIntervalElapsed_Focus(object sender, ElapsedEventArgs eventArgs) {
            Seconds++;
            if (Seconds >= 4) {
                Minutes++;
                Seconds = 0;
            }

            if (Minutes >= _workLength) {
                Minutes = 0;
                CyclesElapsed++;

                if (CyclesElapsed >= SessionLength) {
                    StopTimer();
                    return;
                }

                if (CyclesElapsed % 4 == 0) {
                    SetLongRest();
                }

                SystemTimer.Elapsed -= OnIntervalElapsed_Focus;
                SystemTimer.Elapsed += OnIntervalElapsed_Rest;
                ViewModel.ModeDisplayField = RestLength == _shortRestLength
                    ? TimerModeEnum.Rest
                    : TimerModeEnum.LongRest;
            }

            // Zaktualizuj wartość wyświetlaną na UI
            SetSecondsDisplayValue();
            SetMinutesDisplayValue();
        }

        private void OnIntervalElapsed_Rest(object sender, ElapsedEventArgs eventArgs) {
            Seconds++;
            if (Seconds >= 4) {
                Minutes++;
                Seconds = 0;
            }

            if (Minutes >= RestLength) {
                Minutes = 0;

                if (RestLength >= _longRestLength) {
                    SetShortRest();
                }

                SystemTimer.Elapsed -= OnIntervalElapsed_Rest;
                SystemTimer.Elapsed += OnIntervalElapsed_Focus;
                ViewModel.ModeDisplayField = TimerModeEnum.Focus;
            }

            // Zaktualizuj wartość wyświetlaną na UI
            SetSecondsDisplayValue();
            SetMinutesDisplayValue();
        }
        #endregion

        #region Private methods
        private void InitializeData(int sessionLength) {
            SessionLength = sessionLength;
            CyclesElapsed = 0;
            RestLength = _shortRestLength;
            Seconds = 0;
            Minutes = 0;
            SystemTimer = new Timer(_interval);
            SystemTimer.Elapsed += OnIntervalElapsed_Focus;
            SystemTimer.AutoReset = true;
            SystemTimer.Start();
            ViewModel.ModeDisplayField = TimerModeEnum.Focus;
        }

        private void DisableTimerAndSetDataToDefault() {
            SystemTimer?.Stop();
            SystemTimer = null;
            CyclesElapsed = 0;
            RestLength = _shortRestLength;
            Seconds = 0;
            Minutes = 0;
            SetSecondsDisplayValue();
            SetMinutesDisplayValue();
            ViewModel.ModeDisplayField = TimerModeEnum.Disabled;
        }

        private void SetShortRest() {
            RestLength = _shortRestLength;
        }

        private void SetLongRest() {
            RestLength = _longRestLength;
        }

        private void SetSecondsDisplayValue() {
            ViewModel.SecondsDisplayField = Seconds;
        }

        private void SetMinutesDisplayValue() {
            ViewModel.MinutesDisplayField = Minutes;
        }
        #endregion
    }
}
