namespace Pomodorek.Tests.UnitTests.Converters;

public class TimeConverterTests
{
    private readonly TimeConverter _converter;

    public TimeConverterTests()
    {
        _converter = new TimeConverter();
    }

    [Theory]
    [InlineData(60, "01:00")]
    [InlineData(90, "01:30")]
    [InlineData(0, "00:00")]
    [InlineData(1337, "22:17")]
    public void Convert_ReturnsExcpectedResult(int received, string expected)
    {
        // act
        var result = _converter.Convert(
            received,
            It.IsAny<Type>(),
            It.IsAny<object>(),
            It.IsAny<CultureInfo>());

        // assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ConvertBack_ThrowsNotImplementedException()
    {
        // assert
        Assert.Throws<NotImplementedException>(() => _converter.ConvertBack(
            It.IsAny<object>(),
            It.IsAny<Type>(),
            It.IsAny<object>(),
            It.IsAny<CultureInfo>()));
    }
}