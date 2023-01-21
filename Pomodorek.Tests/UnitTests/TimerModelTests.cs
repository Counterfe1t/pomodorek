using Moq;
using Pomodorek.Models;

namespace Pomodorek.Tests.UnitTests
{
    public class TimerModelTests
    {
        private readonly TimerModel _timer;
        private readonly Mock<Action> _callbackMock;

        public TimerModelTests()
        {
            _callbackMock = new Mock<Action>();
            _timer = new TimerModel(_callbackMock.Object);
        }

        [Fact]
        public async Task Start_WhenCalled_CallbackIsInvoked()
        {
            // arrange
            _callbackMock.Invocations.Clear();

            // act
            _timer.Start();
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
            _timer.Start();
            await Task.Delay(1000);
            _timer.Stop();

            // assert
            _callbackMock.Verify(x => x.Invoke(), Times.Once);
        }
    }
}
