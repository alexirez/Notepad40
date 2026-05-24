using System.ComponentModel;
using System.Runtime.CompilerServices;

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
                OnPropertyChanged();            
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
                OnPropertyChanged();
            }
        }
    }

    private string _fontFamily = "Default";
    public string FontFamily
    {
        get => _fontFamily;
        set
        {
            if (_fontFamily != value)
            {
                _fontFamily = value;
                OnPropertyChanged();
            }
        }
    }

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    public event PropertyChangedEventHandler? PropertyChanged;
}