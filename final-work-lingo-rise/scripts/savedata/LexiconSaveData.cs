using System.Collections.Generic;

public class LexiconSaveData
{
    public Dictionary<string, WordProgressData> WordProgress { get; set; } = new();

    public List<string> UnlockedWords { get; set; } = new();
}