namespace Pomodorek.Services;

public interface ISoundService
{
    Task PlaySoundAsync(string fileName);
}
