using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Notepad40.Models;

namespace Notepad40.Services;

public class SettingsManager : ISettingsManager, INotifyPropertyChanged
{
    private readonly string _settingsDirectory; // path to settings folder
    private readonly string _notesDirectory; // path to notes folder
    private AppSettings _settings;
    public AppSettings Settings
    {
        get => _settings;
        private set
        {
            if (_settings == value)
                return;

            if (_settings != null)
                _settings.PropertyChanged -= OnSettingsChanged;

            _settings = value;
            _settings.PropertyChanged += OnSettingsChanged;

            OnPropertyChanged();
        }
    }

    protected void OnPropertyChanged(
        [CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    public event PropertyChangedEventHandler? PropertyChanged;

    public SettingsManager()
    {
        _settingsDirectory = Path.Combine(FileSystem.AppDataDirectory, "Settings");
        _notesDirectory = Path.Combine(FileSystem.AppDataDirectory, "Notes");
        Directory.CreateDirectory(_settingsDirectory);
        Directory.CreateDirectory(_notesDirectory);
        _settings = new AppSettings();
        Load(); // load settings from disk
    }

    public void Load()
    {
        var filePath = Path.Combine(_settingsDirectory, "appsettings.json");
        if (File.Exists(filePath))
        {
            var json = File.ReadAllText(filePath);
            Settings = JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
        }
        else
        {
            Settings = new AppSettings
            {
                NotesDirectory = _notesDirectory,
                FontSize = 14f
            };
            Save(); // create default file
        }
    }

    public void Save()
    {
        var filePath = Path.Combine(_settingsDirectory, "appsettings.json");
        var json = JsonSerializer.Serialize(Settings, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }

    private void OnSettingsChanged(object? sender, PropertyChangedEventArgs e)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(e.PropertyName));
    }
}