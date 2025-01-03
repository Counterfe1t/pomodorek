namespace Pomodorek.Services;

public class PermissionsService : IPermissionsService
{
    public async Task<PermissionStatus> CheckAndRequestPermissionsAsync()
    {
        // TODO: Check for necessary permissions on ios platform
        return PermissionStatus.Granted;
    }
}