using System.Linq;
using Xamarin.Forms;

namespace Pomodorek.Converters
{
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

        private static void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            if (string.IsNullOrWhiteSpace(args.NewTextValue))
                return;

            var isValid = args.NewTextValue
                .ToCharArray()
                .All(x => char.IsDigit(x));

            var isNotZero =
                isValid && int.Parse(args.NewTextValue) != 0;

            ((Entry)sender).Text = isValid && isNotZero
                ? args.NewTextValue
                : args.NewTextValue.Remove(args.NewTextValue.Length - 1);
        }
    }
}
