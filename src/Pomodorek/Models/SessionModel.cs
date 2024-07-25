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

    // Trigger long rest every 4th work interval
    public bool IsLongRest => WorkIntervalsCount % 4 == 0;
}