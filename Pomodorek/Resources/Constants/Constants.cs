namespace Pomodorek.Resources.Constants;

public static class Constants
{
    public const string AppSettingsFileName = "Pomodorek.appsettings.json";

    public const int SixtySeconds = 60;

    public class Settings
    {
        public const string IsSoundEnabled = nameof(IsSoundEnabled);
        public const string SoundVolume = nameof(SoundVolume);
        public const string FocusLengthInMin = nameof(FocusLengthInMin);
        public const string ShortRestLengthInMin = nameof(ShortRestLengthInMin);
        public const string LongRestLengthInMin = nameof(LongRestLengthInMin);
        public const string NumberOfSessions = nameof(NumberOfSessions);
    }

    public class Pages
    {
        public const string Pomodorek = nameof(Pomodorek);
        public const string Settings = nameof(Settings);
    }

    public class Labels
    {
        public const string Focus = "FOCUS";
        public const string ShortRest = "REST";
        public const string LongRest = "LONG REST";
        public const string Stopped = "STOPPED";
    }

    public class Messages
    {
        public const string Focus = "Get back to work.";
        public const string ShortRest = "Take a short break.";
        public const string LongRest = "Take a long break.";
        public const string SessionOver = "Session has ended.";
        public const string SettingsSaved = "Settings have been saved.";
        public const string SettingsRestored = "Default settings have been restored.";
        public const string Confirm = "Okay";
        public const string Cancel = "Cancel";
    }

    public class Sounds
    {
        public const string SessionStart = "session_start.wav";
        public const string SessionOver = "session_over.wav";
    }
}