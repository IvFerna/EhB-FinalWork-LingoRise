using Godot;
using System;

public partial class PlayerController : CharacterBody2D
{
    public static PlayerController Instance { get; private set; }
    [Export] private float moveSpeed = 200f;
    private bool _canMove = true;
    private Vector2 ScreenSize;

    public override void _Ready()
    {
        if (Instance != null && Instance != this)
        {
            QueueFree();
            return;
        }
        Instance = this;

        ScreenSize = GetViewportRect().Size;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!_canMove)
        {
            GD.Print("You cannot move");
            return;
        }

        Vector2 input = new Vector2(
        Input.GetActionStrength("right") - Input.GetActionStrength("left"),
        Input.GetActionStrength("down") - Input.GetActionStrength("up")
        );

        // Normalize so diagonal isn't faster than cardinal
        Velocity = input.Normalized() * (input.Length() > 0 ? moveSpeed : 0);
        MoveAndSlide();
    }

}

