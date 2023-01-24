namespace Pomodorek.Tests.UnitTests.Converters;

public class TimerStatusConverterTests
{
    private readonly TimerStatusConverter _converter;

    public TimerStatusConverterTests()
    {
        _converter = new TimerStatusConverter();
    }

    [Theory]
    [InlineData(TimerStatusEnum.Stopped, Constants.StoppedLabel)]
    [InlineData(TimerStatusEnum.Focus, Constants.FocusLabel)]
    [InlineData(TimerStatusEnum.ShortRest, Constants.ShortRestLabel)]
    [InlineData(TimerStatusEnum.LongRest, Constants.LongRestLabel)]
    [InlineData((TimerStatusEnum)1337, "")]
    public void Convert_ReturnsExcpectedResult(
        TimerStatusEnum received,
        string expected)
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
