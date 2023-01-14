using Pomodorek.Resources.Constants;
using Pomodorek.Resources.Enums;
using System.Globalization;

namespace Pomodorek.Converters
{
    public class TimerStatusConverter : IValueConverter
    {
        #region IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var enumValue = (TimerStatusEnum)value;
            switch (enumValue)
            {
                case TimerStatusEnum.Focus:
                    return Constants.FocusModeLabel;
                case TimerStatusEnum.ShortRest:
                    return Constants.ShortRestModeLabel;
                case TimerStatusEnum.LongRest:
                    return Constants.LongRestModeLabel;
                case TimerStatusEnum.Disabled:
                default:
                    return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
