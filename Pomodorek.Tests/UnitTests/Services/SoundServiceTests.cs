using Plugin.Maui.Audio;

namespace Pomodorek.Tests.UnitTests.Services;

public class SoundServiceTests
{
    private readonly SoundService _soundService;
    private readonly Mock<IAudioManager> _audioManagerMock;
    private readonly Mock<IFileSystem> _fileSystemMock;

    public SoundServiceTests()
    {
        _audioManagerMock = new Mock<IAudioManager>();
        _fileSystemMock = new Mock<IFileSystem>();

        _soundService = new SoundService(
            _audioManagerMock.Object,
            _fileSystemMock.Object);
    }

    [Fact]
    public async Task PlaySound_WhenCalled_StartsPlayingSound()
    {
        // arrange
        var audioPlayerMock = new Mock<IAudioPlayer>();
        var audioStream = new Mock<Stream>();

        _fileSystemMock
            .Setup(x => x.OpenAppPackageFileAsync(It.IsAny<string>()))
            .ReturnsAsync(audioStream.Object);

        _audioManagerMock
            .Setup(x => x.CreatePlayer(audioStream.Object))
            .Returns(audioPlayerMock.Object);

        // act
        await _soundService.PlaySound(It.IsAny<string>());

        // assert
        _audioManagerMock.Verify(x => x.CreatePlayer(It.IsAny<Stream>()), Times.Once);
        audioPlayerMock.Verify(x => x.Play(), Times.Once);
    }
}
