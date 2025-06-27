namespace Pomodorek.Behaviors;

public partial class NumericValidationBehavior : Behavior<Entry>
{
    /// <inheritdoc />
    protected override void OnAttachedTo(Entry entry)
    {
        entry.TextChanged += OnEntryTextChanged;
        base.OnAttachedTo(entry);
    }

    /// <inheritdoc />
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

        var isNumber = int.TryParse(args.NewTextValue, out int value);

        entry.Text = isNumber && value > 0
            ? args.NewTextValue.Trim()
            : args.OldTextValue;
    }
}
