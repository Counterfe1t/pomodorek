namespace Pomodorek.Tests.UnitTests.Converters;

public class IntervalConverterTests
{
    private readonly IntervalConverter _cut;

    public IntervalConverterTests()
    {
        _cut = ClassUnderTest.Is<IntervalConverter>();
    }

    [Theory]
    [MemberData(nameof(ConvertData))]
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

    public static TheoryData<IntervalEnum, string> ConvertData =>
        new()
        {
            { IntervalEnum.Work, AppResources.TimerPage_WorkIntervalLabel },
            { IntervalEnum.ShortRest, AppResources.TimerPage_ShortRestIntervalLabel },
            { IntervalEnum.LongRest, AppResources.TimerPage_LongRestIntervalLabel },
            { (IntervalEnum)1337, "" }
        };
}