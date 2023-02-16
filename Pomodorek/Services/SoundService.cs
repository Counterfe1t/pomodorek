using Plugin.Maui.Audio;

namespace Pomodorek.Services;

public class SoundService : ISoundService
{
    private readonly IAudioManager _audioManager;
    private readonly IFileSystem _fileSystem;

    public SoundService(IAudioManager audioManager, IFileSystem fileSystem)
    {
        _audioManager = audioManager;
        _fileSystem = fileSystem;
    }

    public async Task PlaySound(string sound)
    {
        using var stream = await _fileSystem.OpenAppPackageFileAsync(sound);
        var foo = _audioManager
            .CreatePlayer(stream);
        foo.Play();
    }
}
