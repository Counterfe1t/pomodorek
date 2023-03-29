namespace Pomodorek.Models;

public class AppSettings
{
    public bool DefaultIsSoundEnabled { get; set; }
    public float DefaultSoundVolume { get; set; }
    public int DefaultFocusLengthInMin { get; set; }
    public int DefaultShortRestLengthInMin { get; set; }
    public int DefaultLongRestLengthInMin { get; set; }
}
