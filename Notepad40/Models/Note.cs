using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Notepad40.Models;

public class Note : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    private string _title = "";
    public string Title
    {
        get => _title;
        set
        {
            if (_title == value) return;
            _title = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(Preview));
        }
    }
    private string _content = "";
    public string Content
    {
        get => _content;
        set
        {
            if (_content == value) return;
            _content = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(Preview));
        }
    }
    public Guid Id { get; set; }
    public DateTime CreatedUtc { get; set; }
    public DateTime ModifiedUtc { get; set; }
    public string Preview => !string.IsNullOrWhiteSpace(Title) ? Title : Content;

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
