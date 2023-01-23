using Pomodorek.Resources.Constants;
using Pomodorek.Resources.Enums;
using System.Globalization;

namespace Pomodorek.Converters;

public class TimerStatusConverter : IValueConverter
{
    #region IValueConverter

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)

        => (TimerStatusEnum)value switch
        {
            TimerStatusEnum.Focus => Constants.FocusModeLabel,
            TimerStatusEnum.ShortRest => Constants.ShortRestModeLabel,
            TimerStatusEnum.LongRest => Constants.LongRestModeLabel,
            _ => string.Empty,
        };

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    #endregion
}