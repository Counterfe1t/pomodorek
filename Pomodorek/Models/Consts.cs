namespace Pomodorek.Models {
    public class Consts {
        public static readonly int FocusLength = 2; //25
        public static readonly int ShortRestLength = 1; //5
        public static readonly int LongRestLength = 2; //20

        public static readonly string FocusModeLabel = "FOCUS";
        public static readonly string ShortRestModeLabel = "REST";
        public static readonly string LongRestModeLabel = "LONG REST";

        public static readonly string FocusModeNotificationMessage = "Go back to work.";
        public static readonly string ShortRestModeNotificationMessage = "Take a short break.";
        public static readonly string LongRestModeNotificationMessage = "Take a long break.";
        public static readonly string SessionOverNotificationMessage = "Session has ended.";
    }
}
