using Godot;
using System;

[GlobalClass]
public partial class NPC : Resource
{
    [Export] public string NpcName { get; set; }
    [Export] public string GreetingDialogue;
    [Export] public string RequestDialogue;
    [Export] public string WrongItemDialogue;
    [Export] public string SuccessDialogue;
    [Export] public string GoodbyeDialogue;
    [Export] public Texture2D NpcTexture { get; set; }
    [Export] public InventoryItem DesiredItem { get; set; }
}
