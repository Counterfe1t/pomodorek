namespace Pomodorek.Models.Constants;

public static class Constants
{
    public const string ApplicationName = "Pomodorek";
    public const string AppSettingsFileName = "Pomodorek.appsettings.json";

    public const int OneMinuteInSec = 60;
    public const int OneSecondInTicks = 10_000_000;
    public const int OneSecondInMs = 1_000;

    public class Settings
    {
        public const string IsDarkThemeEnabled = nameof(IsDarkThemeEnabled);
        public const string IsSoundEnabled = nameof(IsSoundEnabled);
        public const string SoundVolume = nameof(SoundVolume);
        public const string WorkLengthInMin = nameof(WorkLengthInMin);
        public const string ShortRestLengthInMin = nameof(ShortRestLengthInMin);
        public const string LongRestLengthInMin = nameof(LongRestLengthInMin);
        public const string IntervalsCount = nameof(IntervalsCount);
        public const string SavedSession = nameof(SavedSession);
    }

    public class Pages
    {
        public const string Pomodorek = nameof(Pomodorek);
        public const string Settings = nameof(Settings);
    }

    public class Labels
    {
        public const string Work = "Work";
        public const string ShortRest = "Rest";
        public const string LongRest = "Long Rest";
    }

    public class Messages
    {
        public const string Work = "Timer finished at {{value}}. Get back to work.";
        public const string ShortRest = "Timer finished at {{value}}. Take a short break.";
        public const string LongRest = "Timer finished at {{value}}. Take a long break.";
        public const string SessionOver = "Session has ended.";
        public const string SettingsSaved = "Settings have been saved.";
        public const string SettingsRestored = "Default settings have been restored.";
        public const string Confirm = "Okay";
        public const string Cancel = "Cancel";
        public const string TimerIsRunning = "Timer is running";
        public const string ResetSession = "This action will reset the current session in progress.\nAre you sure?";
        public const string RestoreDefaultSettings = "This action will restore default settings.\nAre you sure?";
    }

    public class Sounds
    {
        public const string IntervalStart = "session_start.wav";
        public const string IntervalOver = "session_over.wav";
    }
}