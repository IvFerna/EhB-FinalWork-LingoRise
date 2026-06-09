using Godot;
using System;

public partial class MainHubScene : Control
{
    public static MainHubScene Instance;
    [Export] private Marker2D bakeryExitSpawn;
    [Export] private Marker2D coffeeShopExitSpawn;
    [Export] private Marker2D defaultSpawn;
    public override void _Ready()
    {
        var player = GetTree().CurrentScene?.GetNodeOrNull<PlayerController>("Player");

        if (player == null)
        {
            GD.PrintErr("MainHubScene: Player node not found in the scene tree.");
            return;
        }

        QuestUI.Instance.SetQuest("quest_explore_town");

        GD.Print("player found:", player);
        player.SetCameraEnabled(true);

        var pendingSpawn = Main.Instance?.PendingSpawn ?? SpawnPoint.None;
        switch (pendingSpawn)
        {
            case SpawnPoint.Default:
                player.Position = defaultSpawn.Position;
                break;

            case SpawnPoint.BakeryEntrance:
                player.Position = bakeryExitSpawn.Position;
                break;

            case SpawnPoint.CoffeeShopEntrance:
                player.Position = coffeeShopExitSpawn.Position;
                break;

            default:
                player.Position = defaultSpawn.Position;
                break;
        }

        DialogueManager.Instance.SetDialogueEnabled(false);
    }

    public void OnBakeryArea2DEnter(Node body)
    {
        if (body is PlayerController player)
        {
            Main.Instance.CallDeferred(nameof(Main.LoadWorld),
            "res://scenes/scenarios/BakeryScene.tscn"
            );
        }
    }
    public void OnCoffeeShopArea2DEnter(Node body)
    {
        if (body is PlayerController player)
        {
            Main.Instance.CallDeferred(nameof(Main.LoadWorld),
            "res://scenes/scenarios/CoffeeShopScene.tscn"
            );
        }
    }

}
