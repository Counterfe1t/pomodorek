namespace Pomodorek.Tests.UnitTests.Services;

public class SoundServiceTests
{
    private readonly ISoundService _cut;

    private readonly Mock<IAudioManager> _audioManagerMock;
    private readonly Mock<IFileSystem> _fileSystemMock;
    private readonly Mock<ISettingsService> _settingsService;
    private readonly Mock<IConfigurationService> _configurationServiceMock;
    private readonly Mock<IAudioPlayer> _audioPlayerMock;

    private AppSettings _appSettings => new();

    public SoundServiceTests()
    {
        _audioManagerMock = new();
        _fileSystemMock = new();
        _settingsService = new();
        _configurationServiceMock = new();
        _audioPlayerMock = new();

        _configurationServiceMock
            .Setup(x => x.AppSettings)
            .Returns(_appSettings);

        _cut = ClassUnderTest.Is<SoundService>(
            _audioManagerMock.Object,
            _fileSystemMock.Object,
            _settingsService.Object,
            _configurationServiceMock.Object);
    }

    [Fact]
    public async Task PlaySoundAsync_SoundIsEnabled_ShouldPlaySound()
    {
        // arrange
        var audioStream = new MemoryStream(Encoding.UTF8.GetBytes(string.Empty));

        _settingsService
            .Setup(x => x.Get(Constants.Settings.IsSoundEnabled, It.IsAny<bool>()))
            .Returns(true);
        
        _fileSystemMock
            .Setup(x => x.OpenAppPackageFileAsync(It.IsAny<string>()))
            .ReturnsAsync(audioStream);

        _audioManagerMock
            .Setup(x => x.CreatePlayer(audioStream))
            .Returns(_audioPlayerMock.Object);

        // act
        await _cut.PlaySoundAsync(It.IsAny<string>());

        // assert
        _audioManagerMock.Verify(x => x.CreatePlayer(It.IsAny<Stream>()), Times.Once);
        _audioPlayerMock.Verify(x => x.Play(), Times.Once);
    }

    [Fact]
    public async Task PlaySoundAsync_SoundIsDisabled_ShouldNotPlaySound()
    {
        // arrange
        var audioStream = new MemoryStream(Encoding.UTF8.GetBytes(string.Empty));

        _settingsService
            .Setup(x => x.Get(Constants.Settings.IsSoundEnabled, It.IsAny<bool>()))
            .Returns(false);

        _fileSystemMock
            .Setup(x => x.OpenAppPackageFileAsync(It.IsAny<string>()))
            .ReturnsAsync(audioStream);

        _audioManagerMock
            .Setup(x => x.CreatePlayer(audioStream))
            .Returns(_audioPlayerMock.Object);

        // act
        await _cut.PlaySoundAsync(It.IsAny<string>());

        // assert
        _audioManagerMock.Verify(x => x.CreatePlayer(It.IsAny<Stream>()), Times.Never);
        _audioPlayerMock.Verify(x => x.Play(), Times.Never);
    }
}