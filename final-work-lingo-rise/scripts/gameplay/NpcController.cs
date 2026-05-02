using Godot;
using System;

public partial class NpcController : Area2D
{
    [Export] public NPC NpcData { get; set; }

    public override void _Ready()
    {
        Sprite2D sprite = GetNode<Sprite2D>("NpcSprite");
        if (NpcData != null && NpcData.NpcTexture != null)
        {
            sprite.Texture = NpcData.NpcTexture;
        }
        GD.Print(NpcData.NpcName + " says: " + NpcData.Dialogue);
    }
    public void OnBodyEntered(Node2D body)
    {
        if (body is PlayerController player)
        {
            if (player.HasItem("bread"))
            {
                GD.Print("Gifted bread to " + NpcData.NpcName);
                player.RemoveFromInventory("bread");
            }
            else
            {
                GD.Print(NpcData.NpcName + " says: " + NpcData.Dialogue);
            }
        }
    }
}
