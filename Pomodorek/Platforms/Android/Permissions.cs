namespace Pomodorek.Platforms.Android;

public class NotificationPermission : Permissions.BasePlatformPermission
{
    public override (string androidPermission, bool isRuntime)[] RequiredPermissions =>
        new List<(string androidPermission, bool isRuntime)>
        {
            ("android.permission.POST_NOTIFICATIONS", true),
        }.ToArray();
}