namespace Pomodorek.Tests.UnitTests.Converters;

public class NegationConverterTests
{
    private readonly NegationConverter _cut;

    public NegationConverterTests()
    {
        _cut = ClassUnderTest.Is<NegationConverter>();
    }

    [Theory]
    [InlineData(null, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    public void Convert_ShouldReturnExcpectedResult(bool? received, bool expected)
    {
        // act
        var result = _cut.Convert(
            received,
            It.IsAny<Type>(),
            It.IsAny<object>(),
            It.IsAny<CultureInfo>());

        // assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ConvertBack_ShouldThrowNotImplementedException()
    {
        // assert
        Assert.Throws<NotImplementedException>(() => _cut.ConvertBack(
            It.IsAny<object>(),
            It.IsAny<Type>(),
            It.IsAny<object>(),
            It.IsAny<CultureInfo>()));
    }
}