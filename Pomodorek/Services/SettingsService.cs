namespace Pomodorek.Services;

public class SettingsService : ISettingsService
{
    private readonly IPreferences _preferences;

    public SettingsService(IPreferences preferences)
    {
        _preferences = preferences;
    }

    public T Get<T>(string key, T defaultValue) => _preferences.Get(key, defaultValue);

    public void Set<T>(string key, T value) => _preferences.Set(key, value);
}
