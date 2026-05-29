using Godot;

public partial class AmbientNpcController : Area2D
{
    [Export]
    public AmbientNPC NpcData;

    private LexiconManager _wordSystem;

    private Node2D _bubbleAnchor;

    private Sprite2D _sprite;

    public override void _Ready()
    {
        _wordSystem =
            GetNode<LexiconManager>(
                "/root/LexiconManager"
            );

        _bubbleAnchor =
            GetNode<Node2D>("BubbleAnchor");

        _sprite =
            GetNode<Sprite2D>("NpcSprite");

        if (NpcData?.NpcTexture != null)
        {
            _sprite.Texture = NpcData.NpcTexture;
        }

        StartAmbientDialogueLoop();
    }

    private async void StartAmbientDialogueLoop()
    {
        while (true)
        {
            float randomOffset = (float)GD.RandRange(-2f, 2f);

            await ToSignal(
                GetTree().CreateTimer(
                   NpcData.SpeakInterval + randomOffset
                ),
                "timeout"
            );

            if (DialogueManager.Instance.IsDialogueActive)
                continue;

            if (!DialogueManager.Instance.CanPlayDialogue)
                continue;

            await SpeakDialogue();
        }
    }

    private async System.Threading.Tasks.Task SpeakDialogue()
    {
        if (NpcData?.Dialogue == null)
            return;

        foreach (var word in NpcData.Dialogue.ExposedWords)
        {
            _wordSystem.RegisterExposure(
                word.Id,
                ExposureType.Heard
            );
        }

        await DialogueManager.Instance.ShowDialogue(
            _bubbleAnchor,
            "es",
            NpcData.Dialogue.Text,
            DialoguePriority.Ambient
        );
    }
}