namespace Pomodorek.Converters;

public class IntervalConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        (IntervalEnum)value switch
        {
            IntervalEnum.Work => Constants.Labels.Work,
            IntervalEnum.ShortRest => Constants.Labels.ShortRest,
            IntervalEnum.LongRest => Constants.Labels.LongRest,
            _ => string.Empty,
        };

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}