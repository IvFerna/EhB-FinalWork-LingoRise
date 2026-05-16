using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class LexiconSaveData : Resource
{
    [Export]
    public Godot.Collections.Dictionary<string, int> WordExposure { get; set; } = new();

    [Export]
    public Godot.Collections.Array<string> UnlockedWords { get; set; } = new();
}