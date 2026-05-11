using Godot;
using System;

public partial class QuestUI : Control
{
    private Label _mainLabel;
    private Label _subLabel;
    public override void _Ready()
    {
        _mainLabel = GetNode<Label>("Panel/MainQuestLabel");
        _subLabel = GetNode<Label>("Panel/VBoxContainer/SubQuestLabel");

        var requestSystem =
            GetNode<RequestSystem>("/root/Main/Systems/RequestSystem");

        requestSystem.RequestStateChanged += UpdateSubQuest;
        requestSystem.RequestCompleted += ClearSubQuest;

        _mainLabel.Text = "Buy breakfast";
        _subLabel.Text = "Talk to the baker";
    }

    private void UpdateSubQuest(RequestSystem.RequestState state, InventoryItem item)
    {
        switch (state)
        {
            case RequestSystem.RequestState.FindItem:
                _subLabel.Text =
                    $"Find {item.ForeignWord}";
                break;

            case RequestSystem.RequestState.ReturnToBaker:
                _subLabel.Text =
                    "Return to the baker";
                break;
        }
    }

    private void ClearSubQuest()
    {
        _subLabel.Text = "Purchase complete";
    }
}
