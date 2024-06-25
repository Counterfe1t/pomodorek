namespace Pomodorek.Services.Interfaces;

public interface IApplicationService
{
    /// <summary>
    /// Current application context.
    /// </summary>
    Application Application { get; }
}