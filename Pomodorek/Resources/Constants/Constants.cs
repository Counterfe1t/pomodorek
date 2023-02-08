namespace Pomodorek.Resources.Constants;

public static class Constants
{
    // TODO: Move these values to app config
    public const short FocusLength = 25 * 60;
    public const short ShortRestLength = 5 * 60;
    public const short LongRestLength = 15 * 60; // 15-30 min break

    public const string FocusLabel = "FOCUS";
    public const string ShortRestLabel = "REST";
    public const string LongRestLabel = "LONG REST";
    public const string StoppedLabel = "STOPPED";

    public const string FocusNotificationMessage = "Get back to work.";
    public const string ShortRestNotificationMessage = "Take a short break.";
    public const string LongRestNotificationMessage = "Take a long break.";
    public const string SessionOverNotificationMessage = "Session has ended.";
}
