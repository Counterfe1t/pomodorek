namespace Pomodorek.Tests.UnitTests.Converters;

public class TimeConverterTests
{
    private readonly TimeConverter _cut;

    public TimeConverterTests()
    {
        _cut = ClassUnderTest.Is<TimeConverter>();
    }

    [Theory]
    [InlineData(60, "01:00")]
    [InlineData(90, "01:30")]
    [InlineData(0, "00:00")]
    [InlineData(1337, "22:17")]
    public void Convert_ShouldReturnExcpectedResult(int received, string expected)
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