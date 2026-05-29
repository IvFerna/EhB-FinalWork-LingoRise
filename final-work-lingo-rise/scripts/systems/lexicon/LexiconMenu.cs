using Godot;
using System;

public partial class LexiconMenu : Control
{


    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("toggle_lexicon"))
        {
            Visible = !Visible;
            GD.Print("Toggled Lexicon Menu: " + Visible);
        }
    }

    private void OnClosePressed()
    {
        UIManager.Instance.CloseAllMenus();
    }

    private void onBackPressed()
    {
        UIManager.Instance.CloseCurrentSubmenu(this);
    }

}
