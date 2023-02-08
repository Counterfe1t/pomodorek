using Pomodorek.Services;

namespace Pomodorek.Tests.UnitTests;

public class TimerServiceTests
{
    private readonly ITimerService _timer;
    private readonly Mock<Action> _callbackMock;

    public TimerServiceTests()
    {
        _callbackMock = new Mock<Action>();
        _timer = new TimerService();
    }

    // TODO: This fails every once in a while
    [Fact]
    public async Task Start_WhenCalled_CallbackIsInvoked()
    {
        // arrange
        _callbackMock.Invocations.Clear();

        // act
        _timer.Start(_callbackMock.Object);
        await Task.Delay(1000);

        // assert
        _callbackMock.Verify(x => x.Invoke(), Times.AtLeastOnce);
    }

    [Fact]
    public async Task Start_WhenStoppedAfterOneSecond_CallbackIsInvokedOnce()
    {
        // arrange
        _callbackMock.Invocations.Clear();

        // act
        _timer.Start(_callbackMock.Object);
        await Task.Delay(1000);
        _timer.Stop();

        // assert
        _callbackMock.Verify(x => x.Invoke(), Times.Once);
    }
}
