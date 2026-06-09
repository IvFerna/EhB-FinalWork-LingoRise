using Godot;
using System;

public partial class PlayerController : CharacterBody2D
{
    public static PlayerController Instance { get; private set; }
    [Export] private float moveSpeed = 200f;
    private bool _canMove = true;
    private Vector2 ScreenSize;
    private InventoryComponent _inventory;
    private InventoryUI _inventoryUI;

    // [Export] private Texture2D UpTexture;
    // [Export] private Texture2D DownTexture;
    // [Export] private Texture2D LeftTexture;
    // [Export] private Texture2D RightTexture;

    private AnimatedSprite2D _animatedSprite;
    private string _lastDirection = "down";
    private string _currentAnimation = "";

    public override void _Ready()
    {
        if (Instance != null && Instance != this)
        {
            QueueFree();
            return;
        }
        Instance = this;

        ScreenSize = GetViewportRect().Size;
        _inventory = GetNode<InventoryComponent>("InventoryComponent");
        _inventoryUI = GetTree().CurrentScene.GetNode<InventoryUI>("InventoryUI");
        _inventory.InventoryChanged += OnInventoryChanged;

        _animatedSprite = GetNode<AnimatedSprite2D>("Sprite2D");
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!_canMove)
        {
            GD.Print("You cannot move");
            return;
        }

        Vector2 input = new Vector2(
        Input.GetActionStrength("right") - Input.GetActionStrength("left"),
        Input.GetActionStrength("down") - Input.GetActionStrength("up")
        );

        // Normalize so diagonal isn't faster than cardinal
        Velocity = input.Normalized() * (input.Length() > 0 ? moveSpeed : 0);
        UpdateDirection(input);
        MoveAndSlide();
    }

    public bool AddToInventory(Item item)
    {
        return _inventory.AddItem(item);
    }

    public bool HasItem(string itemId)
    {
        return _inventory.HasItem(itemId);
    }
    private void OnInventoryChanged()
    {
        _inventoryUI.UpdateInventory(_inventory.GetItems());
    }
    public bool RemoveFromInventory(string itemId)
    {
        return _inventory.RemoveItem(itemId);
    }

    public InventoryItem GetHeldItem()
    {
        return _inventory.GetSelectedItem();
    }
    public void SetCameraEnabled(bool enabled)
    {
        GetNode<Camera2D>("Camera2D").Enabled = enabled;
    }

    private void UpdateDirection(Vector2 input)
    {
        if (input == Vector2.Zero)
        {
            PlayAnimation("default");
            return;
        }

        if (Mathf.Abs(input.X) > Mathf.Abs(input.Y))
        {
            if (input.X > 0)
            {
                PlayAnimation("Walk_right");
                _lastDirection = "right";
            }
            else
            {
                PlayAnimation("Walk_left");
                _lastDirection = "left";
            }
        }
        else
        {
            if (input.Y > 0)
            {
                PlayAnimation("Walk_down");
                _lastDirection = "down";
            }
            else
            {
                PlayAnimation("Walk_up");
                _lastDirection = "up";
            }
        }
    }

    private void PlayAnimation(string animationName)
    {
        if (_currentAnimation == animationName)
            return;

        _currentAnimation = animationName;
        _animatedSprite.Play(animationName);
    }

}

