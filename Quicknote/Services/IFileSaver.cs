using Quicknote.Models;

namespace Quicknote.Services;

public interface IFileSaver
{
    Task SaveAsync(Note note);
}