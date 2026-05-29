using Godot;
using System;

[GlobalClass]
public partial class NPC : Resource
{
    [Export] public string NpcName { get; set; }
    [Export] public DialogueData GreetingDialogue;
    [Export] public DialogueData RequestDialogue;
    [Export] public DialogueData WrongItemDialogue;
    [Export] public DialogueData SuccessDialogue;
    [Export] public DialogueData GoodbyeDialogue;
    [Export] public Texture2D NpcTexture { get; set; }
    [Export] public InventoryItem DesiredItem { get; set; }
}
