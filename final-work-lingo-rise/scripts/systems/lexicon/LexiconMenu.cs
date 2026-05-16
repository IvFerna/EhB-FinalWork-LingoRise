using Godot;
using System;

public partial class LexiconMenu : CanvasLayer
{
    public override void _Ready()
    {
        Visible = false;
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("toggle_lexicon"))
        {
            Visible = !Visible;
            GD.Print("Toggled Lexicon Menu: " + Visible);
        }
    }

    private void OnCloseButtonPressed()
    {
        Visible = false;
        GD.Print("Lexicon Menu closed through button.");
    }
}
