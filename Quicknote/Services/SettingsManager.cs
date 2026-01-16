using System.Text.Json;
using Quicknote.Models;

namespace Quicknote.Services;

public class SettingsManager : ISettingsManager
{
    private readonly string _settingsDirectory; // path to settings folder
    private readonly string _notesDirectory; // path to notes folder
    public AppSettings Settings { get; private set; }

    public SettingsManager()
    {
        _settingsDirectory = Path.Combine(FileSystem.AppDataDirectory, "Settings");
        _notesDirectory = Path.Combine(FileSystem.AppDataDirectory, "Notes");
        Directory.CreateDirectory(_settingsDirectory);
        Directory.CreateDirectory(_notesDirectory);
        Settings = new AppSettings();
        Load();
    }

    public void Load()
    {
        if (File.Exists(_settingsDirectory))
        {
            var json = File.ReadAllText(_settingsDirectory);
            Settings = JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
        }
        else
        {
            Settings = new AppSettings();
            Settings.NotesDirectory = _notesDirectory;
            Settings.FontSize = 14f;
            Save(); // create default file
        }
    }

    public void Save()
    {
        var json = JsonSerializer.Serialize(Settings, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(Path.Combine(_settingsDirectory, "appsettings.json"), json);
    }
}