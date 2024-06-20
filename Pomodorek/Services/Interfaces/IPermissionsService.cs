namespace Pomodorek.Services;

public interface IPermissionsService
{
    /// <summary>
    /// Check if the application has necessary permissions. If the result is negative request access.
    /// </summary>
    /// <returns><see cref="PermissionStatus" /> object representing the result of the invoked action.</returns>
    Task<PermissionStatus> CheckAndRequestPermissionsAsync();
}