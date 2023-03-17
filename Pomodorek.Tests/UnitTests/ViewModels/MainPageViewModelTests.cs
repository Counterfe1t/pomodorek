namespace Pomodorek.Tests.UnitTests.ViewModels;

public class MainPageViewModelTests
{
    private readonly MainPageViewModel _viewModel;
    private readonly Mock<ITimerService> _timerMock;
    private readonly Mock<INotificationService> _notificationServiceMock;
    private readonly Mock<ISettingsService> _settingsServiceMock;
    private readonly Mock<IConfigurationService> _configurationServiceMock;
    private readonly Mock<ISoundService> _soundServiceMock;

    private static AppSettings AppSettings => new();

    public MainPageViewModelTests()
    {
        _timerMock = new Mock<ITimerService>();
        _notificationServiceMock = new Mock<INotificationService>();
        _settingsServiceMock = new Mock<ISettingsService>();
        _configurationServiceMock = new Mock<IConfigurationService>();
        _soundServiceMock = new Mock<ISoundService>();

        _viewModel = new MainPageViewModel(
            _timerMock.Object,
            _notificationServiceMock.Object,
            _settingsServiceMock.Object,
            _configurationServiceMock.Object,
            _soundServiceMock.Object);
    }

    [Fact]
    public async Task DisplayNotification_WhenCalled_InvokesNotificationService()
    {
        // arrange
        var message = "foo";

        // act
        await _viewModel.DisplayNotification(message);

        // assert
        _notificationServiceMock.Verify(x => x.DisplayNotification(message), Times.Once);
    }

    [Fact]
    public void StartSession_WhenTimerIsNotRunning_StartsTimer()
    {
        // arrange
        _configurationServiceMock
            .Setup(x => x.GetAppSettings())
            .Returns(AppSettings);

        // act
        _viewModel.StartCommand.Execute(null);

        // assert
        _timerMock.Verify(x => x.Start(It.IsAny<Action>()), Times.Once);
        _soundServiceMock.Verify(x => x.PlaySound(It.IsAny<string>()), Times.Once);
        _settingsServiceMock.Verify(x => x.Set(Constants.Settings.NumberOfSessions, It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public void StartSession_WhenTimerIsRunning_DoesNotStartTimer()
    {
        // arrange
        _viewModel.IsRunning = true;

        // act
        _viewModel.StartCommand.Execute(null);

        // assert
        _timerMock.Verify(x => x.Start(It.IsAny<Action>()), Times.Never);
        _soundServiceMock.Verify(x => x.PlaySound(It.IsAny<string>()), Times.Never);
        _settingsServiceMock.Verify(x => x.Set(Constants.Settings.NumberOfSessions, It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public void StopSession_WhenCalled_StopsTimer()
    {
        // act
        _viewModel.StopCommand.Execute(null);

        // assert
        _timerMock.Verify(x => x.Stop(), Times.Once);
    }
}