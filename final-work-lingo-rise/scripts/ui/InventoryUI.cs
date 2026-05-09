using Godot;
using System.Collections.Generic;

public partial class InventoryUI : CanvasLayer
{
    private List<PanelContainer> _slots = new();

    public override void _Ready()
    {
        _slots.Add(GetNode<PanelContainer>("PanelContainer/HBoxContainer/InventorySlot1"));
        _slots.Add(GetNode<PanelContainer>("PanelContainer/HBoxContainer/InventorySlot2"));
        _slots.Add(GetNode<PanelContainer>("PanelContainer/HBoxContainer/InventorySlot3"));
    }

    public void UpdateInventory(List<InventoryItem> items)
    {
        for (int i = 0; i < _slots.Count; i++)
        {
            if (i < items.Count)
            {
                _slots[i].Call("SetItem", items[i]);
            }
            else
            {
                _slots[i].Call("SetItem", (InventoryItem)null);
            }
        }
    }
}