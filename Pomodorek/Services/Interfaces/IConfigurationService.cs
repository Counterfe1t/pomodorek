using Pomodorek.Models;

namespace Pomodorek.Services;

public interface IConfigurationService
{
    AppSettings GetAppSettings();
}
