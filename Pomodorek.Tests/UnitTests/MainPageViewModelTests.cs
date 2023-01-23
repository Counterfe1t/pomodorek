namespace Pomodorek.Tests.UnitTests;

public class MainPageViewModelTests
{
    private readonly MainPageViewModel _viewModel;
    private readonly Mock<INotificationService> _notificationServiceMock;

    public MainPageViewModelTests()
    {
        _notificationServiceMock = new Mock<INotificationService>();
        _viewModel = new MainPageViewModel(_notificationServiceMock.Object);
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
}