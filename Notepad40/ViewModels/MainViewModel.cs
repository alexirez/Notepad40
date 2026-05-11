using System.Collections.ObjectModel;
using System.Windows.Input;
using Notepad40.Models;
using Notepad40.Views;
using Notepad40.Services;

namespace Notepad40.ViewModels;

public class MainViewModel : ViewModelBase
{
    INoteService _noteService;
    ISettingsManager _settingsManager;
    public event EventHandler? OpenSettingsRequested;
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
    private double _fontSize;
    public double FontSize
    {
        get => _fontSize;
        set
        {
            if (_fontSize == value) return;
            _fontSize = value;
            OnPropertyChanged();
        }
    }

    public ICommand SaveNoteCommand { get; }
    public ICommand NewNoteCommand { get; }
    public ICommand DeleteNoteCommand { get; }
    public ICommand OpenSettingsCommand { get; }

    public MainViewModel(INoteService noteService, ISettingsManager settingsManager)
    {
        _settingsManager = settingsManager;
        _noteService = noteService;
        SaveNoteCommand = new Command(async () => await SaveNoteAsync());
        DeleteNoteCommand = new Command(
            execute: async () => await DeleteSelectedNoteAsync(),
            canExecute: () => SelectedNote != null
        );
        NewNoteCommand = new Command(NewNote);
        OpenSettingsCommand = new Command(async () => await OnOpenSettingsRequested());

        // subscribe to changes in the settings
        _fontSize = _settingsManager.Settings.FontSize; // initial value

        _settingsManager.Settings.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(AppSettings.FontSize))
                FontSize = _settingsManager.Settings.FontSize;
        };
    }

    public async Task OnOpenSettingsRequested()
    {
        OpenSettingsRequested?.Invoke(this, EventArgs.Empty);
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

    private void NewNote()
    {
        SelectedNote = null;
        NoteText = string.Empty;
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
