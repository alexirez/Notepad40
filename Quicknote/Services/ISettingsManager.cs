using Quicknote.Models;

namespace Quicknote.Services;

public interface ISettingsManager
{
    AppSettings Settings { get; }

    void Load();
    void Save();
}