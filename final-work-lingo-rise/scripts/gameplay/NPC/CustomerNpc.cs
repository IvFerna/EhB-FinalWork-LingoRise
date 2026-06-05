using Godot;

public partial class CustomerNpc : NpcController
{
    private bool _orderGiven;
    private bool _served;

    public bool IsServed => _served;

    public async void OnBodyEntered(Node body)
    {
        if (body is not PlayerController player)
            return;

        if (_served)
            return;

        if (!_orderGiven)
        {
            _orderGiven = true;

            await _dialogueManager.ShowDialogueWithExposure(
                _bubbleAnchor,
                NpcData.RequestDialogue,
                DialoguePriority.Gameplay
            );

            return;
        }

        InventoryItem heldItem = player.GetHeldItem();

        if (heldItem == null)
        {
            // Repeat order when player talks again
            await _dialogueManager.ShowDialogueWithExposure(
                _bubbleAnchor,
                NpcData.RequestDialogue,
                DialoguePriority.Gameplay
            );

            return;
        }

        if (heldItem.ItemName == NpcData.DesiredItem.ItemName)
        {
            _served = true;

            player.RemoveFromInventory(heldItem.Id);

            await _dialogueManager.ShowDialogueWithExposure(
                _bubbleAnchor,
                NpcData.SuccessDialogue,
                DialoguePriority.Gameplay
            );

            GetTree().CallGroup(
                "CoffeeShop",
                "OnCustomerServed"
            );
        }
        else
        {
            await _dialogueManager.ShowDialogueWithExposure(
                _bubbleAnchor,
                NpcData.WrongItemDialogue,
                DialoguePriority.Gameplay
            );
        }
    }
}