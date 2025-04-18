namespace Pomodorek.Models;

public class AppSettings
{
    public string AppName { get; set; }

    public string AppVersion { get; set; }

    public bool DefaultIsDarkThemeEnabled { get; set; }

    public bool DefaultIsSoundEnabled { get; set; }

    public float DefaultSoundVolume { get; set; }
    
    public int DefaultWorkLengthInMin { get; set; }
    
    public int DefaultShortRestLengthInMin { get; set; }
    
    public int DefaultLongRestLengthInMin { get; set; }
}