using System;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public static event EventHandler OnAnyDoorOpened; // Event for any door opening
    public event EventHandler OnDoorOpened; // Event for this specific door opening

    [SerializeField] private bool isOpen; // Tracks if the door starts open
    [SerializeField] private bool isChest; // chest interactable
    private GridPosition gridPosition; // Grid position of the door
    private Animator animator; // Animator component
    private Action onInteractComplete; // Callback when interaction completes
    private bool isActive; // Is the door currently interacting
    private float timer; // Timer for interaction duration

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        Debug.Log($"{gameObject.name} - Grid Position: {gridPosition}");

        LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);

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
            onInteractComplete?.Invoke(); // Safeguard to avoid null references
        }
    }

    public void Interact(Action onInteractComplete)
    {
        if (isActive) return; // Prevent overlapping interactions

        this.onInteractComplete = onInteractComplete;
        isActive = true;
        timer = 0.5f; // Interaction duration

        if (isChest && !isOpen)
        {
            OpenChest();
        }
        else if (!isChest)
        {
            if (isOpen)
            {
                CloseDoor();
            }
            else
            {
                OpenDoor();
            }
        }
        else 
        {
            return;
        }
    }

    private void OpenChest()
    {
        isOpen = true;
        animator.SetTrigger("Open"); //insert animation lmao
        GameManager.Instance.chest --;
        Debug.Log($"Chest opened! Remaining chests: {GameManager.Instance.chest}");
    }

    private void OpenDoor()
    {
        isOpen = true;
        animator.SetBool("IsOpen", isOpen);
        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, true);

        // Trigger events when the door is opened
        OnDoorOpened?.Invoke(this, EventArgs.Empty);
        OnAnyDoorOpened?.Invoke(this, EventArgs.Empty);

    }

    private void CloseDoor()
    {
        isOpen = false;
        animator.SetBool("IsOpen", isOpen);
        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, false);
    }
}
