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
    public void Get_WhenCalled_GetsSettingsFromStorage()
    {
        // arrange
        _preferencesMock
            .Setup(x => x.Get(Constants.FocusLengthInMin, Value, null))
            .Returns(Value);

        // act
        var result = _settingsService.Get(Constants.FocusLengthInMin, Value);

        // assert
        _preferencesMock.Verify(x => x.Get(Constants.FocusLengthInMin, Value, null), Times.Once);
        Assert.Equal(Value, result);
    }

    [Fact]
    public void Set_WhenCalled_SetsSettingsToStorage()
    {
        // act
        _settingsService.Set(Constants.FocusLengthInMin, Value);

        // assert
        _preferencesMock.Verify(x => x.Set(Constants.FocusLengthInMin, Value, null), Times.Once);
    }
}
