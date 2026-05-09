using Godot;

[GlobalClass]
public partial class InventoryItem : Resource
{
    [Export] public string ItemId { get; set; }
    [Export] public string ItemName { get; set; }
    [Export] public string Description { get; set; }
    [Export] public Texture2D Icon { get; set; }
    [Export] public string TargetLanguageWord { get; set; }
    [Export] public string NativeLanguageWord { get; set; }
    [Export] public int Quantity { get; set; } = 1;
    [Export] public string Category { get; set; }
}