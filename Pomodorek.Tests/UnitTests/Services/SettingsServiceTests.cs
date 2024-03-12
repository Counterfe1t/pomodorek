namespace Pomodorek.Tests.UnitTests.Services;

public class SettingsServiceTests
{
    private const int Value = 1337;

    private readonly SettingsService _settingsService;
    private readonly Mock<IPreferences> _preferencesMock;

    public SettingsServiceTests()
    {
        _preferencesMock = new Mock<IPreferences>();

        _settingsService = new SettingsService(_preferencesMock.Object);
    }

    [Fact]
    public void Get_GetsSettingsFromStorage()
    {
        // arrange
        _preferencesMock
            .Setup(x => x.Get(Constants.Settings.WorkLengthInMin, Value, null))
            .Returns(Value);

        // act
        var result = _settingsService.Get(Constants.Settings.WorkLengthInMin, Value);

        // assert
        _preferencesMock.Verify(x => x.Get(Constants.Settings.WorkLengthInMin, Value, null), Times.Once);
        Assert.Equal(Value, result);
    }

    [Fact]
    public void Set_SetsSettingsToStorage()
    {
        // act
        _settingsService.Set(Constants.Settings.WorkLengthInMin, Value);

        // assert
        _preferencesMock.Verify(x => x.Set(Constants.Settings.WorkLengthInMin, Value, null), Times.Once);
    }
}