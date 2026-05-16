using System.Reflection;
using Godot;

public partial class NpcController : Area2D
{
    [Export] public NPC NpcData { get; set; }
    [Export] public float GreetingDelay { get; set; } = 0.35f;

    private RequestSystem _requestSystem;
    private LexiconManager _wordSystem;

    private PackedScene _textBubbleScene;
    private SpeechBubble _activeBubble;

    public override async void _Ready()
    {
        Sprite2D sprite = GetNode<Sprite2D>("NpcSprite");

        _requestSystem = GetNode<RequestSystem>("/root/RequestSystem");

        _wordSystem = GetNode<LexiconManager>("/root/LexiconManager");

        _textBubbleScene =
              GD.Load<PackedScene>(
                  "res://scenes/ui/SpeechBubble.tscn"
              );

        if (NpcData != null && NpcData.NpcTexture != null)
        {
            sprite.Texture = NpcData.NpcTexture;
        }

        await ToSignal(GetTree().CreateTimer(GreetingDelay), "timeout");

        ShowDialogue(
            NpcData.GreetingDialogue
        );
    }

    public void OnBodyEntered(Node body)
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

            GD.Print($"Registering exposure for {NpcData.DesiredItem.ForeignWord}");
            GD.Print($"Desired item ID: {NpcData.DesiredItem.Id}");
            _wordSystem.RegisterExposure(
                NpcData.DesiredItem.Id
            );

            string requestDialogue =
            NpcData.RequestDialogue.Replace(
                "{item}",
                NpcData.DesiredItem.ForeignWord
            );

            ShowDialogue(requestDialogue);
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
                heldItem.Id
            );

            ShowDialogue(
                NpcData.SuccessDialogue
            );

            _requestSystem.CompleteRequest();
        }
        else
        {
            string wrongDialogue =
                NpcData.WrongItemDialogue.Replace(
                    "{item}",
                    heldItem.ForeignWord
                );

            ShowDialogue(wrongDialogue);
        }
    }

    private void ShowDialogue(string text)
    {
        if (_activeBubble != null)
        {
            _activeBubble.QueueFree();
        }

        _activeBubble =
            _textBubbleScene.Instantiate<SpeechBubble>();

        GetNode<Node2D>("BubbleAnchor")
            .AddChild(_activeBubble);

        _activeBubble.ShowText(text);
    }
}