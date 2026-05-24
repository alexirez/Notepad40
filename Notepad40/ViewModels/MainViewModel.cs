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

            _noteTitle = _selectedNote?.Title ?? string.Empty;
            _noteText = _selectedNote?.Content ?? string.Empty;
            OnPropertyChanged(nameof(NoteTitle));
            OnPropertyChanged(nameof(NoteText));
            ((Command)DeleteNoteCommand).ChangeCanExecute();
        }
    }
    private string _noteTitle = ""; // ensure non-null by default
    public string NoteTitle
    {
        get => _noteTitle;
        set
        {
            if (_noteTitle == value) return;
            _noteTitle = value;
            OnPropertyChanged();
        }
    }
    private string _noteText = "";
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
    private string _fontFamily = "Default";
    public string FontFamily
    {
        get => _fontFamily;
        set
        {
            if (_fontFamily == value) return;
            _fontFamily = value;
            OnPropertyChanged();
        }
    }

    public ICommand SaveNoteCommand { get; }
    public ICommand NewNoteCommand { get; }
    public ICommand DeleteNoteCommand { get; }
    public ICommand OpenSettingsCommand { get; }
    public ICommand IncreaseFontSizeCommand { get; }
    public ICommand DecreaseFontSizeCommand { get; }

    public event EventHandler? NoteSaved;

    public MainViewModel(INoteService noteService, ISettingsManager settingsManager)
    {
        _settingsManager = settingsManager;
        _noteService = noteService;

        // Define ICommands
        SaveNoteCommand = new Command(async () => await SaveNoteAsync());
        DeleteNoteCommand = new Command(
            execute: async () => await DeleteSelectedNoteAsync(),
            canExecute: () => SelectedNote != null
        );
        NewNoteCommand = new Command(NewNote);
        OpenSettingsCommand = new Command(async () => await OnOpenSettingsRequested());
        IncreaseFontSizeCommand = new Command(() =>
        {
            if (FontSize < ISettingsManager.MaxFontSize)
            {
                FontSize++;
                _settingsManager.Settings.FontSize = FontSize;
                _settingsManager.Save();
            }
        });
        DecreaseFontSizeCommand = new Command(() =>
        {
            if (FontSize > ISettingsManager.MinFontSize)
            {
                FontSize--;
                _settingsManager.Settings.FontSize = FontSize;
                _settingsManager.Save();
            }
        });

        _fontSize = _settingsManager.Settings.FontSize; // initial values
        _fontFamily = _settingsManager.Settings.FontFamily;

        // Subscribe to changes in the settings
        _settingsManager.Settings.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(AppSettings.FontSize))
                FontSize = _settingsManager.Settings.FontSize;
            if (e.PropertyName == nameof(AppSettings.FontFamily))
                FontFamily = _settingsManager.Settings.FontFamily;
        };
    }

    public async Task OnOpenSettingsRequested()
    {
        OpenSettingsRequested?.Invoke(this, EventArgs.Empty);
    }

    /*Performs all work related to initializing the mainView*/
    public async Task InitializeAsync()
    {
        await InitializeNotesAsync();
    }

    /*Load all notes into memory*/
    public async Task InitializeNotesAsync()
    {
        IReadOnlyList<Note> notesFromDisk = await _noteService.LoadAllAsync();

        Notes.Clear();
        foreach (Note note in notesFromDisk)
            Notes.Add(note);
    }

    private void NewNote()
    {
        SelectedNote = null;
        NoteTitle = string.Empty;
        NoteText = string.Empty;
    }

    /*Save a note onto disk*/
    private async Task SaveNoteAsync()
    {
        if (string.IsNullOrWhiteSpace(NoteTitle) 
            && string.IsNullOrWhiteSpace(NoteText))
            return;

        if (SelectedNote == null)
        {
            var note = new Note
            {
                Id = Guid.NewGuid(),
                Title = NoteTitle,
                Content = NoteText,
                CreatedUtc = DateTime.UtcNow,
                ModifiedUtc = DateTime.UtcNow
            };

            Notes.Add(note);
            SelectedNote = note;
            await _noteService.SaveAsync(note);
        }
        else
        {
            SelectedNote.Title = NoteTitle;
            SelectedNote.Content = NoteText;
            SelectedNote.ModifiedUtc = DateTime.UtcNow;

            await _noteService.UpdateAsync(SelectedNote);
        }
        NoteSaved?.Invoke(this, EventArgs.Empty);
    }

    /*Delete note from memory, then remove from Notes list to update UI*/
    public async Task DeleteSelectedNoteAsync()    {
        if (_selectedNote == null) return;
        await _noteService.DeleteAsync(_selectedNote.Id);

        Notes.Remove(_selectedNote);
        SelectedNote = null;
    }
}
