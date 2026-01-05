using Quicknote.Models;
using System.Text.Json;

namespace Quicknote.Services;

/*This service is responsible for saving, deleting, and modifying notes.
More will be added as needed.*/
public class NoteService : INoteService
{
    private readonly ISettingsManager _settingsManager;
    private readonly string _notesDirectory;

    public NoteService(ISettingsManager sm)
    {
        _settingsManager = sm;
        _notesDirectory = _settingsManager.Settings.NotesDirectory;
        Directory.CreateDirectory(_notesDirectory);
    }

    public async Task SaveAsync(Note note)
    /*Save a JSON file to disk, in AppData directory*/
    {
        var json = JsonSerializer.Serialize(note, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        var filePath = Path.Combine(_notesDirectory, $"{note.Id}.json");

        await File.WriteAllTextAsync(filePath, json);
    }

    public Task DeleteAsync(Guid noteId)
    {
        string filePath = GetNotePath(noteId);

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        return Task.CompletedTask;
    }

    private string GetNotePath(Guid id)
    {
        return Path.Combine(_notesDirectory, $"{id}.json");
    }
}
