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
            Debug.LogError("No linked door found!");
            onInteractComplete?.Invoke();
            return;
        }

        this.onInteractComplete = onInteractComplete;

        // Toggle door state
        if (linkedDoor.IsOpen) // Use a public property or method in the Door class to check if the door is open
        {
            linkedDoor.CloseDoor();
            Debug.Log($"{gameObject.name} interacted with. Door is now closed.");
        }
        else
        {
            linkedDoor.OpenDoor();
            Debug.Log($"{gameObject.name} interacted with. Door is now open.");
        }

        onInteractComplete?.Invoke();
    }
}
