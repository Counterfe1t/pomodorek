using Plugin.Maui.Audio;

namespace Pomodorek.Services;

public class SoundService : ISoundService
{
    private readonly IAudioManager _audioManager;

    public SoundService(IAudioManager audioManager)
    {
        _audioManager = audioManager;
    }

    public async Task PlaySound(string sound)
    {
        using var stream = await FileSystem.OpenAppPackageFileAsync(sound);
        var player = _audioManager.CreatePlayer(stream);
        player.Play();
    }
}
