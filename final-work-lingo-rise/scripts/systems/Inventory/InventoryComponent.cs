using Godot;
using System.Collections.Generic;
using System.Linq;
using System;

public partial class InventoryComponent : Node
{
    [Export] public int MaxSlots = 1;

    public event Action InventoryChanged;

    private List<InventoryItem> _items = new();

    private Item _heldWorldItem;

    public bool AddItem(Item worldItem)
    {
        if (worldItem == null || worldItem.ItemData == null)
            return false;

        // Replace currently held item
        if (_items.Count >= MaxSlots)
        {
            if (_heldWorldItem != null)
            {
                _heldWorldItem.ReturnToWorld();
            }

            _items.Clear();
        }

        _items.Add(worldItem.ItemData);
        _heldWorldItem = worldItem;

        InventoryChanged?.Invoke();

        GD.Print($"Added {worldItem.ItemData.ItemName}");

        return true;
    }

    public bool HasItem(string itemId)
    {
        return _items.Any(item => item.Id == itemId);
    }

    public List<InventoryItem> GetItems()
    {
        return _items;
    }

    public bool RemoveItem(string itemId)
    {
        InventoryItem itemToRemove =
            _items.Find(item => item.Id == itemId);

        if (itemToRemove == null)
        {
            GD.Print($"Item not found: {itemId}");
            return false;
        }

        _items.Remove(itemToRemove);

        _heldWorldItem = null;

        GD.Print($"Removed {itemToRemove.ItemName}");

        InventoryChanged?.Invoke();

        return true;
    }

    public InventoryItem GetSelectedItem()
    {
        if (_items.Count == 0)
            return null;

        return _items[0];
    }
}