namespace Pomodorek.Tests.UnitTests;

public class NumericValidationBehaviorTests
{
    [Theory]
    [MemberData(nameof(OnEntryTextChangedTestData))]
    public void OnEntryTextChanged_ShouldSetExpectedTextValue(string received, string expected)
    {
        // arrange
        var entry = new Entry();
        var args = new TextChangedEventArgs(null, received);

        // act
        NumericValidationBehavior.OnEntryTextChanged(entry, args);

        // assert
        Assert.Equal(expected, entry.Text);
    }

    public static TheoryData<string?, string?> OnEntryTextChangedTestData =>
        new()
        {
            { null, "" },
            { "", "" },
            { "foo", null },
            { "1337", "1337" },
            { " 2137 ", "2137" }
        };
}