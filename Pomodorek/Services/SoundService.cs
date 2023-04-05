namespace Pomodorek.Services;

public class SoundService : ISoundService
{
    private readonly IAudioManager _audioManager;
    private readonly IFileSystem _fileSystem;
    private readonly ISettingsService _settingsService;
    private readonly IConfigurationService _configurationService;

    private AppSettings AppSettings => _configurationService.GetAppSettings();

    private bool IsSoundEnabled => _settingsService.Get(Constants.Settings.IsSoundEnabled, true);

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

    public async Task PlaySound(string sound)
    {
        if (!IsSoundEnabled)
            return;

        var stream = await _fileSystem.OpenAppPackageFileAsync(sound);
        var player = _audioManager.CreatePlayer(stream);
        
        player.Volume = _settingsService.Get(
            Constants.Settings.SoundVolume,
            AppSettings.DefaultSoundVolume);

        player.Play();

        // TODO: Release used memory once player has stopped playing
        // For some reason IAudioPlayer.Duration always returns 0
        await Task.Delay(5000);

        stream?.Dispose();
        player?.Dispose();
    }
}
