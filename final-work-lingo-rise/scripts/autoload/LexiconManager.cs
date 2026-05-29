using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

public enum ExposureType
{
    Heard,
    Seen,
    Interacted
}

public partial class LexiconManager : Node
{
    private const int UnlockThreshold = 3;
    private Dictionary<string, WordProgressData> _wordProgress = new();
    private HashSet<string> _unlockedWords = new();
    private ILexiconRepository _repository;

    public event Action<string> OnWordUnlocked;
    public event Action OnLexiconUpdated;

    public override void _Ready()
    {
        LoadProgress();

        _repository = GetNode<LocalLexiconRepository>("/root/LocalLexiconRepository");
        
    }

    public void RegisterExposure(
        string wordId,
        ExposureType type
    )
    {
        if (string.IsNullOrEmpty(wordId))
            return;

        if (!_wordProgress.ContainsKey(wordId))
        {
            _wordProgress[wordId] =
                new WordProgressData();
        }

        var progress = _wordProgress[wordId];



        switch (type)
        {
            case ExposureType.Heard:
                progress.HeardCount++;
                break;

            case ExposureType.Seen:
                progress.SeenCount++;
                break;

            case ExposureType.Interacted:
                progress.InteractionCount++;
                break;
        }

        CheckUnlock(wordId);

        SaveProgress();
    }

    private void CheckUnlock(string wordId)
    {
        if (_wordProgress[wordId].TotalExposureCount >= UnlockThreshold &&
            !_unlockedWords.Contains(wordId))
        {
            UnlockWord(wordId);
        }
    }

    public void UnlockWord(string wordId)
    {
        if (_unlockedWords.Add(wordId))
        {
            if (_wordProgress.ContainsKey(wordId))
            {
                _wordProgress[wordId].IsUnlocked = true;
            }

            SaveProgress();

            OnWordUnlocked?.Invoke(wordId);
            OnLexiconUpdated?.Invoke();

            GD.Print($"Unlocked word: {wordId}");
        }
    }

    public bool IsUnlocked(string wordId)
    {
        return _unlockedWords.Contains(wordId);
    }

    public int GetExposureCount(string wordId)
    {
        return _wordProgress.GetValueOrDefault(wordId, new WordProgressData()).TotalExposureCount;
    }

    public IReadOnlyCollection<string> GetUnlockedWords()
    {
        return _unlockedWords;
    }

    private const string SavePath = "user://lexicon_save.json";
    public void SaveProgress()
    {
        var saveData = new LexiconSaveData
        {
            WordProgress = _wordProgress,
            UnlockedWords = _unlockedWords.ToList()
        };

        string json = JsonSerializer.Serialize(
            saveData,
            new JsonSerializerOptions
            {
                WriteIndented = true
            });

        using var file = FileAccess.Open(
            SavePath,
            FileAccess.ModeFlags.Write);

        file.StoreString(json);

        GD.Print("Lexicon progress saved.");
    }

    public void LoadProgress()
    {
        if (!FileAccess.FileExists(SavePath))
        {
            GD.Print("No lexicon save found.");
            return;
        }

        using var file = FileAccess.Open(
            SavePath,
            FileAccess.ModeFlags.Read);

        string json = file.GetAsText();

        var saveData = JsonSerializer.Deserialize<LexiconSaveData>(json);

        if (saveData == null)
        {
            GD.PrintErr("Failed to deserialize lexicon save.");
            return;
        }

        _wordProgress = saveData.WordProgress ?? new();
        _unlockedWords = saveData.UnlockedWords != null
            ? new HashSet<string>(saveData.UnlockedWords)
            : new HashSet<string>();

        GD.Print("=== LEXICON SAVE DATA ===");

        foreach (var progress in _wordProgress)
        {
            GD.Print(
                $"Progress: {progress.Key} = {progress.Value.TotalExposureCount}");
        }

        foreach (var unlocked in _unlockedWords)
        {
            GD.Print($"Unlocked: {unlocked}");
        }

        GD.Print("=========================");
    }

    public void ResetProgress()
    {
        _wordProgress.Clear();
        _unlockedWords.Clear();

        if (FileAccess.FileExists(SavePath))
        {
            DirAccess.RemoveAbsolute(ProjectSettings.GlobalizePath(SavePath));
        }
        OnLexiconUpdated?.Invoke();
    }

    public ILexiconEntry GetEntry(string id)
    {
        return _repository.GetById(id);
    }

    public void DebugPrintState()
    {
        GD.Print("=== LEXICON MANAGER ===");

        GD.Print("-- Exposure --");

        foreach (var entry in _wordProgress)
        {
            GD.Print($"{entry.Key}: {entry.Value.TotalExposureCount}");
        }

        GD.Print("-- Unlocked --");

        foreach (var word in _unlockedWords)
        {
            GD.Print(word);
        }

        GD.Print("=======================");
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("debug_lexicon_print"))
        {
            DebugPrintState();
        }

        if (Input.IsActionJustPressed("debug_lexicon_reset"))
        {
            ResetProgress();

            GD.Print("Lexicon progress reset.");
        }
        if (Input.IsActionJustPressed("debug_lexicon_unlock_all"))
        {
            UnlockAllWords();
        }
    }

    public IEnumerable<ILexiconEntry> GetUnlockedEntries()
    {
        return _unlockedWords
            .Select(id => _repository.GetById(id))
            .Where(entry => entry != null);
    }
    public IEnumerable<ILexiconEntry> GetUnlockedEntriesByCategory(WordCategory category)
    {
        return GetUnlockedEntries()
            .Where(entry => entry.Category == category);
    }

    public IEnumerable<ILexiconEntry> GetEntriesByCategory(WordCategory category)
    {
        return _repository
            .GetAll()
            .Where(entry => entry.Category == category);
    }
    public int GetUnlockedCount(WordCategory category)
    {
        return GetUnlockedEntriesByCategory(category).Count();
    }
    public int GetTotalCount(WordCategory category)
    {
        return GetEntriesByCategory(category).Count();
    }
    public void UnlockAllWords()
    {
        foreach (var entry in _repository.GetAll())
        {
            _unlockedWords.Add(entry.Id);

            if (!_wordProgress.ContainsKey(entry.Id))
            {
                _wordProgress[entry.Id] = new WordProgressData();
            }
        }
        OnLexiconUpdated?.Invoke();
        SaveProgress();

        GD.Print("All lexicon words unlocked.");
    }
}