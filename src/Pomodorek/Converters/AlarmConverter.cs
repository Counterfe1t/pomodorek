namespace Pomodorek.Converters;

public class AlarmConverter : IValueConverter
{
    public object Convert(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture)
        => value is not null ? FormatValue((DateTimeOffset)value) : string.Empty;

    public object ConvertBack(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture)
        => throw new NotImplementedException();

    public static string FormatValue(DateTimeOffset value)
        => value.ToLocalTime().ToString("HH:mm");
}