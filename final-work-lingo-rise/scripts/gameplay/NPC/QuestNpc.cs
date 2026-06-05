using System.Collections.Generic;
using Godot;

public partial class QuestNpc : NpcController
{
    private RequestSystem _requestSystem;

    public override void _Ready()
    {
        base._Ready();

        _requestSystem = GetNode<RequestSystem>("/root/RequestSystem");
    }

    public async void OnBodyEntered(Node body)
    {
        if (body is not PlayerController player)
            return;

        if (_requestSystem.CurrentRequestedItem == null)
        {
            _requestSystem.StartRequest(
                NpcData.DesiredItem
            );

            await _dialogueManager.ShowDialogueWithExposure(
                _bubbleAnchor,
                NpcData.RequestDialogue,
                DialoguePriority.Gameplay
            );

            return;
        }

        InventoryItem heldItem = player.GetHeldItem();

        if (heldItem == null)
            return;

        if (_requestSystem.ValidateItem(heldItem))
        {
            player.RemoveFromInventory(heldItem.Id);

            await _dialogueManager.ShowDialogueWithExposure(
                _bubbleAnchor,
                NpcData.SuccessDialogue,
                DialoguePriority.Gameplay
            );

            _requestSystem.CompleteRequest();
        }
        else
        {
            await _dialogueManager.ShowDialogueWithExposure(
                _bubbleAnchor,
                NpcData.WrongItemDialogue,
                DialoguePriority.Gameplay,
                new Dictionary<string, string> { { "item", heldItem.VocabularyEntry.ForeignWord } });
        }
    }

    public async void OnQuestCompleted()
    {
        await _dialogueManager.ShowDialogueWithExposure(
            _bubbleAnchor,
            NpcData.CompletionDialogue,
            DialoguePriority.Gameplay
        );
    }
}