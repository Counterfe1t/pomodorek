namespace Pomodorek.Services;

public interface ISettingsService
{
    /// <summary>
    /// Get setting value from device preferences.
    /// </summary>
    /// <param name="key">Setting key.</param>
    /// <param name="defaultValue">Default setting value.</param>
    /// <returns>Setting value from device preferences or default value.</returns>
    T Get<T>(string key, T defaultValue);

    /// <summary>
    /// Set value to device preferences.
    /// </summary>
    /// <param name="key">Setting key.</param>
    /// <param name="value">Default setting value.</param>
    void Set<T>(string key, T value);
}