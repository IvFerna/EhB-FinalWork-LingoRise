using Godot;
using System.Collections.Generic;
using System.Linq;
using System;

public partial class InventoryComponent : Node
{
    [Export] public int MaxSlots = 3;

    public event Action InventoryChanged;
    private List<InventoryItem> _items = new();

    public bool AddItem(InventoryItem item)
    {
        if (_items.Count >= MaxSlots)
        {
            GD.Print("Inventory full");
            return false;
        }

        _items.Add(item);
        InventoryChanged?.Invoke();
        GD.Print($"Added {item.ItemName}");

        return true;
    }

    public bool HasItem(string itemId)
    {
        return _items.Any(item => item.ItemId == itemId);
    }

    public List<InventoryItem> GetItems()
    {
        return _items;
    }
    public bool RemoveItem(string itemId)
    {
        InventoryItem itemToRemove = _items.Find(item => item.ItemId == itemId);

        if (itemToRemove == null)
        {
            GD.Print($"Item not found: {itemId}");
            return false;
        }

        _items.Remove(itemToRemove);

        GD.Print($"Removed {itemToRemove.ItemName}");

        InventoryChanged?.Invoke();

        return true;
    }

}