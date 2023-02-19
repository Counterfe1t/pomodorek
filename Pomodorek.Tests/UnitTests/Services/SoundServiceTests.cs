using Plugin.Maui.Audio;

namespace Pomodorek.Tests.UnitTests.Services;

public class SoundServiceTests
{
    private readonly SoundService _soundService;
    private readonly Mock<IAudioManager> _audioManagerMock;
    private readonly Mock<IFileSystem> _fileSystemMock;
    private readonly Mock<ISettingsService> _settingsService;

    public SoundServiceTests()
    {
        _audioManagerMock = new Mock<IAudioManager>();
        _fileSystemMock = new Mock<IFileSystem>();
        _settingsService = new Mock<ISettingsService>();

        _soundService = new SoundService(
            _audioManagerMock.Object,
            _fileSystemMock.Object,
            _settingsService.Object);
    }

    [Fact]
    public async Task PlaySound_WhenSoundEnabled_PlaysSound()
    {
        // arrange
        var audioPlayerMock = new Mock<IAudioPlayer>();
        var audioStream = new Mock<Stream>();

        _settingsService
            .Setup(x => x.Get(Constants.Settings.IsSoundEnabled, It.IsAny<bool>()))
            .Returns(true);

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

    [Fact]
    public async Task PlaySound_WhenSoundDisabled_DoesNotPlaySound()
    {
        // arrange
        var audioPlayerMock = new Mock<IAudioPlayer>();
        var audioStream = new Mock<Stream>();

        _settingsService
            .Setup(x => x.Get(Constants.Settings.IsSoundEnabled, It.IsAny<bool>()))
            .Returns(false);

        _fileSystemMock
            .Setup(x => x.OpenAppPackageFileAsync(It.IsAny<string>()))
            .ReturnsAsync(audioStream.Object);

        _audioManagerMock
            .Setup(x => x.CreatePlayer(audioStream.Object))
            .Returns(audioPlayerMock.Object);

        // act
        await _soundService.PlaySound(It.IsAny<string>());

        // assert
        _audioManagerMock.Verify(x => x.CreatePlayer(It.IsAny<Stream>()), Times.Never);
        audioPlayerMock.Verify(x => x.Play(), Times.Never);
    }
}
