namespace Pomodorek.Models;

public class Session : ObservableObject
{
    private IntervalEnum _currentInterval;
    private int _intervalsCount;
    private int _workIntervalsCount;
    private int _shortRestIntervalsCount;
    private int _longRestIntervalsCount;

    public IntervalEnum CurrentInterval
    {
        get => _currentInterval;
        set => SetProperty(ref _currentInterval, value);
    }

    public int IntervalsCount
    {
        get => _intervalsCount;
        set => SetProperty(ref _intervalsCount, value);
    }

    public int WorkIntervalsCount
    {
        get => _workIntervalsCount;
        set => SetProperty(ref _workIntervalsCount, value);
    }

    public int ShortRestIntervalsCount
    {
        get => _shortRestIntervalsCount;
        set => SetProperty(ref _shortRestIntervalsCount, value);
    }

    public int LongRestIntervalsCount
    {
        get => _longRestIntervalsCount;
        set => SetProperty(ref _longRestIntervalsCount, value);
    }

    public DateTime TriggerAlarmAt { get; set; }

    public bool IsLongRest => WorkIntervalsCount % 4 == 0;
}