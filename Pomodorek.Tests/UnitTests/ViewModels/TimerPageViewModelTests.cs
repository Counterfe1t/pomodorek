namespace Pomodorek.Tests.UnitTests.ViewModels;

public class TimerPageViewModelTests
{
    private readonly TimerPageViewModel _viewModel;
    private readonly Mock<ITimerService> _timerServiceMock;
    private readonly Mock<IDateTimeService> _dateTimeServiceMock;
    private readonly Mock<IPermissionsService> _permissionsServiceMock;
    private readonly Mock<ISessionService> _sessionServiceMock;

    public TimerPageViewModelTests()
    {
        _timerServiceMock = new Mock<ITimerService>();
        _dateTimeServiceMock = new Mock<IDateTimeService>();
        _permissionsServiceMock = new Mock<IPermissionsService>();
        _sessionServiceMock = new Mock<ISessionService>();

        _viewModel = new TimerPageViewModel(
            _timerServiceMock.Object,
            _dateTimeServiceMock.Object,
            _permissionsServiceMock.Object,
            _sessionServiceMock.Object);
    }

    [Fact]
    public void Start_TimerIsNotRunning_StartsTimer()
    {
        // arrange
        _viewModel.Session = new Session();

        _dateTimeServiceMock
            .Setup(x => x.UtcNow)
            .Returns(DateTime.Now);
        
        // act
        _viewModel.StartCommand.Execute(null);

        // assert
        _timerServiceMock.Verify(x => x.Start(It.IsAny<Action>()), Times.Once);

        _sessionServiceMock.Verify(x => x.StartInterval(It.IsAny<Session>()), Times.Once);
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

        _sessionServiceMock.Verify(x => x.StartInterval(It.IsAny<Session>()), Times.Never);
    }

    [Fact]
    public void Stop_StopsTimer()
    {
        // arrange
        _dateTimeServiceMock
            .Setup(x => x.UtcNow)
            .Returns(DateTime.Now);

        // act
        _viewModel.StopCommand.Execute(null);

        // assert
        _timerServiceMock.Verify(x => x.Stop(true), Times.Once);
    }
}