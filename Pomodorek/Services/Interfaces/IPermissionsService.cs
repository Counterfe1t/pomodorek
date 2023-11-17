namespace Pomodorek.Services;

public interface IPermissionsService
{
    Task<PermissionStatus> CheckAndRequestPermissionsAsync();
}