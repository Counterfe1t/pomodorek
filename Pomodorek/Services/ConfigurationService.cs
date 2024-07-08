using Pomodorek.Interfaces;

namespace Pomodorek.Services;

public class ConfigurationService : IConfigurationService
{
    private readonly IConfiguration _configuration;

    public AppSettings AppSettings => _configuration.Get<AppSettings>();

    public ConfigurationService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
}