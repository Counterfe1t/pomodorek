namespace Pomodorek.Tests.UnitTests;

public class NumericValidationBehaviorTests
{
    [Theory]
    [InlineData(null, "")]
    [InlineData("", "")]
    [InlineData("1337", "1337")]
    [InlineData(" 2137 ", "2137")]
    public void OnEntryTextChanged_ReturnsExpectedResult(string received, string expected)
    {
        // arrange
        var sender = new Entry();
        var args = new TextChangedEventArgs(null, received);

        // act
        NumericValidationBehavior.OnEntryTextChanged(sender, args);

        // assert
        Assert.True(sender.Text == expected);
    }
}
