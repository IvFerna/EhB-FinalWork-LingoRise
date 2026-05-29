using Godot;



public enum WordCategory
{
    Noun,
    Adjective,
    Verb
}

[GlobalClass]
public partial class InventoryItem : Resource
{
    [Export] public string Id { get; set; }
    [Export] public string ItemName { get; set; }
    [Export] public string Description { get; set; }
    [Export] public Texture2D Icon { get; set; }
    [Export] public int Quantity { get; set; } = 1;
    [Export] public LexiconEntry VocabularyEntry;


}