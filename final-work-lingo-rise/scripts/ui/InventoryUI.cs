using Godot;
using System.Collections.Generic;

public partial class InventoryUI : CanvasLayer
{
    private List<InventorySlot> _slots = new();

    public override void _Ready()
    {
        _slots.Add(GetNode<InventorySlot>("PanelContainer/HBoxContainer/InventorySlot1"));
        _slots.Add(GetNode<InventorySlot>("PanelContainer/HBoxContainer/InventorySlot2"));
        _slots.Add(GetNode<InventorySlot>("PanelContainer/HBoxContainer/InventorySlot3"));
    }

    public void UpdateInventory(List<InventoryItem> items)
    {
        for (int i = 0; i < _slots.Count; i++)
        {
            if (i < items.Count)
            {
                _slots[i].SetItem(items[i]);
            }
            else
            {
                _slots[i].SetItem(null);
            }
        }
    }
}