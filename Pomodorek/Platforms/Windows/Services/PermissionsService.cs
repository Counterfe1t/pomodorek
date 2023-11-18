using System.Diagnostics;

namespace Pomodorek.Services;

public class PermissionsService : IPermissionsService
{
    public async Task<PermissionStatus> CheckAndRequestPermissionsAsync()
    {
        Debug.WriteLine("Requesting permissions is only supported on Android.");
        return await Task.FromResult(PermissionStatus.Unknown);
    }
}