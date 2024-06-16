namespace Pomodorek.Services;

public interface ISoundService
{
    /// <summary>
    /// Play sound asynchronously.
    /// </summary>
    /// <param name="fileName">The name of the .wav file.</param>
    Task PlaySoundAsync(string fileName);
}
