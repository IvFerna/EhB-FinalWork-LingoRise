using Godot;

public partial class Item : Area2D
{
    [Export] public InventoryItem ItemData { get; set; }

    private Sprite2D _sprite;
    private WordSystem _wordSystem;

    public override void _Ready()
    {
        _sprite = GetNode<Sprite2D>("ItemSprite");

        _wordSystem = GetNode<WordSystem>(
            "/root/Main/Systems/WordSystem"
        );

        if (ItemData != null && ItemData.Icon != null)
        {
            _sprite.Texture = ItemData.Icon;
        }
    }

    public void OnBodyEntered(Node body)
    {
        if (body is not PlayerController player)
            return;

        bool added = player.AddToInventory(this);

        if (!added)
            return;

        GD.Print($"Picked up {ItemData.ItemName}");

        _wordSystem.RegisterExposure(
            ItemData.TargetLanguageWord
        );

        var requestSystem =
            GetNode<RequestSystem>(
                "/root/Main/Systems/RequestSystem"
            );

        if (requestSystem.CurrentRequestedItem == ItemData)
        {
            requestSystem.MarkItemCollected();
        }

        Visible = false;
        SetDeferred("Monitoring", false);
    }

    public void ReturnToWorld()
    {
        Visible = true;
        SetDeferred("Monitoring", true);
    }
}