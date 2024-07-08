namespace Pomodorek.Interfaces;

public interface IConfigurationService
{
    /// <summary>
    /// Application settings.
    /// </summary> 
    AppSettings AppSettings { get; }
}