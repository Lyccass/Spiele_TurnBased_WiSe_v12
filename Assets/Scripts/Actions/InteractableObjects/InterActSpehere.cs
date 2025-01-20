using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterActSpehere : MonoBehaviour, IInteractable
{
[SerializeField] private Material activeMaterial;
[SerializeField] private Material inactiveMaterial;
[SerializeField]private MeshRenderer meshrenderer;
private GridPosition gridPosition;
private Action onInteractComplete;
private float timer;
private bool isActive;
private void Start() 
{
    gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
    LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);
    SetColorActive();
}


private void Update() 
    {
        if(!isActive)
        {
            return;
        }
        timer -= Time.deltaTime;
        if(timer <= 0f)
        {
            isActive = false;
            onInteractComplete();
        }
    }

private void SetColorActive()
{
    isActive = true;
    meshrenderer.material = activeMaterial;
}

private void SetColorInactive()
{
    isActive = false;
    meshrenderer.material = inactiveMaterial;
}

public void Interact(Action onInteractComplete)

    {
        this.onInteractComplete = onInteractComplete;
        isActive = true;
        timer = .5f;
        if(isActive)
        {
            SetColorInactive();
        }

        else 
        {
            SetColorActive();
        }
    }
}

