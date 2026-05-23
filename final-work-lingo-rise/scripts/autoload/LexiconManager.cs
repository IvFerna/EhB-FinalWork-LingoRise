using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class LexiconManager : Node
{
    private const int UnlockThreshold = 3;
    private Dictionary<string, int> _wordExposure = new();
    private HashSet<string> _unlockedWords = new();
    private ILexiconRepository _repository;

    public event Action<string> OnWordUnlocked;
    public event Action OnLexiconUpdated;

    public override void _Ready()
    {
        LoadProgress();

        _repository = GetNode<LocalLexiconRepository>("/root/LocalLexiconRepository");
    }

    public void RegisterExposure(string wordId)
    {
        if (string.IsNullOrEmpty(wordId))
            return;

        if (!_wordExposure.ContainsKey(wordId))
            _wordExposure[wordId] = 0;

        _wordExposure[wordId]++;

        SaveProgress();
        CheckUnlock(wordId);
    }

    private void CheckUnlock(string wordId)
    {
        if (_wordExposure[wordId] >= UnlockThreshold &&
            !_unlockedWords.Contains(wordId))
        {
            UnlockWord(wordId);
        }
    }

    public void UnlockWord(string wordId)
    {
        if (_unlockedWords.Add(wordId))
        {
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
        return _wordExposure.GetValueOrDefault(wordId, 0);
    }

    public IReadOnlyCollection<string> GetUnlockedWords()
    {
        return _unlockedWords;
    }

    private const string SavePath = "user://lexicon_save.tres";
    public void SaveProgress()
    {
        var saveData = new LexiconSaveData();

        foreach (var entry in _wordExposure)
        {
            saveData.WordExposure[entry.Key] = entry.Value;
        }

        foreach (var wordId in _unlockedWords)
        {
            saveData.UnlockedWords.Add(wordId);
        }

        ResourceSaver.Save(saveData, SavePath);
    }

    public void LoadProgress()
    {
        if (!ResourceLoader.Exists(SavePath))
            return;

        var saveData = ResourceLoader.Load<LexiconSaveData>(SavePath);

        if (saveData == null)
            return;

        _wordExposure.Clear();
        _unlockedWords.Clear();

        foreach (var entry in saveData.WordExposure)
        {
            _wordExposure[entry.Key.ToString()] = entry.Value;
        }

        foreach (string wordId in saveData.UnlockedWords)
        {
            _unlockedWords.Add(wordId);
        }

        GD.Print("=== LEXICON SAVE DATA ===");

        foreach (var exposure in _wordExposure)
        {
            GD.Print($"Exposure: {exposure.Key} = {exposure.Value}");
        }

        foreach (var unlocked in _unlockedWords)
        {
            GD.Print($"Unlocked: {unlocked}");
        }

        GD.Print("=========================");
    }

    public void ResetProgress()
    {
        _wordExposure.Clear();
        _unlockedWords.Clear();

        if (ResourceLoader.Exists(SavePath))
        {
            DirAccess.RemoveAbsolute(SavePath);
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

        foreach (var entry in _wordExposure)
        {
            GD.Print($"{entry.Key}: {entry.Value}");
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

            if (!_wordExposure.ContainsKey(entry.Id))
            {
                _wordExposure[entry.Id] = UnlockThreshold;
            }
        }
        OnLexiconUpdated?.Invoke();
        SaveProgress();

        GD.Print("All lexicon words unlocked.");
    }
}