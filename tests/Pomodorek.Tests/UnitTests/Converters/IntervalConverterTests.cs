namespace Pomodorek.Tests.UnitTests.Converters;

public class IntervalConverterTests
{
    private readonly IntervalConverter _cut;

    public IntervalConverterTests()
    {
        _cut = ClassUnderTest.Is<IntervalConverter>();
    }

    [Theory]
    [InlineData(IntervalEnum.Work, Constants.Labels.Work)]
    [InlineData(IntervalEnum.ShortRest, Constants.Labels.ShortRest)]
    [InlineData(IntervalEnum.LongRest, Constants.Labels.LongRest)]
    [InlineData((IntervalEnum)1337, "")]
    public void Convert_ShouldReturnExcpectedResult(
        IntervalEnum received,
        string expected)
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