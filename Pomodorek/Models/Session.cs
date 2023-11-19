namespace Pomodorek.Models;

public class Session : ObservableObject
{
    private IntervalEnum _currentInterval;
    public IntervalEnum CurrentInterval
    {
        get => _currentInterval;
        set => SetProperty(ref _currentInterval, value);
    }

    private int _intervalsCount;
    public int IntervalsCount
    {
        get => _intervalsCount;
        set => SetProperty(ref _intervalsCount, value);
    }

    private int _workIntervalsCount;
    public int WorkIntervalsCount
    {
        get => _workIntervalsCount;
        set => SetProperty(ref _workIntervalsCount, value);
    }

    private int _shortRestIntervalsCount;
    public int ShortRestIntervalsCount
    {
        get => _shortRestIntervalsCount;
        set => SetProperty(ref _shortRestIntervalsCount, value);
    }

    private int _longRestIntervalsCount;
    public int LongRestIntervalsCount
    {
        get => _longRestIntervalsCount;
        set => SetProperty(ref _longRestIntervalsCount, value);
    }

    public DateTime TriggerAlarmAt { get; set; }

    public bool IsLongRest => WorkIntervalsCount % 4 == 0;
}