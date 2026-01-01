using System.Collections.ObjectModel;
using System.Text.Json;
using System.Windows.Input;
using Quicknote.Models;
using Quicknote.Services;

namespace Quicknote.ViewModels;

public class MainViewModel : ViewModelBase
{
    IFileSaver _fileSaver;
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

    public MainViewModel(IFileSaver fileSaver)
    {
        _fileSaver = fileSaver;
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

        await _fileSaver.SaveAsync(note);
    }
}
