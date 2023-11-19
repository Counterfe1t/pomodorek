using Pomodorek.Interfaces;

namespace Pomodorek.Tests.UnitTests.ViewModels;

public class MainPageViewModelTests
{
    private readonly MainPageViewModel _viewModel;
    private readonly Mock<ITimerService> _timerServiceMock;
    private readonly Mock<ISettingsService> _settingsServiceMock;
    private readonly Mock<IDateTimeService> _dateTimeServiceMock;
    private readonly Mock<IPermissionsService> _permissionsServiceMock;
    private readonly Mock<ISessionService> _sessionServiceMock;

    public MainPageViewModelTests()
    {
        _timerServiceMock = new Mock<ITimerService>();
        _settingsServiceMock = new Mock<ISettingsService>();
        _dateTimeServiceMock = new Mock<IDateTimeService>();
        _permissionsServiceMock = new Mock<IPermissionsService>();
        _sessionServiceMock = new Mock<ISessionService>();

        _viewModel = new MainPageViewModel(
            _timerServiceMock.Object,
            _settingsServiceMock.Object,
            _dateTimeServiceMock.Object,
            _permissionsServiceMock.Object,
            _sessionServiceMock.Object);
    }

    [Fact]
    public void Start_TimerIsNotRunning_StartsTimer()
    {
        // act
        _viewModel.StartCommand.Execute(null);

        // assert
        _timerServiceMock.Verify(x => x.Start(It.IsAny<Action>()), Times.Once);
    }

    [Fact]
    public void Start_TimerIsRunning_DoesNotStartTimer()
    {
        // arrange
        _viewModel.State = TimerStateEnum.Running;

        // act
        _viewModel.StartCommand.Execute(null);

        // assert
        _timerServiceMock.Verify(x => x.Start(It.IsAny<Action>()), Times.Never);
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