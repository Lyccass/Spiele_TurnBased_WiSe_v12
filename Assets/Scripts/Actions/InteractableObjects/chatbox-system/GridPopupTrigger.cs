using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPopupTrigger : MonoBehaviour
{
    [SerializeField] private string mapName = "FirstMap"; // The map this applies to
    [SerializeField] private List<TutorialPopupData> tutorialTriggers; // List of tutorial triggers

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

        foreach (TutorialPopupData popupData in tutorialTriggers)
        {
            if (popupData.triggerPosition == currentGridPosition && !triggeredPositions.Contains(currentGridPosition))
            {
                Debug.Log($"Unit stepped on tutorial tile at {currentGridPosition}");

                triggeredPositions.Add(currentGridPosition);

                // Show the tutorial pop-up with multiple messages
                TutorialPopupUI.Instance.ShowPopup(popupData.messages);
            }
        }
    }
}
