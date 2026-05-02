using Godot;
using System;

[GlobalClass]
public partial class NPC : Resource
{
    [Export] public string NpcName { get; set; }
    [Export] public string Dialogue { get; set; }
    [Export] public Texture2D NpcTexture { get; set; }
    [Export] public InventoryItem DesiredItem { get; set; }
}
