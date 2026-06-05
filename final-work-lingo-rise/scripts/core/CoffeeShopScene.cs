using Godot;
using System;

public partial class CoffeeShopScene : Control
{
    [Export] private Marker2D playerSpawnPoint;

    [Export] private QuestNpc barista;
    private int _servedCustomers;
    private bool _completed;
    public override void _Ready()
    {
        AddToGroup("CoffeeShop");
        var player = GetTree().CurrentScene.GetNode<PlayerController>("Player");

        player.SetCameraEnabled(false);

        GetNode<Camera2D>("Camera2D").Enabled = true;

        player.Position = playerSpawnPoint.Position;
        QuestUI.Instance._mainLabel.Text = "Serve the customers";
        QuestUI.Instance._subLabel.Text = "Talk to a customer";

        DialogueManager.Instance.SetDialogueEnabled(true);
    }
    public void OnCoffeeShopArea2DEnter(Node body)
    {
        if (body is PlayerController player)
        {
            Main.Instance.PendingSpawn = SpawnPoint.CoffeeShopEntrance;

            Main.Instance.CallDeferred(nameof(Main.LoadWorld),
            "res://scenes/scenarios/MainHubScene.tscn"
            );

        }
    }
    public void OnCustomerServed()
    {
        _servedCustomers++;

        GD.Print($"Customers served: {_servedCustomers}/3");

        if (_servedCustomers >= 3)
        {
            CompleteScenario();
        }

    }
    private async void CompleteScenario()
    {
        if (_completed)
            return;

        _completed = true;

        barista.OnQuestCompleted();
        Main.Instance.CompleteScenario("coffee_shop");

        GD.Print("Coffee Shop scenario completed!");
        QuestUI.Instance._mainLabel.Text = "Completed!";
        QuestUI.Instance._subLabel.Text = "";
        QuestUI.Instance._subLabel.Visible = false;

        DialogueManager.Instance.SetDialogueEnabled(false);

    }
}
