namespace Pomodorek.Services;

public interface IConfigurationService
{
    /// <summary>
    /// Application settings.
    /// </summary> 
    AppSettings AppSettings { get; }
}