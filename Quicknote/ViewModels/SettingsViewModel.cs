using Quicknote.Services;

namespace Quicknote.ViewModels;

public class SettingsViewModel : ViewModelBase
{
    ISettingsManager _settingsManager;

    public SettingsViewModel(ISettingsManager settingsManager)
    {
        _settingsManager = settingsManager;
    }
}