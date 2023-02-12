using Microsoft.Extensions.Configuration;
using Pomodorek.Models;

namespace Pomodorek.Services;

public class ConfigurationService : IConfigurationService
{
    private readonly IConfiguration _configuration;

    public ConfigurationService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public AppSettings GetAppSettings() =>
        _configuration.Get<AppSettings>();
}
