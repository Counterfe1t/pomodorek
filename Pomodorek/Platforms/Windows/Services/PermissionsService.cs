namespace Pomodorek.Services;

public class PermissionsService : IPermissionsService
{
    public Task<PermissionStatus> CheckAndRequestPermissionsAsync()
    {
        throw new PlatformNotSupportedException("Requesting permissions is only supported on Android.");
    }
}