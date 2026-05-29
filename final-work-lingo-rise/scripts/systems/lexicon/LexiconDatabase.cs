using Godot;

[GlobalClass]
public partial class LexiconDatabase : Resource
{
    [Export]
    public Godot.Collections.Array<Resource> Entries = new();
}