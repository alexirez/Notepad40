using System.Collections.ObjectModel;
using System.Text.Json;
using System.Windows.Input;
using Quicknote.Models;
using Quicknote.Services;

namespace Quicknote.ViewModels;

public class MainViewModel : ViewModelBase
{
    INoteService _noteService;
    ISettingsManager _settingsManager;
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
    public double FontSize
    {
        get => _settingsManager.Settings.FontSize;
        set
        {
            if (_settingsManager.Settings.FontSize == value) return;
            _settingsManager.Settings.FontSize = value;
            OnPropertyChanged();
            _settingsManager.Save(); // immediately save changed settings locally
        }
    }

    public ICommand SaveNoteCommand { get; }

    public MainViewModel(INoteService noteService, ISettingsManager settingsManager)
    {
        _settingsManager = settingsManager;
        _noteService = noteService;
        SaveNoteCommand = new Command(async () => await SaveNoteAsync());
        _ = InitializeNotesAsync(); // Load notes from memory
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

    public async Task InitializeAsync()
    /*This method is used to Initialize the viewModel in App.xaml.cs*/
    {
        await InitializeNotesAsync();
    }
    public async Task InitializeNotesAsync()
    {
        IReadOnlyList<Note> notesFromDisk = await _noteService.LoadAllAsync();

        Notes.Clear();
        foreach (Note note in notesFromDisk)
            Notes.Add(note);
    }
}
