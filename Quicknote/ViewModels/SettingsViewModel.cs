using System.Windows.Input;
using Notepad40.Services;

namespace Notepad40.ViewModels;

public class SettingsViewModel : ViewModelBase
{
    ISettingsManager _settingsManager;
    private const double MinFontSize = 8;
    private const double MaxFontSize = 28;
    private string _fontSizeText;
    public string FontSizeText
    {
        get => _fontSizeText;
        set
        {
            if (_fontSizeText == value) return;
            _fontSizeText = value;
            OnPropertyChanged();
        }
    }
    public double FontSize
    {
        get => _settingsManager.Settings.FontSize;
        set
        {
            if (_settingsManager.Settings.FontSize == value) return;
            _settingsManager.Settings.FontSize = value;
            OnPropertyChanged();
            _settingsManager.Save(); // immediately save changed settings to disk
            FontSizeText = value.ToString(); // keep textbox synced
        }
    }
    public ICommand IncreaseFontSizeCommand { get; } // for increasing with + button
    public ICommand DecreaseFontSizeCommand { get; } // for decreasing with - button

    public SettingsViewModel(ISettingsManager settingsManager)
    {
        _settingsManager = settingsManager;
        IncreaseFontSizeCommand = new Command(IncrementFontSize);
        DecreaseFontSizeCommand = new Command(DecrementFontSize);

        _fontSizeText = _settingsManager.Settings.FontSize.ToString();
    }

    private void IncrementFontSize()
    {
        if (FontSize < MaxFontSize)
            FontSize++;
    }
    private void DecrementFontSize()
    {
        if (FontSize > MinFontSize)
            FontSize--;
    }

    public void CommitFontSize()
    {
        if (!double.TryParse(FontSizeText, out var value))
        {
            // revert if invalid
            FontSizeText = FontSize.ToString();
            return;
        }

        value = Math.Clamp(value, MinFontSize, MaxFontSize);
        FontSize = value;
    }
}