using Quicknote.Models;
using System.Text.Json;

namespace Quicknote.Services;

public class FileSaver : IFileSaver
{
    public async Task SaveAsync(Note note)
    /*Save a JSON file to disk, in AppData directory*/
    {
        var json = JsonSerializer.Serialize(note, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        var directory = FileSystem.AppDataDirectory;
        var filePath = Path.Combine(directory, $"{note.Id}.json");

        await File.WriteAllTextAsync(filePath, json);
    }
}
