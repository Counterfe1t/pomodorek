namespace Pomodorek.Tests.UnitTests;

public class StringParserTests
{
    [Fact]
    public void ParseString_ShouldProperlyParseStringContent()
    {
        // arrange
        var value = "foo";
        var expectedResult = $"This is a {value}.";

        // act
        var result = StringParser.Parse(value, "This is a {{value}}.");

        // assert
        Assert.Equal(expectedResult, result);
    }
}