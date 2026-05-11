using System.ComponentModel;

namespace Notepad40.Models;

public class AppSettings : INotifyPropertyChanged
{
    private string _notesDirectory = "";
    public string NotesDirectory
    {
        get => _notesDirectory;
        set
        {
            if (_notesDirectory != value)
            {
                _notesDirectory = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NotesDirectory)));
            }
        }
    }
    private double _fontSize = 14;
    public double FontSize
    {
        get => _fontSize;
        set
        {
            if (_fontSize != value)
            {
                _fontSize = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FontSize)));
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}