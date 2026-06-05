using System.Collections.Generic;
using Godot;
public enum SpawnPoint
{
    None,
    BakeryEntrance,
    CoffeeShopEntrance,
    Default,
}
public partial class Main : Node
{
    public static Main Instance;

    [Export] public Node2D WorldContainer;
    [Export] private Label ScenarioCountLabel;

    private LexiconManager _wordSystem;
    private InventoryComponent _inventory;
    private DialogueManager _dialogueManager;
    public int CompletedScenarios { get; private set; }
    private HashSet<string> _completedScenarioIds =
    new();

    public SpawnPoint PendingSpawn { get; set; }

    public override void _Ready()
    {
        Instance = this;
        _wordSystem = GetNode<LexiconManager>(
         "/root/LexiconManager");

        _inventory = GetTree().CurrentScene?.GetNodeOrNull<InventoryComponent>("Player/InventoryComponent");
        if (_inventory == null)
        {
            _inventory = GetTree().Root.GetNodeOrNull<InventoryComponent>("/root/Main/Player/InventoryComponent");
        }
        if (_inventory == null)
        {
            var player = GetTree().CurrentScene?.GetNodeOrNull<PlayerController>("Player")
                ?? GetTree().Root.GetNodeOrNull<PlayerController>("/root/Main/Player");
            _inventory = player?.GetNodeOrNull<InventoryComponent>("InventoryComponent");
        }
        if (_inventory == null)
        {
            GD.PrintErr("Main: InventoryComponent not found in the scene tree.");
        }

        _dialogueManager = GetNode<DialogueManager>(
         "/root/DialogueManager"
        );

    }

    public void LoadWorld(string path)
    {
        GD.Print($"Loading world: {path}");
        foreach (Node child in WorldContainer.GetChildren())
        {
            child.QueueFree();
        }

        var scene = GD.Load<PackedScene>(path);
        var instance = scene.Instantiate();

        WorldContainer.AddChild(instance);
    }
    public bool CompleteScenario(string scenarioId)
    {
        if (_completedScenarioIds.Contains(scenarioId))
            return false;

        _completedScenarioIds.Add(scenarioId);

        CompletedScenarios++;

        return true;
    }

    public override void _Process(double delta)
    {
        ScenarioCountLabel.Text = $"{CompletedScenarios}/2";
    }

    public void ResetGame()
    {
        CompletedScenarios = 0;

        _completedScenarioIds.Clear();
        _wordSystem.ResetProgress();
        _inventory?.ClearInventory();
        _dialogueManager?.ClearDialogue();
        PendingSpawn = SpawnPoint.Default;

        CallDeferred(nameof(Main.LoadWorld),
        "res://scenes/scenarios/MainHubScene.tscn"
        );

        GD.Print("Game Reset");
    }
}