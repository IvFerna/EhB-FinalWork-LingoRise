using Godot;

public partial class Item : Area2D
{
    [Export] public InventoryItem ItemData { get; set; }

    private Sprite2D _sprite;

    public override void _Ready()
    {
        _sprite = GetNode<Sprite2D>("ItemSprite");

        if (ItemData != null && ItemData.Icon != null)
        {
            _sprite.Texture = ItemData.Icon;
        }
    }

    public void OnBodyEntered(Node2D body)
    {
        if (body is PlayerController player)
        {
            bool added = player.AddToInventory(ItemData);

            if (added)
            {
                GD.Print($"Picked up {ItemData.ItemName}");
                QueueFree();
            }
        }
    }
}