using System.Text.Json;
using Quicknote.Models;

namespace Quicknote.Services;

public class SettingsManager : ISettingsManager
{
    private readonly string _filePath; // path to settings config JSON
    public AppSettings Settings { get; private set; }

    public SettingsManager()
    {
        _filePath = Path.Combine(FileSystem.AppDataDirectory, "appsettings.json");
        Settings = new AppSettings();
        Load();
    }

    public void Load()
    {
        if (File.Exists(_filePath))
        {
            var json = File.ReadAllText(_filePath);
            Settings = JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
        }
        else
        {
            Settings = new AppSettings();
            Settings.NotesDirectory = FileSystem.AppDataDirectory;
            Save(); // create default file
        }
    }

    public void Save()
    {
        var json = JsonSerializer.Serialize(Settings, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }
}