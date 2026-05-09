using Godot;

public partial class SpeechBubble : Control
{
    private Label _label;

    public override void _Ready()
    {
        _label = GetNode<Label>("Panel/TextLabel");
        Hide();
    }

    public void ShowText(string text)
    {
        _label.Text = text;
        Show();
    }

    public void HideText()
    {
        Hide();
    }
}