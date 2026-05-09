using Godot;
using System.Collections.Generic;

public partial class WordSystem : Node
{
    private Dictionary<string, int> _wordExposure = new();

    public void RegisterExposure(string word)
    {
        if (!_wordExposure.ContainsKey(word))
            _wordExposure[word] = 0;

        _wordExposure[word]++;
    }
}