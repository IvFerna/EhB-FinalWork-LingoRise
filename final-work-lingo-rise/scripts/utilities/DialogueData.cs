using Godot;

[GlobalClass]
public partial class DialogueData : Resource
{
    [Export(PropertyHint.MultilineText)]
    public string Text;

    [Export]
    public Godot.Collections.Array<LexiconEntry> ExposedWords = new();
}