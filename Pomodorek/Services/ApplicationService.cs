namespace Pomodorek.Services;

public class ApplicationService : IApplicationService
{
    public Application Application => Application.Current;
}