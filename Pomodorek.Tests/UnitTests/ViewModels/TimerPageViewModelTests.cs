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

        _sessionServiceMock
            .Setup(x => x.GetSession())
            .Returns(BaseSessionService.GetNewSession);

        _viewModel = new TimerPageViewModel(
            _timerServiceMock.Object,
            _dateTimeServiceMock.Object,
            _permissionsServiceMock.Object,
            _sessionServiceMock.Object);

        _timerServiceMock.Invocations.Clear();
    }

    [Fact]
    public void StartCommand_StartsTimer()
    {
        // act
        _viewModel.StartCommand.Execute(null);

        // assert
        Assert.Equal(TimerStateEnum.Running, _viewModel.State);

        _timerServiceMock.Verify(x => x.Start(It.IsAny<Action>()), Times.Once);
        _sessionServiceMock
            .Verify(x => x.StartInterval(It.Is<SessionModel>(y => y.CurrentInterval == IntervalEnum.Work)), Times.Once);
    }

    [Fact]
    public void PauseCommand_PausesTimer()
    {
        // act
        _viewModel.PauseCommand.Execute(null);

        // assert
        Assert.Equal(TimerStateEnum.Paused, _viewModel.State);

        _timerServiceMock.Verify(x => x.Stop(true), Times.Once);
    }

    [Fact]
    public void StopCommand_TimerIsRunning_StopsTimer()
    {
        // arrange
        _viewModel.State = TimerStateEnum.Running;

        // act
        _viewModel.StopCommand.Execute(null);

        // assert
        Assert.Equal(TimerStateEnum.Stopped, _viewModel.State);

        _timerServiceMock.Verify(x => x.Stop(true), Times.Once);
    }

    [Fact]
    public void StopCommand_TimerIsStopped_DoesNotStopTimer()
    {
        // arrange
        _viewModel.State = TimerStateEnum.Stopped;

        // act
        _viewModel.StopCommand.Execute(null);

        // assert
        Assert.Equal(TimerStateEnum.Stopped, _viewModel.State);

        _timerServiceMock.Verify(x => x.Stop(true), Times.Never);
    }

    [Fact]
    public void ResetCommand_TimerIsRunning_StopsTimerAndResetsSession()
    {
        // arrange
        var expectedSession = BaseSessionService.GetNewSession;

        _viewModel.State = TimerStateEnum.Running;

        // act
        _viewModel.ResetCommand.Execute(null);

        // assert
        Assert.Equal(TimerStateEnum.Stopped, _viewModel.State);
        Assert.Equal(expectedSession.IntervalsCount, _viewModel.Session.IntervalsCount);
        Assert.Equal(expectedSession.WorkIntervalsCount, _viewModel.Session.WorkIntervalsCount);
        Assert.Equal(expectedSession.ShortRestIntervalsCount, _viewModel.Session.ShortRestIntervalsCount);
        Assert.Equal(expectedSession.LongRestIntervalsCount, _viewModel.Session.LongRestIntervalsCount);
        Assert.Equal(expectedSession.TriggerAlarmAt, _viewModel.Session.TriggerAlarmAt);

        _timerServiceMock.Verify(x => x.Stop(true), Times.Once);
    }

    [Fact]
    public void ResetCommand_TimerIsStopped_DoesNotStopTimerAndResetsSession()
    {
        // arrange
        var expectedSession = BaseSessionService.GetNewSession;

        _viewModel.State = TimerStateEnum.Stopped;

        // act
        _viewModel.ResetCommand.Execute(null);

        // assert
        Assert.Equal(TimerStateEnum.Stopped, _viewModel.State);
        Assert.Equal(expectedSession.IntervalsCount, _viewModel.Session.IntervalsCount);
        Assert.Equal(expectedSession.WorkIntervalsCount, _viewModel.Session.WorkIntervalsCount);
        Assert.Equal(expectedSession.ShortRestIntervalsCount, _viewModel.Session.ShortRestIntervalsCount);
        Assert.Equal(expectedSession.LongRestIntervalsCount, _viewModel.Session.LongRestIntervalsCount);
        Assert.Equal(expectedSession.TriggerAlarmAt, _viewModel.Session.TriggerAlarmAt);

        _timerServiceMock.Verify(x => x.Stop(true), Times.Never);
    }
}