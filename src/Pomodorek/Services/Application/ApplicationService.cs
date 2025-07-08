namespace Pomodorek.Services;

public class ApplicationService : IApplicationService
{
    public Application Application
        => Application.Current ?? throw new Exception("Application should not be null.");
}