using Pomodorek.Platforms.Android;

namespace Pomodorek.Services;

public class PermissionsService : IPermissionsService
{
    public async Task<PermissionStatus> CheckAndRequestPermissionsAsync()
    {
        if (await Permissions.CheckStatusAsync<AndroidPermissions>() == PermissionStatus.Granted)
            return PermissionStatus.Granted;

        if (Permissions.ShouldShowRationale<AndroidPermissions>())
        {
            // TODO Prompt the user with additional information as to why the permission is needed
        }

        return await Permissions.RequestAsync<AndroidPermissions>();
    }
}