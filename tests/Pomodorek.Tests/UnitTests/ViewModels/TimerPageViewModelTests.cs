namespace Pomodorek.Tests.UnitTests.ViewModels;

public class TimerPageViewModelTests
{
    private readonly TimerPageViewModel _cut;

    private readonly Mock<ITimerService> _timerServiceMock;
    private readonly Mock<ITimeProvider> _timeProviderMock;
    private readonly Mock<IPermissionsService> _permissionsServiceMock;
    private readonly Mock<ISessionService> _sessionServiceMock;
    private readonly Mock<IPopupService> _popupServiceMock;
    private readonly Mock<IAlertService> _alertServiceMock;
    private readonly Mock<INavigationService> _navigationServiceMock;

    public TimerPageViewModelTests()
    {
        _timerServiceMock = new();
        _timeProviderMock = new();
        _permissionsServiceMock = new();
        _sessionServiceMock = new();
        _popupServiceMock = new();
        _alertServiceMock = new();
        _navigationServiceMock = new();

        _sessionServiceMock
            .Setup(x => x.GetSession())
            .Returns(SessionModel.Create());

        _cut = ClassUnderTest.Is<TimerPageViewModel>(
            _timerServiceMock.Object,
            _timeProviderMock.Object,
            _permissionsServiceMock.Object,
            _sessionServiceMock.Object,
            _popupServiceMock.Object,
            _alertServiceMock.Object,
            _navigationServiceMock.Object);

        _timerServiceMock.Invocations.Clear();
    }

    [Fact]
    public void StartCommand_ShouldStartTimer()
    {
        // act
        _cut.StartCommand.Execute(null);

        // assert
        Assert.Equal(TimerStateEnum.Running, _cut.State);

        _timerServiceMock.Verify(
            x => x.Start(It.IsAny<Action>()),
            Times.Once);
        _sessionServiceMock.Verify(
            x => x.StartInterval(It.Is<SessionModel>(y => y.CurrentInterval == IntervalEnum.Work)),
            Times.Once);
    }

    [Fact]
    public void PauseCommand_ShouldPauseTimer()
    {
        // act
        _cut.PauseCommand.Execute(null);

        // assert
        Assert.Equal(TimerStateEnum.Paused, _cut.State);

        _timerServiceMock.Verify(x => x.Stop(true), Times.Once);
    }

    [Fact]
    public void StopCommand_TimerIsRunning_ShouldStopTimer()
    {
        // arrange
        _cut.State = TimerStateEnum.Running;

        // act
        _cut.StopCommand.Execute(null);

        // assert
        Assert.Equal(TimerStateEnum.Stopped, _cut.State);

        _timerServiceMock.Verify(x => x.Stop(true), Times.Once);
    }

    [Fact]
    public void StopCommand_TimerIsStopped_ShouldNotStopTimer()
    {
        // arrange
        _cut.State = TimerStateEnum.Stopped;

        // act
        _cut.StopCommand.Execute(null);

        // assert
        Assert.Equal(TimerStateEnum.Stopped, _cut.State);

        _timerServiceMock.Verify(x => x.Stop(true), Times.Never);
    }

    [Fact]
    public void ResetCommand_TimerIsRunning_ShouldStopTimerAndShouldResetSession()
    {
        // arrange
        var expectedSession = SessionModel.Create();

        _cut.State = TimerStateEnum.Running;

        _alertServiceMock
            .Setup(x => x.DisplayConfirmAsync(_cut.Title, Constants.Messages.ResetSession))
            .ReturnsAsync(true);

        // act
        _cut.ResetCommand.Execute(null);

        // assert
        Assert.Equal(TimerStateEnum.Stopped, _cut.State);
        Assert.Equal(expectedSession.IntervalsCount, _cut.Session.IntervalsCount);
        Assert.Equal(expectedSession.WorkIntervalsCount, _cut.Session.WorkIntervalsCount);
        Assert.Equal(expectedSession.ShortRestIntervalsCount, _cut.Session.ShortRestIntervalsCount);
        Assert.Equal(expectedSession.LongRestIntervalsCount, _cut.Session.LongRestIntervalsCount);
        Assert.Equal(expectedSession.TriggerAlarmAt, _cut.Session.TriggerAlarmAt);

        _timerServiceMock.Verify(x => x.Stop(true), Times.Once);
    }

