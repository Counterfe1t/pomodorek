namespace Pomodorek.Behaviors;

public class NumericValidationBehavior : Behavior<Entry>
{
    protected override void OnAttachedTo(Entry entry)
    {
        entry.TextChanged += OnEntryTextChanged;
        base.OnAttachedTo(entry);
    }

    protected override void OnDetachingFrom(Entry entry)
    {
        entry.TextChanged -= OnEntryTextChanged;
        base.OnDetachingFrom(entry);
    }

    public static void OnEntryTextChanged(object? sender, TextChangedEventArgs args)
    {
        if (sender is not Entry entry)
            return;

        if (string.IsNullOrWhiteSpace(args.NewTextValue))
        {
            entry.Text = string.Empty;
            return;
        }

        var isDigit = int.TryParse(args.NewTextValue, out int value);

        entry.Text = isDigit && value > 0
            ? args.NewTextValue.Trim()
            : args.OldTextValue;
    }
}
