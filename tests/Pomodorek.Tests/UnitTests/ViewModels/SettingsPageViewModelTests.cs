namespace Pomodorek.Tests.UnitTests.ViewModels;

public class SettingsPageViewModelTests
{
    private readonly SettingsPageViewModel _cut;

    private readonly Mock<ISettingsService> _settingsServiceMock;
    private readonly Mock<IConfigurationService> _configurationServiceMock;
    private readonly Mock<IAlertService> _alertServiceMock;
    private readonly Mock<INavigationService> _navigationServiceMock;
    private readonly Mock<IApplicationService> _applicationServiceMock;

    private AppSettings AppSettings => new()
    {
        AppName = "Pomodorek",
        AppVersion = "1.0.0",
        DefaultIsDarkThemeEnabled = true,
        DefaultIsSoundEnabled = true,
        DefaultSoundVolume = 1,
        DefaultWorkLengthInMin = 3,
        DefaultShortRestLengthInMin = 3,
        DefaultLongRestLengthInMin = 7
    };

    public SettingsPageViewModelTests()
    {
        _settingsServiceMock = new();
        _configurationServiceMock = new();
        _alertServiceMock = new();
        _navigationServiceMock = new();
        _applicationServiceMock = new();

        _configurationServiceMock
            .Setup(x => x.AppSettings)
            .Returns(AppSettings);

        _applicationServiceMock
            .Setup(x => x.Application)
            .Returns(new Application());

        _cut = ClassUnderTest.Is<SettingsPageViewModel>(
            _settingsServiceMock.Object,
            _configurationServiceMock.Object,
            _alertServiceMock.Object,
            _navigationServiceMock.Object,
            _applicationServiceMock.Object);
    }

    [Fact]
    public void InitializeSettings_ShouldInitializeProperties()
    {
        // act
        _cut.InitializeSettings();

        // assert
        _settingsServiceMock.Verify(
            x => x.Get(Constants.Settings.IsDarkThemeEnabled, It.IsAny<bool>()),
            Times.Once);
        _settingsServiceMock.Verify(
            x => x.Get(Constants.Settings.IsSoundEnabled, It.IsAny<bool>()),
            Times.Once);
        _settingsServiceMock.Verify(
            x => x.Get(Constants.Settings.SoundVolume, It.IsAny<float>()),
            Times.Once);
        _settingsServiceMock.Verify(
            x => x.Get(Constants.Settings.WorkLengthInMin, It.IsAny<int>()),
            Times.Once);
        _settingsServiceMock.Verify(
            x => x.Get(Constants.Settings.ShortRestLengthInMin, It.IsAny<int>()),
            Times.Once);
        _settingsServiceMock.Verify(
            x => x.Get(Constants.Settings.LongRestLengthInMin, It.IsAny<int>()),
            Times.Once);
    }

    [Fact]
    public void SaveSettings_ShouldSaveSettingsToStorage()
    {
        // act
        _cut.SaveSettingsCommand.Execute(null);

        // assert
        _settingsServiceMock.Verify(
            x => x.Set(Constants.Settings.IsDarkThemeEnabled, It.IsAny<bool>()),
            Times.Once);
        _settingsServiceMock.Verify(
            x => x.Set(Constants.Settings.IsSoundEnabled, It.IsAny<bool>()),
            Times.Once);
        _settingsServiceMock.Verify(
            x => x.Set(Constants.Settings.SoundVolume, It.IsAny<float>()),
            Times.Once);
        _settingsServiceMock.Verify(
            x => x.Set(Constants.Settings.WorkLengthInMin, It.IsAny<int>()),
            Times.Once);
        _settingsServiceMock.Verify(
            x => x.Set(Constants.Settings.ShortRestLengthInMin, It.IsAny<int>()),
            Times.Once);
        _settingsServiceMock.Verify(
            x => x.Set(Constants.Settings.LongRestLengthInMin, It.IsAny<int>()),
            Times.Once);
        _alertServiceMock.Verify(
            x => x.DisplayAlertAsync(AppResources.SettingsPage_Title, AppResources.SettingsPage_SettingsSavedText),
            Times.Once);
        _navigationServiceMock.Verify(
            x => x.NavigateToAsync(AppResources.TimerPage_Route),
            Times.Once);
    }

