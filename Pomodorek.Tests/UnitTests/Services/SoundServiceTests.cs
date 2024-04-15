using Plugin.Maui.Audio;

namespace Pomodorek.Tests.UnitTests.Services;

public class SoundServiceTests
{
    private readonly SoundService _soundService;
    private readonly Mock<IAudioManager> _audioManagerMock;
    private readonly Mock<IFileSystem> _fileSystemMock;
    private readonly Mock<ISettingsService> _settingsService;
    private readonly Mock<IConfigurationService> _configurationServiceMock;

    private static AppSettings AppSettings => new();

    public SoundServiceTests()
    {
        _audioManagerMock = new Mock<IAudioManager>();
        _fileSystemMock = new Mock<IFileSystem>();
        _settingsService = new Mock<ISettingsService>();
        _configurationServiceMock = new Mock<IConfigurationService>();

        _soundService = new SoundService(
            _audioManagerMock.Object,
            _fileSystemMock.Object,
            _settingsService.Object,
            _configurationServiceMock.Object);
    }

    [Fact]
    public async Task PlaySoundAsync_SoundIsEnabled_PlaysSound()
    {
        // arrange
        var audioPlayerMock = new Mock<IAudioPlayer>();
        var audioStream = new Mock<Stream>();

        _settingsService
            .Setup(x => x.Get(Constants.Settings.IsSoundEnabled, It.IsAny<bool>()))
            .Returns(true);

        _configurationServiceMock
            .Setup(x => x.GetAppSettings()).Returns(AppSettings);

        _fileSystemMock
            .Setup(x => x.OpenAppPackageFileAsync(It.IsAny<string>()))
            .ReturnsAsync(audioStream.Object);

        _audioManagerMock
            .Setup(x => x.CreatePlayer(audioStream.Object))
            .Returns(audioPlayerMock.Object);

        // act
        await _soundService.PlaySoundAsync(It.IsAny<string>());

        // assert
        _audioManagerMock.Verify(x => x.CreatePlayer(It.IsAny<Stream>()), Times.Once);
        audioPlayerMock.Verify(x => x.Play(), Times.Once);
    }

    [Fact]
    public async Task PlaySoundAsync_SoundIsDisabled_DoesNotPlaySound()
    {
        // arrange
        var audioPlayerMock = new Mock<IAudioPlayer>();
        var audioStream = new Mock<Stream>();

        _settingsService
            .Setup(x => x.Get(Constants.Settings.IsSoundEnabled, It.IsAny<bool>()))
            .Returns(false);

        _configurationServiceMock
            .Setup(x => x.GetAppSettings()).Returns(AppSettings);

        _fileSystemMock
            .Setup(x => x.OpenAppPackageFileAsync(It.IsAny<string>()))
            .ReturnsAsync(audioStream.Object);

        _audioManagerMock
            .Setup(x => x.CreatePlayer(audioStream.Object))
            .Returns(audioPlayerMock.Object);

        // act
        await _soundService.PlaySoundAsync(It.IsAny<string>());

        // assert
        _audioManagerMock.Verify(x => x.CreatePlayer(It.IsAny<Stream>()), Times.Never);
        audioPlayerMock.Verify(x => x.Play(), Times.Never);
    }
}
