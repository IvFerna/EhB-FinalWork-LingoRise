using Godot;
using System;

public partial class RequestSystem : Node
{
    public InventoryItem CurrentRequestedItem { get; private set; }
    public RequestState CurrentState { get; private set; }

    [Signal]
    public delegate void RequestStartedEventHandler(InventoryItem item);

    [Signal]
    public delegate void RequestCompletedEventHandler();

    [Signal]
    public delegate void RequestStateChangedEventHandler(
        RequestState state,
        InventoryItem item
    );

    public enum RequestState
    {
        None,
        TalkToNpc,
        FindItem,
        ReturnToNpc,
        Completed
    }

    public override void _Ready()
    {
        CurrentState = RequestState.TalkToNpc;
    }

    public void StartRequest(InventoryItem item)
    {
        CurrentRequestedItem = item;
        CurrentState = RequestState.FindItem;

        EmitSignal(
            SignalName.RequestStateChanged,
            (int)CurrentState,
            item
        );
    }

    public void MarkItemCollected()
    {
        CurrentState = RequestState.ReturnToNpc;

        EmitSignal(
            SignalName.RequestStateChanged,
            (int)CurrentState,
            CurrentRequestedItem
        );
    }

    public bool ValidateItem(InventoryItem item)
    {
        if (CurrentRequestedItem == null || item == null)
            return false;

        return CurrentRequestedItem.ItemName == item.ItemName;
    }

    public void CompleteRequest()
    {
        CurrentRequestedItem = null;
        CurrentState = RequestState.Completed;
        EmitSignal(SignalName.RequestCompleted);
    }
}