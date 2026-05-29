using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class LocalLexiconRepository : Node, ILexiconRepository
{
    private readonly Dictionary<string, ILexiconEntry> _entries = new();

    public override void _Ready()
    {
        LoadEntries();
    }

    private void LoadEntries()
    {
        var database = GD.Load<LexiconDatabase>(
            "res://resources/Lexicon/LexiconDatabase.tres");

        if (database == null)
        {
            GD.PrintErr("Failed to load lexicon database.");
            return;
        }

        foreach (var resource in database.Entries)
        {
            if (resource is ILexiconEntry entry &&
                !string.IsNullOrEmpty(entry.Id))
            {
                _entries[entry.Id] = entry;
            }
        }

        GD.Print("=== LEXICON REPOSITORY ===");

        GD.Print($"Loaded {_entries.Count} lexicon entries.");

        foreach (var entry in _entries.Values)
        {
            GD.Print(
                $"ID: {entry.Id} | " +
                $"Word: {entry.ForeignWord} | " +
                $"Category: {entry.Category}"
            );
        }

        GD.Print("==========================");
    }

    // private void LoadResourcesFromFolder(string path)
    // {
    //     var dir = DirAccess.Open(path);

    //     if (dir == null)
    //         return;

    //     dir.ListDirBegin();

    //     while (true)
    //     {
    //         var fileName = dir.GetNext();

    //         // GD.Print($"Found file: {fileName} in {path}");
    //         if (string.IsNullOrEmpty(fileName))
    //             break;

    //         if (dir.CurrentIsDir())
    //             continue;

    //         if (!fileName.EndsWith(".tres"))
    //             continue;

    //         var fullPath = path + fileName;

    //         var resource = ResourceLoader.Load<Resource>(fullPath);
    //         GD.Print($"Loaded resource: {fullPath} | Type: {resource.GetType()}");

    //         if (resource is ILexiconEntry entry && !string.IsNullOrEmpty(entry.Id))
    //         {
    //             _entries[entry.Id] = entry;
    //         }
    //     }


    //     dir.ListDirEnd();
    // }

    public ILexiconEntry GetById(string id)
    {
        return _entries.GetValueOrDefault(id);
    }

    public IEnumerable<ILexiconEntry> GetAll()
    {
        return _entries.Values;
    }

    public IEnumerable<ILexiconEntry> GetByCategory(WordCategory category)
    {
        return _entries.Values.Where(e => e.Category == category);
    }
}