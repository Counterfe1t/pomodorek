namespace Pomodorek.Models;

public partial class SessionModel : ObservableObject
{
    [ObservableProperty]
    private IntervalEnum _currentInterval;
    
    [ObservableProperty]
    private DateTime _triggerAlarmAt;

    [ObservableProperty]
    private int _intervalsCount;

    [ObservableProperty]
    private int _workIntervalsCount;
    
    [ObservableProperty]
    private int _shortRestIntervalsCount;

    [ObservableProperty]
    private int _longRestIntervalsCount;

    /// <summary>
    /// Long rest is triggered after every 4th work interval.
    /// </summary>
    public bool IsLongRest => WorkIntervalsCount % 4 == 0;

    public static SessionModel Create() => new()
    {
        IntervalsCount = 0,
        WorkIntervalsCount = 0,
        ShortRestIntervalsCount = 0,
        LongRestIntervalsCount = 0,
        CurrentInterval = IntervalEnum.Work
    };
}