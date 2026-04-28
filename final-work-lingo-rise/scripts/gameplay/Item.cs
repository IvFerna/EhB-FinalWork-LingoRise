using Godot;
using System;

public partial class Item : Area2D
{
    [Export] public string ItemName { get; set; }
    [Export] public string Description { get; set; }
    [Export] public Texture2D Icon { get; set; }

    private Sprite2D _sprite;

    public override void _Ready()
    {
        _sprite = GetNode<Sprite2D>("ItemSprite");
        GD.Print($"Icon: {Icon}");
        GD.Print($"Sprite: {_sprite}");

        if (Icon != null)
        {
            _sprite.Texture = Icon;
        }
    }

    public void OnBodyEntered(Node2D body)
    {
        if (body.IsInGroup("Player"))
        {
            GD.Print($"Interacting with {ItemName}");
            QueueFree();
        }
    }
}
