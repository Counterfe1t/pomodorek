namespace Pomodorek.Converters;

public class IntervalConverter : IValueConverter
{
    public object Convert(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture)
        => (IntervalEnum?)value switch
        {
            IntervalEnum.Work => AppResources.TimerPage_WorkIntervalLabel,
            IntervalEnum.ShortRest => AppResources.TimerPage_ShortRestIntervalLabel,
            IntervalEnum.LongRest => AppResources.TimerPage_LongRestIntervalLabel,
            _ => string.Empty,
        };

    public object ConvertBack(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture)
        => throw new NotImplementedException();
}