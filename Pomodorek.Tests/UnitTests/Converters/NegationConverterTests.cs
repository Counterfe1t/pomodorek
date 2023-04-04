namespace Pomodorek.Tests.UnitTests.Converters;

public class NegationConverterTests
{
    private readonly NegationConverter _converter;

    public NegationConverterTests()
    {
        _converter = new NegationConverter();
    }

    [Theory]
    [InlineData(null, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    public void Convert_ReturnsExcpectedResult(bool? received, bool expected)
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