    [Fact]
    public void ResetCommand_TimerIsStopped_ShouldNotStopTimerAndShouldResetSession()
    {
        // arrange
        var expectedSession = SessionModel.Create();

        _cut.State = TimerStateEnum.Stopped;

        _alertServiceMock
            .Setup(x => x.DisplayConfirmAsync(_cut.Title, Constants.Messages.ResetSession))
            .ReturnsAsync(true);

        // act
        _cut.ResetCommand.Execute(null);

        // assert
        Assert.Equal(TimerStateEnum.Stopped, _cut.State);
        Assert.Equal(expectedSession.IntervalsCount, _cut.Session.IntervalsCount);
        Assert.Equal(expectedSession.WorkIntervalsCount, _cut.Session.WorkIntervalsCount);
        Assert.Equal(expectedSession.ShortRestIntervalsCount, _cut.Session.ShortRestIntervalsCount);
        Assert.Equal(expectedSession.LongRestIntervalsCount, _cut.Session.LongRestIntervalsCount);
        Assert.Equal(expectedSession.TriggerAlarmAt, _cut.Session.TriggerAlarmAt);

        _timerServiceMock.Verify(x => x.Stop(true), Times.Never);
    }

    [Fact]
    public void ResetCommand_ActionWasCanceled_ShouldNotStopTimerAndShouldNotResetSession()
    {
        // arrange
        _cut.State = TimerStateEnum.Running;

        _alertServiceMock
            .Setup(x => x.DisplayConfirmAsync(_cut.Title, Constants.Messages.ResetSession))
            .ReturnsAsync(false);

        // act
        _cut.ResetCommand.Execute(null);

        // assert
        Assert.Equal(TimerStateEnum.Running, _cut.State);

        _timerServiceMock.Verify(x => x.Stop(true), Times.Never);
    }

    [Fact]
    public void ShowSessionDetailsPopupCommand_ShouldDisplaySessionDetailsPopup()
    {
        // act
        _cut.ShowSessionDetailsPopupCommand.Execute(null);

        // assert
        _popupServiceMock.Verify(x => x.ShowSessionDetailsPopup(), Times.Once);
    }

    [Fact]
    public void CloseSessionDetailsPopupCommand_ShouldCloseSessionDetailsPopup()
    {
        // act
        _cut.CloseSessionDetailsPopupCommand.Execute(null);

        // assert
        _popupServiceMock.Verify(x => x.ClosePopup(It.IsAny<Popup>()));
    }

    [Fact]
    public async Task InitializeAsyncCommand_TimerIsRunning_ShouldNotUpdateClock()
    {
        // arrange
        var expectedValue = 2137;
        _cut.SecondsRemaining = expectedValue;
        _cut.State = TimerStateEnum.Running;

        // act
        await _cut.InitializeAsyncCommand.ExecuteAsync(null);

        // assert
        Assert.Equal(expectedValue, _cut.SecondsRemaining);
        
        _sessionServiceMock.Verify(
            x => x.GetIntervalLengthInSec(_cut.Session.CurrentInterval),
            Times.Never);
    }

    [Fact]
    public async Task InitializeAsyncCommand_TimerIsStopped_ShouldUpdateClock()
    {
        // arrange
        var expectedValue = 2137;
        _cut.SecondsRemaining = 0;
        _cut.State = TimerStateEnum.Stopped;

        _sessionServiceMock
            .Setup(x => x.GetIntervalLengthInSec(_cut.Session.CurrentInterval))
            .Returns(expectedValue);

        // act
        await _cut.InitializeAsyncCommand.ExecuteAsync(null);

        // assert
        Assert.Equal(expectedValue, _cut.SecondsRemaining);

        _sessionServiceMock.Verify(
            x => x.GetIntervalLengthInSec(_cut.Session.CurrentInterval),
            Times.Once());
    }
}