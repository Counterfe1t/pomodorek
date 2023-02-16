namespace Pomodorek.Resources.Constants;

public static class Constants
{
    public const string AppSettingsFileName = "Pomodorek.appsettings.json";

    public const int OneMinuteInSec = 60;

    public const string FocusLengthInMin = nameof(FocusLengthInMin);
    public const string ShortRestLengthInMin = nameof(ShortRestLengthInMin);
    public const string LongRestLengthInMin = nameof(LongRestLengthInMin);

    public class PageTitles
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

    public class NotificationMessages
    {
        public const string Focus = "Get back to work.";
        public const string ShortRest = "Take a short break.";
        public const string LongRest = "Take a long break.";
        public const string SessionOver = "Session has ended.";
    }

    public class Sounds
    {
        public const string SessionStart = "session_start.wav";
        public const string SessionOver = "session_over.wav";
    }
}
