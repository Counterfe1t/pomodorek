namespace Pomodorek.Services;

public interface ISettingsService
{
    T Get<T>(string key, T defaultValue);

    void Set<T>(string key, T value);
}