using System;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public static event EventHandler OnAnyDoorOpened;
    public event EventHandler OnDoorOpened;

    [SerializeField] private bool isOpen;
    [SerializeField] private bool isChest;
    [SerializeField] private Door linkedDoor;
    [SerializeField] private LinkedInteractable linkedInteractable;
     public LinkedInteractable LinkedInteractable
    {
        get => linkedInteractable;
        set => linkedInteractable = value;
    }


    
    private GridPosition gridPosition;
    private Animator animator;
    private Action onInteractComplete;
    private bool isActive;
    private float timer;
    public bool IsOpen => isOpen;

    private void Awake()
    {
        animator = GetComponent<Animator>(); // Attempt to assign the Animator

        if (animator == null)
        {
            Debug.LogWarning($"{gameObject.name} does not have an Animator component.");
        }
    }

    private void Start()
{
    gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);

    // Register door as interactable only if no linked interactable
    if (linkedInteractable == null)
    {
        LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);
        Debug.Log($"{gameObject.name} registered as interactable.");
    }
    else
    {
        Debug.Log($"{gameObject.name} is controlled by {linkedInteractable.gameObject.name} and will not be directly interactable.");
    }

    if (isOpen)
    {
        OpenDoor();
      
    }
    else
    {
        CloseDoor();
        
    }
}


    private void Update()
    {
        if (!isActive) return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            isActive = false;
            onInteractComplete?.Invoke();
        }
    }
public void Interact(Action onInteractComplete)
{
    
    if (isActive) return;

    this.onInteractComplete = onInteractComplete;
    isActive = true;
    timer = 0.5f;

    if (isChest && !isOpen)
    {
        OpenChest();
    }
    else if (!isChest)
    {
        if (isOpen)
        {
            CloseDoor();
            AudioManager.Instance.PlaySFX("DoorClose");

            // Close linked door if it's open
            if (linkedDoor != null && linkedDoor.IsOpen)
            {
                linkedDoor.CloseDoor();
                Debug.Log($"Linked door {linkedDoor.name} closed!");
            }
        }
        else
        {
            OpenDoor();
            AudioManager.Instance.PlaySFX("DoorOpen");
            // Open linked door if it's closed
            if (linkedDoor != null && !linkedDoor.IsOpen)
            {
                linkedDoor.OpenDoor();
                Debug.Log($"Linked door {linkedDoor.name} opened!");
            }
        }
    }
}


    private void OpenChest()
    {
        isOpen = true;

        if (animator != null)
        {
            animator.SetTrigger("Open");
        }

        GameManager.Instance.chest--;
        Debug.Log($"Chest opened! Remaining chests: {GameManager.Instance.chest}");
    }

   public void OpenDoor()
{
    if (isOpen) return;

    isOpen = true;
    Debug.Log($"Door {gameObject.name} is now open. (isOpen: {isOpen})");

    if (animator != null)
    {
        animator.SetBool("IsOpen", isOpen);
    }

    Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, true);
    OnDoorOpened?.Invoke(this, EventArgs.Empty);
    OnAnyDoorOpened?.Invoke(this, EventArgs.Empty);
}

public void CloseDoor()
{
    if (!isOpen) return;

    isOpen = false;
    Debug.Log($"Door {gameObject.name} is now closed. (isOpen: {isOpen})");

    if (animator != null)
    {
        animator.SetBool("IsOpen", isOpen);
    }

    Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, false);
}
}
