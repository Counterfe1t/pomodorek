using Pomodorek.Interfaces;

namespace Pomodorek.Services;

public class SoundService : ISoundService
{
    private readonly IAudioManager _audioManager;
    private readonly IFileSystem _fileSystem;
    private readonly ISettingsService _settingsService;

    private readonly AppSettings _appSettings;

    private bool IsSoundEnabled =>
        _settingsService.Get(Constants.Settings.IsSoundEnabled, _appSettings.DefaultIsSoundEnabled);

    public SoundService(
        IAudioManager audioManager,
        IFileSystem fileSystem,
        ISettingsService settingsService,
        IConfigurationService configurationService)
    {
        _appSettings = configurationService.AppSettings;
        _audioManager = audioManager;
        _fileSystem = fileSystem;
        _settingsService = settingsService;
    }

    public async Task PlaySoundAsync(string fileName)
    {
        if (!IsSoundEnabled)
            return;

        using var stream = await _fileSystem.OpenAppPackageFileAsync(fileName);
        using var player = _audioManager.CreatePlayer(stream);

        player.Volume = _settingsService.Get(Constants.Settings.SoundVolume, _appSettings.DefaultSoundVolume);
        player.Play();

        // TODO Fix issue with IAudioPlayer.Duration always returning zero.
        await Task.Delay(3000);
    }
}