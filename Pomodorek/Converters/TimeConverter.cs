namespace Pomodorek.Converters;

public class TimeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var seconds = (int)value;
        var minutes = seconds / 60;
        return $"{(minutes < 10 ? "0" : "")}{minutes}:{((seconds % 60) < 10 ? "0" : "")}{seconds % 60}";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}