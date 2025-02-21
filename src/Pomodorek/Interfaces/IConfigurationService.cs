namespace Pomodorek.Interfaces;

public interface IConfigurationService
{
    /// <summary>
    /// Get application settings.
    /// </summary> 
    AppSettings AppSettings { get; }
}