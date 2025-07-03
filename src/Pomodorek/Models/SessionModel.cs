namespace Pomodorek.Models;

public partial class SessionModel : BindableObject
{
    private IntervalEnum _currentInterval;
    private DateTimeOffset _triggerAlarmAt;
    private int _intervalsCount;
    private int _workIntervalsCount;
    private int _shortRestIntervalsCount;
    private int _longRestIntervalsCount;

    public IntervalEnum CurrentInterval
    {
        get => _currentInterval;
        set
        {
            _currentInterval = value;
            OnPropertyChanged(nameof(CurrentInterval));
        }
    }

    public DateTimeOffset TriggerAlarmAt
    {
        get => _triggerAlarmAt;
        set
        {
            _triggerAlarmAt = value;
            OnPropertyChanged(nameof(TriggerAlarmAt));
        }
    }

    public int IntervalsCount
    {
        get => _intervalsCount;
        set
        {
            _intervalsCount = value;
            OnPropertyChanged(nameof(IntervalsCount));
        }
    }

    public int WorkIntervalsCount
    {
        get => _workIntervalsCount;
        set
        {
            _workIntervalsCount = value;
            OnPropertyChanged(nameof(WorkIntervalsCount));
        }
    }

    public int ShortRestIntervalsCount
    {
        get => _shortRestIntervalsCount;
        set
        {
            _shortRestIntervalsCount = value;
            OnPropertyChanged(nameof(ShortRestIntervalsCount));
        }
    }

    public int LongRestIntervalsCount
    {
        get => _longRestIntervalsCount;
        set
        {
            _longRestIntervalsCount = value;
            OnPropertyChanged(nameof(LongRestIntervalsCount));
        }
    }

    /// <summary>
    /// Long rest is triggered after every 4th work interval.
    /// </summary>
    public bool IsLongRest => WorkIntervalsCount % 4 == 0;

    /// <summary>
    /// Create a new instance of <see cref="SessionModel"/> class.
    /// </summary>
    public static SessionModel Create() => new()
    {
        IntervalsCount = 0,
        WorkIntervalsCount = 0,
        ShortRestIntervalsCount = 0,
        LongRestIntervalsCount = 0,
        CurrentInterval = IntervalEnum.Work
    };
}