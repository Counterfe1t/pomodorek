namespace Pomodorek.Tests.UnitTests.ViewModels;

public class TimerPageViewModelTests
{
    private readonly TimerPageViewModel _viewModel;

    private readonly Mock<ITimerService> _timerServiceMock;
    private readonly Mock<IDateTimeService> _dateTimeServiceMock;
    private readonly Mock<IPermissionsService> _permissionsServiceMock;
    private readonly Mock<ISessionService> _sessionServiceMock;
    private readonly Mock<IPopupService> _popupServiceMock;
    private readonly Mock<IAlertService> _alertServiceMock;

    public TimerPageViewModelTests()
    {
        _timerServiceMock = new();
        _dateTimeServiceMock = new();
        _permissionsServiceMock = new();
        _sessionServiceMock = new();
        _popupServiceMock = new();
        _alertServiceMock = new();

        _sessionServiceMock
            .Setup(x => x.GetSession())
            .Returns(SessionModel.Create());

        _viewModel = new(
            _timerServiceMock.Object,
            _dateTimeServiceMock.Object,
            _permissionsServiceMock.Object,
            _sessionServiceMock.Object,
            _popupServiceMock.Object,
            _alertServiceMock.Object);

        _timerServiceMock.Invocations.Clear();
    }

    [Fact]
    public void StartCommand_ShouldStartTimer()
    {
        // act
        _viewModel.StartCommand.Execute(null);

        // assert
        Assert.Equal(TimerStateEnum.Running, _viewModel.State);

        _timerServiceMock.Verify(x => x.Start(It.IsAny<Action>()), Times.Once);
        _sessionServiceMock.Verify(
            x => x.StartInterval(It.Is<SessionModel>(y => y.CurrentInterval == IntervalEnum.Work)),
            Times.Once);
    }

    [Fact]
    public void PauseCommand_ShouldPauseTimer()
    {
        // act
        _viewModel.PauseCommand.Execute(null);

        // assert
        Assert.Equal(TimerStateEnum.Paused, _viewModel.State);

        _timerServiceMock.Verify(x => x.Stop(true), Times.Once);
    }

    [Fact]
    public void StopCommand_TimerIsRunning_ShouldStopTimer()
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
    public void StopCommand_TimerIsStopped_ShouldNotStopTimer()
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
    public void ResetCommand_TimerIsRunning_ShouldStopTimerAndShouldResetSession()
    {
        // arrange
        var expectedSession = SessionModel.Create();

        _viewModel.State = TimerStateEnum.Running;

        _alertServiceMock
            .Setup(x => x.DisplayConfirmAsync(_viewModel.Title, Constants.Messages.ResetSession))
            .ReturnsAsync(true);

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
    public void ResetCommand_TimerIsStopped_ShouldNotStopTimerAndShouldResetSession()
    {
        // arrange
        var expectedSession = SessionModel.Create();

        _viewModel.State = TimerStateEnum.Stopped;

        _alertServiceMock
            .Setup(x => x.DisplayConfirmAsync(_viewModel.Title, Constants.Messages.ResetSession))
            .ReturnsAsync(true);

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

    [Fact]
    public void ResetCommand_ActionWasCanceled_ShouldNotStopTimerAndShouldNotResetSession()
    {
        // arrange
        _viewModel.State = TimerStateEnum.Running;

        _alertServiceMock
            .Setup(x => x.DisplayConfirmAsync(_viewModel.Title, Constants.Messages.ResetSession))
            .ReturnsAsync(false);

        // act
        _viewModel.ResetCommand.Execute(null);

        // assert
        Assert.Equal(TimerStateEnum.Running, _viewModel.State);

        _timerServiceMock.Verify(x => x.Stop(true), Times.Never);
    }

    [Fact]
    public void ShowSessionDetailsPopupCommand_ShouldDisplaySessionDetailsPopup()
    {
        // act
        _viewModel.ShowSessionDetailsPopupCommand.Execute(null);

        // assert
        _popupServiceMock.Verify(x => x.ShowSessionDetailsPopup(), Times.Once);
    }

    [Fact]
    public void CloseSessionDetailsPopupCommand_ShouldCloseSessionDetailsPopup()
    {
        // act
        _viewModel.CloseSessionDetailsPopupCommand.Execute(null);

        // assert
        _popupServiceMock.Verify(x => x.ClosePopup(It.IsAny<Popup>()));
    }

    [Fact]
    public async Task CheckAndRequestPermissionsAsync_ShouldCheckAndRequestPermissions()
    {
        // act
        await _viewModel.CheckAndRequestPermissionsAsync();

        // assert
        _permissionsServiceMock.Verify(x => x.CheckAndRequestPermissionsAsync(), Times.Once);
    }

    [Theory]
    [MemberData(nameof(UpdateClockTestData))]
    public void UpdateClock_ShouldSetExpectedSecondsRemainingValue(
        int? value,
        int expectedValue,
        int invocations)
    {
        // arrange
        _sessionServiceMock
            .Setup(x => x.GetIntervalLengthInSec(_viewModel.Session.CurrentInterval))
            .Returns(expectedValue);

        // act
        _viewModel.UpdateClock(value);

        // assert
        Assert.Equal(expectedValue, _viewModel.SecondsRemaining);

        _sessionServiceMock
            .Verify(x => x.GetIntervalLengthInSec(_viewModel.Session.CurrentInterval), Times.Exactly(invocations));
    }

    public static TheoryData<int?, int, int> UpdateClockTestData => new()
    {
        { 1337, 1337, 0 },
        { 0, 0, 0 },
        { -1, 60, 1 },
        { null, 60, 1 }
    };
}