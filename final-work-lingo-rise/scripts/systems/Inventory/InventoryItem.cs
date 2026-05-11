using Godot;



public enum WordCategory
{
    Object,
    Food,
    Verb
}

[GlobalClass]
public partial class InventoryItem : Resource, ILexiconEntry
{
    [Export] public string Id { get; set; }
    [Export] public string ItemName { get; set; }
    [Export] public string Description { get; set; }
    [Export] public Texture2D Icon { get; set; }
    [Export] public string ForeignWord { get; set; }
    [Export] public string NativeTranslation { get; set; }
    [Export] public int Quantity { get; set; } = 1;

    [Export] public WordCategory Category { get; set; }

}