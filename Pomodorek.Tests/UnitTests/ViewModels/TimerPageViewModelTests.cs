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

        _timerServiceMock.Invocations.Clear();
    }

    [Fact]
    public void Start_StartsTimer()
    {
        // arrange
        _viewModel.Session = BaseSessionService.GetNewSession();

        _dateTimeServiceMock
            .Setup(x => x.UtcNow)
            .Returns(DateTime.Now);
        
        // act
        _viewModel.StartCommand.Execute(null);

        // assert
        Assert.True(_viewModel.IsRunning);

        _timerServiceMock.Verify(x => x.Start(It.IsAny<Action>()), Times.Once);
        _sessionServiceMock
            .Verify(x => x.StartInterval(It.Is<SessionModel>(y => y.CurrentInterval == IntervalEnum.Work)), Times.Once);
    }

    [Fact]
    public void Pause_PausesTimer()
    {
        // act
        _viewModel.PauseCommand.Execute(null);

        // assert
        Assert.True(_viewModel.State == TimerStateEnum.Paused);

        _timerServiceMock.Verify(x => x.Stop(true), Times.Once);
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
        Assert.True(_viewModel.IsStopped);

        _timerServiceMock.Verify(x => x.Stop(true), Times.Once);
    }

    [Fact]
    public void Reset_StopsTimerAndResetsSession()
    {
        // arrange
        var expectedSession = BaseSessionService.GetNewSession();

        // act
        _viewModel.ResetCommand.Execute(null);

        // assert
        Assert.True(_viewModel.IsStopped);
        Assert.Equal(expectedSession.IntervalsCount, _viewModel.Session.IntervalsCount);
        Assert.Equal(expectedSession.WorkIntervalsCount, _viewModel.Session.WorkIntervalsCount);
        Assert.Equal(expectedSession.ShortRestIntervalsCount, _viewModel.Session.ShortRestIntervalsCount);
        Assert.Equal(expectedSession.LongRestIntervalsCount, _viewModel.Session.LongRestIntervalsCount);
        Assert.Equal(expectedSession.TriggerAlarmAt, _viewModel.Session.TriggerAlarmAt);

        _timerServiceMock.Verify(x => x.Stop(true), Times.Once);
    }
}