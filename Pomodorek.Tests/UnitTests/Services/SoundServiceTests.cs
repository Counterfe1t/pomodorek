namespace Pomodorek.Tests.UnitTests.Services;

public class SoundServiceTests
{
    private readonly SoundService _soundService;
    private readonly Mock<IAudioManager> _audioManagerMock;
    private readonly Mock<IFileSystem> _fileSystemMock;
    private readonly Mock<ISettingsService> _settingsService;
    private readonly Mock<IConfigurationService> _configurationServiceMock;
    private readonly Mock<IAudioPlayer> _audioPlayerMock;

    private static AppSettings AppSettings => new();

    public SoundServiceTests()
    {
        _audioManagerMock = new Mock<IAudioManager>();
        _fileSystemMock = new Mock<IFileSystem>();
        _settingsService = new Mock<ISettingsService>();
        _configurationServiceMock = new Mock<IConfigurationService>();
        _audioPlayerMock = new Mock<IAudioPlayer>();

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
        var audioStream = new MemoryStream(Encoding.UTF8.GetBytes(string.Empty));

        _settingsService
            .Setup(x => x.Get(Constants.Settings.IsSoundEnabled, It.IsAny<bool>()))
            .Returns(true);

        _configurationServiceMock
            .Setup(x => x.GetAppSettings()).Returns(AppSettings);

        _fileSystemMock
            .Setup(x => x.OpenAppPackageFileAsync(It.IsAny<string>()))
            .ReturnsAsync(audioStream);

        _audioManagerMock
            .Setup(x => x.CreatePlayer(audioStream))
            .Returns(_audioPlayerMock.Object);

        // act
        await _soundService.PlaySoundAsync(It.IsAny<string>());

        // assert
        _audioManagerMock.Verify(x => x.CreatePlayer(It.IsAny<Stream>()), Times.Once);
        _audioPlayerMock.Verify(x => x.Play(), Times.Once);
    }

    [Fact]
    public async Task PlaySoundAsync_SoundIsDisabled_DoesNotPlaySound()
    {
        // arrange
        var audioStream = new MemoryStream(Encoding.UTF8.GetBytes(string.Empty));

        _settingsService
            .Setup(x => x.Get(Constants.Settings.IsSoundEnabled, It.IsAny<bool>()))
            .Returns(false);

        _configurationServiceMock
            .Setup(x => x.GetAppSettings()).Returns(AppSettings);

        _fileSystemMock
            .Setup(x => x.OpenAppPackageFileAsync(It.IsAny<string>()))
            .ReturnsAsync(audioStream);

        _audioManagerMock
            .Setup(x => x.CreatePlayer(audioStream))
            .Returns(_audioPlayerMock.Object);

        // act
        await _soundService.PlaySoundAsync(It.IsAny<string>());

        // assert
        _audioManagerMock.Verify(x => x.CreatePlayer(It.IsAny<Stream>()), Times.Never);
        _audioPlayerMock.Verify(x => x.Play(), Times.Never);
    }
}