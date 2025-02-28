using System;
using UnityEngine;

public class LinkedInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private Door linkedDoor; // Reference to the linked door
    private Action onInteractComplete;

    private void Start()
    {
       
    if (linkedDoor == null)
    {
        Debug.LogError("Linked door is not assigned!");
    }
    else
    {
        Debug.Log($"Linked door assigned: {linkedDoor.name}");
        linkedDoor.LinkedInteractable = this;
    }
    }
public void Interact(Action onInteractComplete)
{
    if (linkedDoor == null)
    {
        Debug.LogError($"No linked door found for {gameObject.name}!");
        onInteractComplete?.Invoke();
        return;
    }

    this.onInteractComplete = onInteractComplete;
    AudioManager.Instance.PlaySFX("Lever");

    // Toggle door state
    if (linkedDoor.IsOpen)
    {
        linkedDoor.CloseDoor();
        Debug.Log($"{gameObject.name} interacted with. Door is now closed.");
    }
    else
    {
        linkedDoor.OpenDoor();
        Debug.Log($"{gameObject.name} interacted with. Door is now open.");
    }

    //  Ensure the switch tile remains non-walkable
    GridPosition leverGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
    Debug.Log($"Ensuring LEVER tile remains NON-walkable at {leverGridPosition} for {gameObject.name}");
    Pathfinding.Instance.SetIsWalkableGridPosition(leverGridPosition, false);

    onInteractComplete?.Invoke();
}



}
