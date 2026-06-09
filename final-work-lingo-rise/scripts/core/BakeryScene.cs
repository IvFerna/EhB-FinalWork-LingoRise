using Godot;
using System;

public partial class BakeryScene : Control
{
    [Export] private Marker2D playerSpawnPoint;
    [Export] private QuestNpc baker;
    private bool _completed;
    public override void _Ready()
    {
        var player = GetTree().CurrentScene.GetNode<PlayerController>("Player");

        player.SetCameraEnabled(false);

        GetNode<Camera2D>("Camera2D").Enabled = true;

        player.Position = playerSpawnPoint.Position;
        QuestUI.Instance.SetQuest("quest_buy_breakfast", "quest_talk_baker");
        QuestUI.Instance._subLabel.Visible = true;

        DialogueManager.Instance.SetDialogueEnabled(true);


        var requestSystem =
            GetNode<RequestSystem>("/root/RequestSystem");

        requestSystem.RequestCompleted += OnRequestCompleted;

    }
    public void OnBakeryArea2DEnter(Node body)
    {
        if (body is PlayerController player)
        {
            Main.Instance.PendingSpawn = SpawnPoint.BakeryEntrance;

            Main.Instance.CallDeferred(nameof(Main.LoadWorld),
            "res://scenes/scenarios/MainHubScene.tscn"
            );
        }
    }

    private async void OnRequestCompleted()
    {
        if (_completed)
            return;

        _completed = true;

        QuestUI.Instance._mainLabel.Text = LanguageManager.Instance.GetText("quest_completed");
        QuestUI.Instance._subLabel.Visible = false;

        baker.OnQuestCompleted();
        Main.Instance.CompleteScenario("bakery");

    }


}
