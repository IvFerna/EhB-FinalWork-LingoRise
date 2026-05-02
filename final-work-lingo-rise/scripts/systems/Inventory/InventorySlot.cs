using Godot;

public partial class InventorySlot : PanelContainer
{
    private TextureRect _itemIcon;

    public override void _Ready()
    {
        _itemIcon = GetNode<TextureRect>("ItemIcon");
    }

    public void SetItem(InventoryItem item)
    {
        if (item == null)
        {
            _itemIcon.Texture = null;
            return;
        }

        _itemIcon.Texture = item.Icon;
    }
}