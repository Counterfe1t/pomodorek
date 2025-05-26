namespace Pomodorek.Converters;

public class TimeConverter : IValueConverter
{
    public object Convert(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture)
        => value is not null ? FormatTime((int)value) : string.Empty;

    public object ConvertBack(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture)
        => throw new NotImplementedException();

    public static string FormatTime(int seconds)
        => TimeSpan.FromSeconds(seconds).ToString(@"mm\:ss");
}