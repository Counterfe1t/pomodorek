using Plugin.Maui.Audio;

namespace Pomodorek.Services;

public class SoundService : ISoundService
{
    private readonly IAudioManager _audioManager;
    private readonly IFileSystem _fileSystem;
    private readonly ISettingsService _settingsService;

    private bool IsSoundEnabled => _settingsService.Get(Constants.Settings.IsSoundEnabled, true);

    public SoundService(
        IAudioManager audioManager,
        IFileSystem fileSystem,
        ISettingsService settingsService)
    {
        _audioManager = audioManager;
        _fileSystem = fileSystem;
        _settingsService = settingsService;
    }

    public async Task PlaySound(string sound)
    {
        if (!IsSoundEnabled)
            return;

        using var stream = await _fileSystem.OpenAppPackageFileAsync(sound);
        var player = _audioManager
            .CreatePlayer(stream);
        player.Play();
    }
}
