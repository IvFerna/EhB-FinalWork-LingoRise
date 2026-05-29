using System.Collections.Generic;
using System.Reflection;
using Godot;

public partial class NpcController : Area2D
{
    [Export] public NPC NpcData { get; set; }
    [Export] public float GreetingDelay { get; set; } = 0.35f;

    private RequestSystem _requestSystem;
    private LexiconManager _wordSystem;
    private DialogueManager _dialogueManager;
    private Node2D _bubbleAnchor;


    public override async void _Ready()
    {
        Sprite2D sprite = GetNode<Sprite2D>("NpcSprite");

        _requestSystem = GetNode<RequestSystem>("/root/RequestSystem");

        _wordSystem = GetNode<LexiconManager>("/root/LexiconManager");
        _dialogueManager = GetNode<DialogueManager>("/root/DialogueManager");


        if (NpcData != null && NpcData.NpcTexture != null)
        {
            sprite.Texture = NpcData.NpcTexture;
        }

        await ToSignal(GetTree().CreateTimer(GreetingDelay), "timeout");

        _bubbleAnchor = GetNode<Node2D>("BubbleAnchor");

        await _dialogueManager.ShowDialogueWithExposure(_bubbleAnchor, NpcData.GreetingDialogue, DialoguePriority.Gameplay);
    }

    public async void OnBodyEntered(Node body)
    {
        if (body is not PlayerController player)
            return;
        GD.Print($"Interacted with {NpcData.NpcName}");
        // Start request if no active request
        if (_requestSystem.CurrentRequestedItem == null)
        {

            GD.Print($"Starting request for {NpcData.DesiredItem.ItemName}");
            _requestSystem.StartRequest(
                NpcData.DesiredItem
            );

            GD.Print($"Registering exposure for {NpcData.DesiredItem.VocabularyEntry.ForeignWord}");
            GD.Print($"Desired item ID: {NpcData.DesiredItem.Id}");
            _wordSystem.RegisterExposure(
                NpcData.DesiredItem.VocabularyEntry.Id,
                ExposureType.Heard
            );

            await _dialogueManager.ShowDialogueWithExposure(
                _bubbleAnchor,
                NpcData.RequestDialogue,
                DialoguePriority.Gameplay
            );
            return;
        }


        // Try delivery
        InventoryItem heldItem = player.GetHeldItem();

        if (heldItem == null)
            return;

        if (_requestSystem.ValidateItem(heldItem))
        {
            GD.Print($"Delivered {heldItem.ItemName}");

            player.RemoveFromInventory(heldItem.Id);

            _wordSystem.RegisterExposure(
                heldItem.VocabularyEntry.Id,
                ExposureType.Interacted
            );

            await _dialogueManager.ShowDialogueWithExposure(
                _bubbleAnchor,
                NpcData.SuccessDialogue,
                DialoguePriority.Gameplay
            );

            _requestSystem.CompleteRequest();
        }
        else
        {
            await _dialogueManager.ShowDialogueWithExposure(
                _bubbleAnchor,
                NpcData.WrongItemDialogue,
                DialoguePriority.Gameplay,
                new Dictionary<string, string>
                {
                    { "item", heldItem.VocabularyEntry.ForeignWord }
                }
            );
        }
    }
}
