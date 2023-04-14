namespace Pomodorek.Tests.UnitTests.ViewModels;

public class SettingsPageViewModelTests
{
    private readonly SettingsPageViewModel _viewModel;
    private readonly Mock<ISettingsService> _settingsServiceMock;
    private readonly Mock<IConfigurationService> _configurationServiceMock;
    private readonly Mock<IAlertService> _alertServiceMock;
    private readonly Mock<INavigationService> _navigationServiceMock;

    private static AppSettings AppSettings => new()
    {
        DefaultIsSoundEnabled = true,
        DefaultSoundVolume = 1,
        DefaultFocusLengthInMin = 3,
        DefaultShortRestLengthInMin = 3,
        DefaultLongRestLengthInMin = 7
    };

    public SettingsPageViewModelTests()
    {
        _settingsServiceMock = new Mock<ISettingsService>();
        _configurationServiceMock = new Mock<IConfigurationService>();
        _alertServiceMock = new Mock<IAlertService>();
        _navigationServiceMock = new Mock<INavigationService>();

        _viewModel = new SettingsPageViewModel(
            _settingsServiceMock.Object,
            _configurationServiceMock.Object,
            _alertServiceMock.Object,
            _navigationServiceMock.Object);
    }

    [Fact]
    public void InitializeSettings_WhenCalled_InitializesProperties()
    {
        // arrange
        _configurationServiceMock
            .Setup(x => x.GetAppSettings())
            .Returns(AppSettings);

        // act
        _viewModel.InitializeSettings();

        // assert
        _settingsServiceMock
            .Verify(x => x.Get(Constants.Settings.IsSoundEnabled, It.IsAny<bool>()), Times.Once);
        _settingsServiceMock
            .Verify(x => x.Get(Constants.Settings.SoundVolume, It.IsAny<float>()), Times.Once);
        _settingsServiceMock
            .Verify(x => x.Get(Constants.Settings.FocusLengthInMin, It.IsAny<int>()), Times.Once);
        _settingsServiceMock
            .Verify(x => x.Get(Constants.Settings.ShortRestLengthInMin, It.IsAny<int>()), Times.Once);
        _settingsServiceMock
            .Verify(x => x.Get(Constants.Settings.LongRestLengthInMin, It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public void SaveSettings_WhenCalled_SavesSettingsToStorage()
    {
        // arrange
        _configurationServiceMock
            .Setup(x => x.GetAppSettings())
            .Returns(AppSettings);

        // act
        _viewModel.SaveCommand.Execute(null);

        // assert
        _settingsServiceMock
            .Verify(x => x.Set(Constants.Settings.IsSoundEnabled, It.IsAny<bool>()), Times.Once);
        _settingsServiceMock
            .Verify(x => x.Set(Constants.Settings.SoundVolume, It.IsAny<float>()), Times.Once);
        _settingsServiceMock
            .Verify(x => x.Set(Constants.Settings.FocusLengthInMin, It.IsAny<int>()), Times.Once);
        _settingsServiceMock
            .Verify(x => x.Set(Constants.Settings.ShortRestLengthInMin, It.IsAny<int>()), Times.Once);
        _settingsServiceMock
            .Verify(x => x.Set(Constants.Settings.LongRestLengthInMin, It.IsAny<int>()), Times.Once);
        _alertServiceMock
            .Verify(x => x.DisplayAlertAsync(Constants.Pages.Settings, Constants.Messages.SettingsSaved));
        _navigationServiceMock
            .Verify(x => x.GoToTimerPageAsync(), Times.Once);
    }

    [Fact]
    public void RestoreDefaultSettings_WhenCalled_RestoresDefaultSettings()
    {
        // arrange
        _configurationServiceMock
            .Setup(x => x.GetAppSettings())
            .Returns(AppSettings);

        // act
        _viewModel.RestoreCommand.Execute(null);

        // assert
        _settingsServiceMock
            .Verify(x => x.Set(Constants.Settings.IsSoundEnabled, AppSettings.DefaultIsSoundEnabled), Times.Once);
        _settingsServiceMock
            .Verify(x => x.Set(Constants.Settings.SoundVolume, AppSettings.DefaultSoundVolume), Times.Once);
        _settingsServiceMock
            .Verify(x => x.Set(Constants.Settings.FocusLengthInMin, AppSettings.DefaultFocusLengthInMin), Times.Once);
        _settingsServiceMock
            .Verify(x => x.Set(Constants.Settings.ShortRestLengthInMin, AppSettings.DefaultShortRestLengthInMin), Times.Once);
        _settingsServiceMock
            .Verify(x => x.Set(Constants.Settings.LongRestLengthInMin, AppSettings.DefaultLongRestLengthInMin), Times.Once);
        _alertServiceMock
            .Verify(x => x.DisplayAlertAsync(Constants.Pages.Settings, Constants.Messages.SettingsRestored));
    }
}