namespace Pomodorek.Converters;

public class TimerStatusConverter : IValueConverter
{
    #region IValueConverter

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        (TimerStatusEnum)value switch
        {
            TimerStatusEnum.Stopped => Constants.Labels.Stopped,
            TimerStatusEnum.Focus => Constants.Labels.Focus,
            TimerStatusEnum.ShortRest => Constants.Labels.ShortRest,
            TimerStatusEnum.LongRest => Constants.Labels.LongRest,
            _ => string.Empty,
        };

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        throw new NotImplementedException();

    #endregion
}