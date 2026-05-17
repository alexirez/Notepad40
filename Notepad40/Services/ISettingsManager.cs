using Notepad40.Models;

namespace Notepad40.Services;

public interface ISettingsManager
{
    public const double MinFontSize = 8;
    public const double MaxFontSize = 28;

    AppSettings Settings { get; }

    void Load();
    void Save();
}