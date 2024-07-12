namespace Pomodorek.Tests.UnitTests.Converters;

public class IntervalConverterTests
{
    private readonly IntervalConverter _converter;

    public IntervalConverterTests()
    {
        _converter = new IntervalConverter();
    }

    [Theory]
    [InlineData(IntervalEnum.Work, Constants.Labels.Work)]
    [InlineData(IntervalEnum.ShortRest, Constants.Labels.ShortRest)]
    [InlineData(IntervalEnum.LongRest, Constants.Labels.LongRest)]
    [InlineData((IntervalEnum)1337, "")]
    public void Convert_ReturnsExcpectedResult(
        IntervalEnum received,
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