using System.Collections.ObjectModel;
using System.Windows.Input;
using Quicknote.Models;
using Quicknote.Services;

namespace Quicknote.ViewModels;

public class MainViewModel : ViewModelBase
{
    INoteService _noteService;
    ISettingsManager _settingsManager;
    public ObservableCollection<Note> Notes { get; } = [];
    private Note? _selectedNote;
    public Note? SelectedNote
    {
        get => _selectedNote;
        set
        {
            if (_selectedNote == value) return;
            _selectedNote = value;
            OnPropertyChanged();

            NoteText = _selectedNote?.Content ?? string.Empty;
            ((Command)DeleteNoteCommand).ChangeCanExecute();
        }
    }
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
    public ICommand DeleteNoteCommand { get; }

    public MainViewModel(INoteService noteService, ISettingsManager settingsManager)
    {
        _settingsManager = settingsManager;
        _noteService = noteService;
        SaveNoteCommand = new Command(async () => await SaveNoteAsync());
        DeleteNoteCommand = new Command(
            execute: async () => await DeleteSelectedNoteAsync(),
            canExecute: () => SelectedNote != null
        );
    }

    public async Task InitializeAsync()
    /*Performs all work related to initializing the mainView*/
    {
        await InitializeNotesAsync();
    }

    public async Task InitializeNotesAsync()
    /*Load all notes into memory*/
    {
        IReadOnlyList<Note> notesFromDisk = await _noteService.LoadAllAsync();

        Notes.Clear();
        foreach (Note note in notesFromDisk)
            Notes.Add(note);
    }

    private async Task SaveNoteAsync()
    /*Save a note onto disk*/
    {
        if (string.IsNullOrWhiteSpace(NoteText)) return;

        if (SelectedNote == null)
        {
            var note = new Note
            {
                Id = Guid.NewGuid(),
                Content = NoteText,
                CreatedUtc = DateTime.UtcNow,
                ModifiedUtc = DateTime.UtcNow
            };

            Notes.Add(note);
            await _noteService.SaveAsync(note);
        }
        else
        {
            SelectedNote.Content = NoteText; // Note.Content raises PropertyChanged to update UI
            SelectedNote.ModifiedUtc = DateTime.UtcNow;

            await _noteService.UpdateAsync(SelectedNote);
        }
    }

    public async Task DeleteSelectedNoteAsync()
    /*Delete note from memory, then remove from Notes list to update UI*/
    {
        if (_selectedNote == null) return;
        await _noteService.DeleteAsync(_selectedNote.Id);

        Notes.Remove(_selectedNote);
        SelectedNote = null;
    }
}
