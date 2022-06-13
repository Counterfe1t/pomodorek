using Pomodorek.Models;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace Pomodorek.Converters
{
    public class TimerModeConverter : IValueConverter
    {
        #region IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var enumValue = (TimerModeEnum)value;
            switch (enumValue)
            {
                case TimerModeEnum.Focus:
                    return Constants.FocusModeLabel;
                case TimerModeEnum.ShortRest:
                    return Constants.ShortRestModeLabel;
                case TimerModeEnum.LongRest:
                    return Constants.LongRestModeLabel;
                case TimerModeEnum.Disabled:
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
