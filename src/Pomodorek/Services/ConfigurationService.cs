namespace Pomodorek.Services;

public class ConfigurationService : IConfigurationService
{
    private readonly IConfiguration _configuration;

    public AppSettings AppSettings
        => _configuration.Get<AppSettings>() ?? throw new Exception("App settings should not be null.");

    public ConfigurationService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
}