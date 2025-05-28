namespace Pomodorek.Platforms.Android;

[SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
public class AndroidPermissions : Permissions.BasePlatformPermission
{
    public override (string androidPermission, bool isRuntime)[] RequiredPermissions =>
    [
        (global::Android.Manifest.Permission.ForegroundService, true),
        (global::Android.Manifest.Permission.PostNotifications, true),
        (global::Android.Manifest.Permission.ScheduleExactAlarm, true)
    ];
}