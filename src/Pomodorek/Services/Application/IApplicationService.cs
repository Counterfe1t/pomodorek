namespace Pomodorek.Services;

public interface IApplicationService
{
    /// <summary>
    /// Get current application context.
    /// </summary>
    Application Application { get; }
}