using Quicknote.Models;

namespace Quicknote.Services;

public interface INoteService
{
    Task SaveAsync(Note note);
    Task DeleteAsync(Guid noteId);
}