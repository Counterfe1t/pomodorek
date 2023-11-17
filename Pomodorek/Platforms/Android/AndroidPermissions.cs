namespace Pomodorek.Platforms.Android;

public class AndroidPermissions : Permissions.BasePlatformPermission
{
    public override (string androidPermission, bool isRuntime)[] RequiredPermissions =>
        new List<(string androidPermission, bool isRuntime)>
        {
            (global::Android.Manifest.Permission.ForegroundService, true),
            (global::Android.Manifest.Permission.PostNotifications, true)
        }.ToArray();
}