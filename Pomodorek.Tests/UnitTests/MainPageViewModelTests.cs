namespace Pomodorek.Tests.UnitTests;

public class MainPageViewModelTests
{
    private readonly MainPageViewModel _viewModel;
    private readonly Mock<ITimer> _timerMock;
    private readonly Mock<INotificationService> _notificationServiceMock;

    public MainPageViewModelTests()
    {
        _timerMock = new Mock<ITimer>();
        _notificationServiceMock = new Mock<INotificationService>();
        _viewModel = new MainPageViewModel(
            _timerMock.Object,
            _notificationServiceMock.Object);
    }

    [Fact]
    public async Task DisplayNotification_WhenCalled_InvokesNotificationService()
    {
        // arrange
        var message = "foo";

        // act
        await _viewModel.DisplayNotification(message);

        // assert
        _notificationServiceMock.Verify(x => x.DisplayNotification(message), Times.Once);
    }

    [Fact]
    public async Task StartSession_WhenCalled_StartsTimer()
    {
        // act
        _viewModel.StartSession();

        // assert
        _timerMock.Verify(x => x.Start(It.IsAny<Action>()), Times.Once);
    }

    [Fact]
    public async Task StopSession_WhenCalled_StopsTimer()
    {
        // act
        _viewModel.StopSession();

        // assert
        _timerMock.Verify(x => x.Stop(), Times.Once);
    }
}