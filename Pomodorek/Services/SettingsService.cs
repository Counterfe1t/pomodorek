namespace Pomodorek.Services;

public class SettingsService : ISettingsService
{
    public T Get<T>(string key, T defaultValue) =>
        Preferences.Default.Get<T>(key, defaultValue);
    public void Set<T>(string key, T value) =>
        Preferences.Default.Set<T>(key, value);
}
