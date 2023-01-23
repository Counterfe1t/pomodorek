using System.Globalization;

namespace Pomodorek.Converters;

public class NegationConverter : IValueConverter
{
    #region IValueConverter

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => !(bool)value;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    #endregion
}