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

    public static void OnEntryTextChanged(object sender, TextChangedEventArgs args)
    {
        if (string.IsNullOrWhiteSpace(args.NewTextValue))
        {
            ((Entry)sender).Text = string.Empty;
            return;
        }

        var isDigit = int.TryParse(args.NewTextValue, out int value);

        ((Entry)sender).Text = isDigit && value > 0
            ? args.NewTextValue.Trim()
            : args.OldTextValue;
    }
}          