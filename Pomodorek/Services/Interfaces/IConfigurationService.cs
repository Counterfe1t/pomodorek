namespace Pomodorek.Services;

public interface IConfigurationService
{
    /// <summary>
    /// Get application configuration.
    /// </summary>
    /// <returns><see cref="AppSettings" /> object containing application configuration</returns>
    AppSettings GetAppSettings();
}