namespace Pomodorek.Tests.UnitTests.ViewModels;

public class AboutPageViewModelTests
{
    private readonly AboutPageViewModel _viewModel;

    private readonly Mock<INavigationService> _navigationServiceMock;
    private readonly Mock<IConfigurationService> _configurationServiceMock;
    private readonly Mock<IBrowser> _browserMock;

    private AppSettings _appSettings => new()
    {
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
            .Returns(_appSettings);

        _viewModel = new(
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
        await _viewModel.GoToUrlCommand.ExecuteAsync(url);

        // assert
        _browserMock
            .Verify(x => x.OpenAsync(It.Is<Uri>(y => y.AbsoluteUri == url), It.IsAny<BrowserLaunchOptions>()), Times.Once);
    }
}