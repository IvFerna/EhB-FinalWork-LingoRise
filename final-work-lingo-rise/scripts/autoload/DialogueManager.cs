using Godot;
using System.Threading.Tasks;

public partial class DialogueManager : Node
{
    public static DialogueManager Instance;

    private PackedScene _speechBubbleScene;

    private SpeechBubble _activeBubble;

    public override void _Ready()
    {
        Instance = this;

        _speechBubbleScene =
            GD.Load<PackedScene>(
                "res://scenes/ui/SpeechBubble.tscn"
            );
    }

    public async Task ShowDialogue(
        Node2D anchor,
        string language,
        string text
    )
    {
        _activeBubble?.QueueFree();

        _activeBubble =
            _speechBubbleScene.Instantiate<SpeechBubble>();

        anchor.AddChild(_activeBubble);

        _activeBubble.ShowText(text);

        // Small readability delay
        await ToSignal(
            GetTree().CreateTimer(0.35f),
            "timeout"
        );

        await TTSService.Instance.PlayAudio(
            language,
            text
        );
    }
}