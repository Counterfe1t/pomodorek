using Pomodorek.Models;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace Pomodorek.Impl {
    public class TimerModeEnumToStringConverter : IValueConverter {

        #region IValueConverter
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            var enumValue = (TimerModeEnum)value;
            switch (enumValue) {
                case TimerModeEnum.Focus:
                    return Consts.FocusModeLabel;
                case TimerModeEnum.Rest:
                    return Consts.RestModeLabel;
                case TimerModeEnum.LongRest:
                    return Consts.LongRestModeLabel;
                case TimerModeEnum.Disabled:
                default:
                    return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
        #endregion
    }
}
