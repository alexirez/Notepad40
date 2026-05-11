using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Notepad40.Models;

public class Note : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    private string _content = "";
    public string Content
    {
        get => _content;
        set
        {
            if (_content == value) return;
            _content = value;
            OnPropertyChanged();
        }
    }
    public Guid Id { get; set; }
    public string Title { get; set; } = "";
    public DateTime CreatedUtc { get; set; }
    public DateTime ModifiedUtc { get; set; }

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
