using System.Collections.ObjectModel;
using System.Text.Json;
using System.Windows.Input;
using Quicknote.Models;
using Quicknote.Services;

namespace Quicknote.ViewModels;

public class MainViewModel : ViewModelBase
{
    INoteService _noteService;
    public ObservableCollection<Note> Notes { get; } = new();
    private string _noteText = ""; // ensure non-null by default
    public string NoteText
    {
        get => _noteText;
        set
        {
            if (_noteText == value) return;
            _noteText = value;
            OnPropertyChanged();
        }
    }

    public ICommand SaveNoteCommand { get; }

    public MainViewModel(INoteService noteService)
    {
        _noteService = noteService;
        SaveNoteCommand = new Command(async () => await SaveNoteAsync());
    }

    private async Task SaveNoteAsync()
    {
        if (string.IsNullOrWhiteSpace(NoteText)) return;

        var note = new Note
        {
            Id = Guid.NewGuid(),
            Content = NoteText,
            CreatedUtc = DateTime.UtcNow
        };

        Notes.Add(note);

        await _noteService.SaveAsync(note);
    }
}
