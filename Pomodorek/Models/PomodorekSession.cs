namespace Pomodorek.Models;

public class PomodorekSession : ObservableObject
{
    private IntervalEnum _interval;
    public IntervalEnum Interval
    {
        get => _interval;
        set => SetProperty(ref _interval, value);
    }

    private int _intervalsCount;
    public int IntervalsCount
    {
        get => _intervalsCount;
        set => SetProperty(ref _intervalsCount, value);
    }

    public int IntervalsElapsed { get; set; }

    public bool IsLongRest => IntervalsElapsed % 4 == 0;

    public bool IsFinished => IntervalsElapsed >= IntervalsCount;
}