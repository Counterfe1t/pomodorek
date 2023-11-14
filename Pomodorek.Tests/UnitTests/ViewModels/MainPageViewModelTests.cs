namespace Pomodorek.Tests.UnitTests.ViewModels;

public class MainPageViewModelTests
{
    private readonly MainPageViewModel _viewModel;
    private readonly Mock<ITimerService> _timerMock;
    private readonly Mock<INotificationService> _notificationServiceMock;
    private readonly Mock<ISettingsService> _settingsServiceMock;
    private readonly Mock<IConfigurationService> _configurationServiceMock;
    private readonly Mock<ISoundService> _soundServiceMock;
    private readonly Mock<IMessageService> _messageServiceMock;
    private readonly Mock<IDateTimeService> _dateTimeServiceMock;

    public MainPageViewModelTests()
    {
        _timerMock = new Mock<ITimerService>();
        _notificationServiceMock = new Mock<INotificationService>();
        _settingsServiceMock = new Mock<ISettingsService>();
        _configurationServiceMock = new Mock<IConfigurationService>();
        _soundServiceMock = new Mock<ISoundService>();
        _messageServiceMock = new Mock<IMessageService>();
        _dateTimeServiceMock = new Mock<IDateTimeService>();

        _configurationServiceMock
            .Setup(x => x.GetAppSettings())
            .Returns(new AppSettings());

        _viewModel = new MainPageViewModel(
            _timerMock.Object,
            _notificationServiceMock.Object,
            _settingsServiceMock.Object,
            _configurationServiceMock.Object,
            _soundServiceMock.Object,
            _messageServiceMock.Object,
            _dateTimeServiceMock.Object);
    }

    [Fact]
    public async Task DisplayNotification_InvokesNotificationService()
    {
        // arrange
        var message = "foo";

        // act
        await _viewModel.DisplayNotification(message);

        // assert
        _notificationServiceMock.Verify(x => x.DisplayNotificationAsync(message), Times.Once);
    }

    [Fact]
    public async Task PlaySound_InvokesSoundService()
    {
        // arrange
        var fileName = "bar";

        // act
        await _viewModel.PlaySound(fileName);

        // assert
        _soundServiceMock.Verify(x => x.PlaySoundAsync(fileName), Times.Once);
    }

    [Fact]
    public void Start_WhenTimerIsNotRunning_StartsTimer()
    {
        // act
        _viewModel.StartCommand.Execute(null);

        // assert
        _timerMock.Verify(x => x.Start(It.IsAny<Action>()), Times.Once);

        _soundServiceMock.Verify(x => x.PlaySoundAsync(It.IsAny<string>()), Times.Once);
        
        _settingsServiceMock.Verify(x => x.Set(Constants.Settings.SessionsCount, It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public void Start_WhenTimerIsRunning_DoesNotStartTimer()
    {
        // arrange
        _viewModel.IsRunning = true;

        // act
        _viewModel.StartCommand.Execute(null);

        // assert
        _timerMock.Verify(x => x.Start(It.IsAny<Action>()), Times.Never);
        
        _soundServiceMock.Verify(x => x.PlaySoundAsync(It.IsAny<string>()), Times.Never);
        
        _settingsServiceMock.Verify(x => x.Set(Constants.Settings.SessionsCount, It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public void Stop_StopsTimer()
    {
        // act
        _viewModel.StopCommand.Execute(null);

        // assert
        _timerMock.Verify(x => x.Stop(), Times.Once);
    }
}