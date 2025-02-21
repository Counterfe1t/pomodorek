namespace Pomodorek.Interfaces;

public interface IApplicationService
{
    /// <summary>
    /// Get current application context.
    /// </summary>
    Application Application { get; }
}