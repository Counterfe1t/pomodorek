namespace Pomodorek.Tests.UnitTests.ViewModels;

public class AboutPageViewModelTests
{
    private readonly AboutPageViewModel _cut;

    private readonly Mock<INavigationService> _navigationServiceMock;
    private readonly Mock<IConfigurationService> _configurationServiceMock;
    private readonly Mock<IBrowser> _browserMock;

    private static AppSettings AppSettings => new()
    {
        AppName = "Pomodorek",
        AppVersion = "1.0.0",
        DefaultIsDarkThemeEnabled = true,
        DefaultIsSoundEnabled = true,
        DefaultSoundVolume = 1,
        DefaultWorkLengthInMin = 3,
        DefaultShortRestLengthInMin = 3,
        DefaultLongRestLengthInMin = 7
    };

    public AboutPageViewModelTests()
    {
        _configurationServiceMock = new();
        _navigationServiceMock = new();
        _browserMock = new();

        _configurationServiceMock
            .Setup(x => x.AppSettings)
            .Returns(AppSettings);

        _cut = ClassUnderTest.Is<AboutPageViewModel>(
            _configurationServiceMock.Object,
            _navigationServiceMock.Object,
            _browserMock.Object);
    }

    [Fact]
    public async Task GoToUrl_ShouldOpenUrlInBrowser()
    {
        // arrange
        var url = "https://foo.bar/";

        // act
        await _cut.GoToUrlCommand.ExecuteAsync(url);

        // assert
        _browserMock.Verify(
            x => x.OpenAsync(It.Is<Uri>(y => y.AbsoluteUri == url), It.IsAny<BrowserLaunchOptions>()),
            Times.Once);
    }
}