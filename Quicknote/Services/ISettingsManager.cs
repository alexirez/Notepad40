using Notepad40.Models;

namespace Notepad40.Services;

public interface ISettingsManager
{
    AppSettings Settings { get; }

    void Load();
    void Save();
}