using Pomodorek.Models;
using Pomodorek.ViewModels;
using System.Timers;

namespace Pomodorek.Impl {
    public class ApplicationTimer {

        #region Properties
        public MainPageViewModel ViewModel { get; set; }

        public Timer SystemTimer { get; set; }

        public int Seconds { get; set; }

        public int Minutes { get; set; }

        public int SessionLength { get; set; }

        public int CyclesElapsed { get; set; }

        public int RestLength { get; set; }
        #endregion

        #region Constants
        private const int _interval = 1000;
        private const int _shortRestLength = 1; //5;
        private const int _longRestLength = 2; //35;
        private const int _workLength = 2; //25;
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
            SystemTimer.Enabled = !SystemTimer.Enabled;
        }

        public void StopTimer() {
            SystemTimer.Stop();
            SystemTimer.Dispose();
            CyclesElapsed = 0;
            RestLength = _shortRestLength;
            Seconds = 0;
            Minutes = 0;
            SetSecondsDisplayValue();
            SetMinutesDisplayValue();
            ViewModel.ModeDisplayField = "";
        }

        #region Events
        private void OnTimedEvent_Work(object sender, ElapsedEventArgs eventArgs) {
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

                SystemTimer.Elapsed -= OnTimedEvent_Work;
                SystemTimer.Elapsed += OnTimedEvent_Rest;
                ViewModel.ModeDisplayField = Consts.RestModeLabel;
            }

            // Zaktualizuj wartość wyświetlaną na UI
            SetSecondsDisplayValue();
            SetMinutesDisplayValue();
        }

        private void OnTimedEvent_Rest(object sender, ElapsedEventArgs eventArgs) {
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

                SystemTimer.Elapsed -= OnTimedEvent_Rest;
                SystemTimer.Elapsed += OnTimedEvent_Work;
                ViewModel.ModeDisplayField = Consts.FocusModeLabel;
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
            SystemTimer.Elapsed += OnTimedEvent_Work;
            SystemTimer.AutoReset = true;
            SystemTimer.Start();
            ViewModel.ModeDisplayField = Consts.FocusModeLabel;
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
