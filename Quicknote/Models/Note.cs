namespace Quicknote.Models;

public class Note
{
    public Guid Id { get; set; }
    public string Title { get; set; } = "";
    public string Content { get; set; } = "";
    public DateTime CreatedUtc { get; set; }
    public DateTime ModifiedUtc { get; set; }
}
