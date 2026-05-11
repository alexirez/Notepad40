using Notepad40.Models;

namespace Notepad40.Services;

public interface INoteService
{
    Task SaveAsync(Note note);
    Task DeleteAsync(Guid noteId);
    Task<IReadOnlyList<Note>> LoadAllAsync();
    Task UpdateAsync(Note note);
}