using System;
using System.Globalization;
using Xamarin.Forms;

namespace Pomodorek.Logic {
    public class NumberToStringConverter : IValueConverter {

        #region IValueConverter
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            var number = (int)value;
            return number < 10
                ? $"0{number}"
                : $"{number}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
        #endregion
    }
}
