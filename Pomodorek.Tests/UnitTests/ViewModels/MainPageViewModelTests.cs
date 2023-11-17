namespace Pomodorek.Tests.UnitTests.ViewModels;

public class MainPageViewModelTests
{
    private readonly MainPageViewModel _viewModel;
    private readonly Mock<ITimerService> _timerServiceMock;
    private readonly Mock<INotificationService> _notificationServiceMock;
    private readonly Mock<ISettingsService> _settingsServiceMock;
    private readonly Mock<IConfigurationService> _configurationServiceMock;
    private readonly Mock<ISoundService> _soundServiceMock;
    private readonly Mock<IDateTimeService> _dateTimeServiceMock;
    private readonly Mock<IPermissionsService> _permissionsServiceMock;

    public MainPageViewModelTests()
    {
        _timerServiceMock = new Mock<ITimerService>();
        _notificationServiceMock = new Mock<INotificationService>();
        _settingsServiceMock = new Mock<ISettingsService>();
        _configurationServiceMock = new Mock<IConfigurationService>();
        _soundServiceMock = new Mock<ISoundService>();
        _dateTimeServiceMock = new Mock<IDateTimeService>();
        _permissionsServiceMock = new Mock<IPermissionsService>();

        _configurationServiceMock
            .Setup(x => x.GetAppSettings())
            .Returns(new AppSettings());

        _viewModel = new MainPageViewModel(
            _timerServiceMock.Object,
            _notificationServiceMock.Object,
            _settingsServiceMock.Object,
            _configurationServiceMock.Object,
            _soundServiceMock.Object,
            _dateTimeServiceMock.Object,
            _permissionsServiceMock.Object);
    }

    //[Fact]
    //public async Task DisplayNotification_InvokesNotificationService()
    //{
    //    // act
    //    await _viewModel.DisplayNotification(string.Empty);
    //    // assert
    //    _notificationServiceMock.Verify(x => x.DisplayNotificationAsync(It.IsAny<NotificationDto>()), Times.Once);
    //}

    [Fact]
    public async Task PlaySound_InvokesSoundService()
    {
        // arrange
        var fileName = "sound.wav";

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
        _timerServiceMock.Verify(x => x.Start(It.IsAny<Action>()), Times.Once);

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
        _timerServiceMock.Verify(x => x.Start(It.IsAny<Action>()), Times.Never);
        
        _soundServiceMock.Verify(x => x.PlaySoundAsync(It.IsAny<string>()), Times.Never);
        
        _settingsServiceMock.Verify(x => x.Set(Constants.Settings.SessionsCount, It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public void Stop_StopsTimer()
    {
        // act
        _viewModel.StopCommand.Execute(null);

        // assert
        _timerServiceMock.Verify(x => x.Stop(), Times.Once);
    }
}