    [Fact]
    public void RestoreDefaultSettings_ActionWasCanceled_ShouldNotRestoreDefaultSettings()
    {
        // arrange
        _alertServiceMock
            .Setup(x => x.DisplayConfirmAsync(_cut.Title, AppResources.SettingsPage_RestoreDefaultSettingsText))
            .ReturnsAsync(false);

        // act
        _cut.RestoreSettingsCommand.Execute(null);

        // assert
        _settingsServiceMock.Verify(
            x => x.Set(Constants.Settings.IsDarkThemeEnabled, AppSettings.DefaultIsDarkThemeEnabled),
            Times.Never);
        _settingsServiceMock.Verify(
            x => x.Set(Constants.Settings.IsSoundEnabled, AppSettings.DefaultIsSoundEnabled),
            Times.Never);
        _settingsServiceMock.Verify(
            x => x.Set(Constants.Settings.SoundVolume, AppSettings.DefaultSoundVolume),
            Times.Never);
        _settingsServiceMock.Verify(
            x => x.Set(Constants.Settings.WorkLengthInMin, AppSettings.DefaultWorkLengthInMin),
            Times.Never);
        _settingsServiceMock.Verify(
            x => x.Set(Constants.Settings.ShortRestLengthInMin, AppSettings.DefaultShortRestLengthInMin),
            Times.Never);
        _settingsServiceMock.Verify(
            x => x.Set(Constants.Settings.LongRestLengthInMin, AppSettings.DefaultLongRestLengthInMin),
            Times.Never);
        _alertServiceMock.Verify(
            x => x.DisplayAlertAsync(AppResources.SettingsPage_Title, AppResources.SettingsPage_SettingsRestoredText),
            Times.Never);
    }

    [Fact]
    public void RestoreDefaultSettings_ActionWasNotCanceled_ShouldRestoreDefaultSettings()
    {
        // arrange
        _alertServiceMock
            .Setup(x => x.DisplayConfirmAsync(_cut.Title, AppResources.SettingsPage_RestoreDefaultSettingsText))
            .ReturnsAsync(true);

        // act
        _cut.RestoreSettingsCommand.Execute(null);

        // assert
        _settingsServiceMock.Verify(
            x => x.Set(Constants.Settings.IsDarkThemeEnabled, AppSettings.DefaultIsDarkThemeEnabled),
            Times.Once);
        _settingsServiceMock.Verify(
            x => x.Set(Constants.Settings.IsSoundEnabled, AppSettings.DefaultIsSoundEnabled),
            Times.Once);
        _settingsServiceMock.Verify(
            x => x.Set(Constants.Settings.SoundVolume, AppSettings.DefaultSoundVolume),
            Times.Once);
        _settingsServiceMock.Verify(
            x => x.Set(Constants.Settings.WorkLengthInMin, AppSettings.DefaultWorkLengthInMin),
            Times.Once);
        _settingsServiceMock.Verify(
            x => x.Set(Constants.Settings.ShortRestLengthInMin, AppSettings.DefaultShortRestLengthInMin),
            Times.Once);
        _settingsServiceMock.Verify(
            x => x.Set(Constants.Settings.LongRestLengthInMin, AppSettings.DefaultLongRestLengthInMin),
            Times.Once);
        _alertServiceMock.Verify(
            x => x.DisplayAlertAsync(AppResources.SettingsPage_Title, AppResources.SettingsPage_SettingsRestoredText),
            Times.Once);
    }

    [Fact]
    public async Task DisplayUnsavedChangesDialog_ShouldShowDialogWarningAboutUnsavedChanges()
    {
        // act
        await _cut.DisplayUnsavedChangesDialog();

        // assert
        _alertServiceMock.Verify(
            x => x.DisplayConfirmAsync(AppResources.SettingsPage_Title, AppResources.SettingsPage_UnsavedChangesText),
            Times.Once);
    }
}