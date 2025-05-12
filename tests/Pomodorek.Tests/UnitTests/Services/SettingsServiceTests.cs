namespace Pomodorek.Tests.UnitTests.Services;

public class SettingsServiceTests
{
    private const int Value = 1337;

    private readonly SettingsService _cut;

    private readonly Mock<IPreferences> _preferencesMock;

    public SettingsServiceTests()
    {
        _preferencesMock = new();

        _cut = ClassUnderTest.Is<SettingsService>(_preferencesMock.Object);
    }

    [Fact]
    public void Get_ShouldGetSettingsFromStorage()
    {
        // arrange
        _preferencesMock
            .Setup(x => x.Get(Constants.Settings.WorkLengthInMin, Value, null))
            .Returns(Value);

        // act
        var result = _cut.Get(Constants.Settings.WorkLengthInMin, Value);

        // assert
        Assert.Equal(Value, result);

        _preferencesMock.Verify(x => x.Get(Constants.Settings.WorkLengthInMin, Value, null), Times.Once);
    }

    [Fact]
    public void Set_ShouldSetSettingsToStorage()
    {
        // act
        _cut.Set(Constants.Settings.WorkLengthInMin, Value);

        // assert
        _preferencesMock.Verify(x => x.Set(Constants.Settings.WorkLengthInMin, Value, null), Times.Once);
    }
}