using System.Collections.Generic;
using System.Reflection;
using Godot;

public partial class NpcController : Area2D
{
    [Export] public NPC NpcData { get; set; }
    [Export] public float GreetingDelay { get; set; } = 0.35f;
    [Export] public Marker2D _bubbleAnchor;

    protected LexiconManager _wordSystem;
    protected DialogueManager _dialogueManager;

    public override async void _Ready()
    {
        Sprite2D sprite = GetNode<Sprite2D>("NpcSprite");

        _wordSystem = GetNode<LexiconManager>("/root/LexiconManager");
        _dialogueManager = GetNode<DialogueManager>("/root/DialogueManager");

        if (NpcData != null && NpcData.NpcTexture != null)
        {
            sprite.Texture = NpcData.NpcTexture;
        }

        await ToSignal(GetTree().CreateTimer(GreetingDelay), "timeout");

        if (_bubbleAnchor == null)
        {
            _bubbleAnchor = GetNode<Marker2D>("BubbleAnchor");
        }

        await _dialogueManager.ShowDialogueWithExposure(
            _bubbleAnchor,
            NpcData.GreetingDialogue,
            DialoguePriority.Gameplay
        );
    }
}