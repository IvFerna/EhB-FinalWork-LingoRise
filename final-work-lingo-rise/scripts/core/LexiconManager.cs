using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class LexiconManager : Node
{
    private const int UnlockThreshold = 3;

    private Dictionary<string, int> _wordExposure = new();
    private HashSet<string> _unlockedWords = new();

    public event Action<string> OnWordUnlocked;

    public void RegisterExposure(string wordId)
    {
        if (!_wordExposure.ContainsKey(wordId))
            _wordExposure[wordId] = 0;

        _wordExposure[wordId]++;

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
            OnWordUnlocked?.Invoke(wordId);
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
}