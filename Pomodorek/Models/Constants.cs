namespace Pomodorek.Models
{
    public static class Constants
    {
        public const short FocusLength = 25 * 60;
        public const short ShortRestLength = 5 * 60;
        public const short LongRestLength = 15 * 60; // 15-30 min break

        public const string FocusModeLabel = "FOCUS";
        public const string ShortRestModeLabel = "REST";
        public const string LongRestModeLabel = "LONG REST";

        public const string FocusNotificationMessage = "Get back to work.";
        public const string ShortRestNotificationMessage = "Take a short break.";
        public const string LongRestNotificationMessage = "Take a long break.";
        public const string SessionOverNotificationMessage = "Session has ended.";

        public const string TickEvent = "tick";
        public const string FinishedEvent = "finished";
    }
}
