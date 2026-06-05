using Godot;
using System;

public partial class QuestUI : Control
{
    public static QuestUI Instance;
    public Label _mainLabel;
    public Label _subLabel;
    public override void _Ready()
    {
        Instance = this;
        _mainLabel = GetNode<Label>("Panel/MainQuestLabel");
        _subLabel = GetNode<Label>("Panel/VBoxContainer/HBoxContainer/SubQuestLabel");

        var requestSystem =
            GetNode<RequestSystem>("/root/RequestSystem");

        requestSystem.RequestStateChanged += UpdateSubQuest;
        requestSystem.RequestCompleted += ClearSubQuest;

        _mainLabel.Text = "Explore the town";
        _subLabel.Text = "";
        _subLabel.Visible = false;
    }

    private void UpdateSubQuest(RequestSystem.RequestState state, InventoryItem item)
    {
        _subLabel.Visible = true;
        switch (state)
        {
            case RequestSystem.RequestState.FindItem:

                _subLabel.Text =
                    $"Find {item.VocabularyEntry.ForeignWord}";
                break;

            case RequestSystem.RequestState.ReturnToNpc:
                _subLabel.Text =
                    "Return to the npc";
                break;
            case RequestSystem.RequestState.Completed:
                // _subLabel.Text = null;
                _subLabel.Visible = false;
                break;


        }
    }

    private void ClearSubQuest()
    {
        _subLabel.Text = "Purchase complete";
    }
}
