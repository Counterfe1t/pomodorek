namespace Pomodorek.Services;

public class SoundService : ISoundService
{
    private readonly IAudioManager _audioManager;
    private readonly IFileSystem _fileSystem;
    private readonly ISettingsService _settingsService;
    private readonly IConfigurationService _configurationService;

    private AppSettings AppSettings => _configurationService.GetAppSettings();

    private bool IsSoundEnabled =>
        _settingsService.Get(Constants.Settings.IsSoundEnabled, AppSettings.DefaultIsSoundEnabled);

    public SoundService(
        IAudioManager audioManager,
        IFileSystem fileSystem,
        ISettingsService settingsService,
        IConfigurationService configurationService)
    {
        _audioManager = audioManager;
        _fileSystem = fileSystem;
        _settingsService = settingsService;
        _configurationService = configurationService;
    }

    public async Task PlaySoundAsync(string sound)
    {
        if (!IsSoundEnabled)
            return;

        using var stream = await _fileSystem.OpenAppPackageFileAsync(sound);
        using var player = _audioManager.CreatePlayer(stream);

        player.Volume = _settingsService.Get(
            Constants.Settings.SoundVolume,
            AppSettings.DefaultSoundVolume);

        player.Play();

        // For some reason IAudioPlayer.Duration always returns 0
        await Task.Delay(3000);
    }
}
