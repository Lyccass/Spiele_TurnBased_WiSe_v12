using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridActionUnlocker : MonoBehaviour
{
    [SerializeField] private string mapName = "FirstMap"; // The map this applies to
    [SerializeField] private List<GridUnlockData> unlockTriggers; // List of unlock conditions

    private HashSet<Vector2Int> triggeredPositions = new HashSet<Vector2Int>(); // Prevents re-triggering

    private void Start()
    {
        if (LevelGrid.Instance != null)
        {
            LevelGrid.Instance.OnAnyUnitMovedGridPosition += LevelGrid_OnAnyUnitMovedGridPosition;
        }
        else
        {
            Debug.LogError("LevelGrid.Instance is null!");
        }
    }

    private void OnDestroy()
    {
        if (LevelGrid.Instance != null)
        {
            LevelGrid.Instance.OnAnyUnitMovedGridPosition -= LevelGrid_OnAnyUnitMovedGridPosition;
        }
    }

    private void LevelGrid_OnAnyUnitMovedGridPosition(object sender, LevelGrid.OnAnyUnitMovedGridPositionEventArgs e)
    {
        Vector2Int currentGridPosition = new Vector2Int(e.toGridPosition.x, e.toGridPosition.z);

        foreach (GridUnlockData unlockData in unlockTriggers)
        {
            if (unlockData.unlockPosition == currentGridPosition && !triggeredPositions.Contains(currentGridPosition))
            {
                Debug.Log($"Unit stepped on unlock tile at {currentGridPosition}");

                triggeredPositions.Add(currentGridPosition);

                Unit unit = LevelGrid.Instance.GetUnitAtGridPosition(e.toGridPosition);
                if (unit != null)
                {
                    UnitActionUnlocker unlocker = unit.GetComponent<UnitActionUnlocker>();
                    if (unlocker != null)
                    {
                        unlocker.UnlockAction(unlockData.actionToUnlock); // Unlock specific action
                        Debug.Log($"Unlocked action {unlockData.actionToUnlock.GetActionName()} for {unit.gameObject.name}");
                    }
                }
            }
        }
    }
}
