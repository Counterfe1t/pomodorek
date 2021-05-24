using System;
using System.Globalization;
using Xamarin.Forms;

namespace Pomodorek.Logic {
    public class StartButtonLabelConverter : IValueConverter {

        #region IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            var isTimerPaused = (bool)value;
            return isTimerPaused
                ? "Assets/Images/icon-start18x18.png"
                : "Assets/Images/icon-pause18x18.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }

        #endregion
    }
}
