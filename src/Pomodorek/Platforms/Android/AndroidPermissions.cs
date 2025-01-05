namespace Pomodorek.Platforms.Android;

public class AndroidPermissions : Permissions.BasePlatformPermission
{
    public override (string androidPermission, bool isRuntime)[] RequiredPermissions =>
    [
        (global::Android.Manifest.Permission.ForegroundService, true),
        (global::Android.Manifest.Permission.PostNotifications, true),
        (global::Android.Manifest.Permission.ScheduleExactAlarm, true)
    ];
}