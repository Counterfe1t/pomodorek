namespace Pomodorek.Converters;

public class AlarmConverter : IValueConverter
{
    public object Convert(
        object value,
        Type targetType,
        object parameter,
        CultureInfo culture) => FormatAlarm((DateTime)value);

    public object ConvertBack(
        object value,
        Type targetType,
        object parameter,
        CultureInfo culture) => throw new NotImplementedException();

    public static string FormatAlarm(DateTime alarm)
        => alarm.ToLocalTime().ToString("HH:mm");
}