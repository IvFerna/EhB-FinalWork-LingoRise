using Godot;
using System;

public partial class NpcController : Area2D
{
    [Export] public string NpcName { get; set; }
    [Export] public string Dialogue { get; set; }

    public void OnBodyEntered(Node2D body)
    {
        if (body.IsInGroup("Player"))

        {
            GD.Print($"Interacting with {NpcName}: {Dialogue}");
            // Here you can add code to display the dialogue in the game UI
        }
    }
}
