using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DestructableCrate : MonoBehaviour
{
    public static event EventHandler OnAnyDestroyed;

    private GridPosition gridPosition;

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
    }
    
    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }
    public void Damage()
    {
        Destroy(gameObject);
        OnAnyDestroyed?.Invoke(this, EventArgs.Empty);
    }
}
