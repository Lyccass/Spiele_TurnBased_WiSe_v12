using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScripting : MonoBehaviour
{
    [SerializeField] private List<GameObject> hider1List;
    [SerializeField] private List<GameObject> hider2List;
    [SerializeField] private List<GameObject> hider3List;
    [SerializeField] private List<GameObject> hider4List;
    [SerializeField] private List<GameObject> hider5List;

    
    [SerializeField] private List<GameObject> Allay1List;

    [SerializeField] private List<GameObject> enemy1List;
    [SerializeField] private List<GameObject> enemy2List;
    [SerializeField] private List<GameObject> enemy3List;
    [SerializeField] private List<GameObject> enemy4List;

    private Dictionary<Vector2Int, Action> roomTriggers;
    private HashSet<Vector2Int> triggeredPositions = new HashSet<Vector2Int>();

    private void Start()
    {
            Debug.Log("Subscribing to OnAnyUnitMovedGridPosition");
    if (LevelGrid.Instance != null)
    {
        LevelGrid.Instance.OnAnyUnitMovedGridPosition += LevelGrid_OnAnyUnitMovedGridPosition;
    }
    else
    {
        Debug.LogError("LevelGrid.Instance is null!");
    }

        // Initialize room triggers
        roomTriggers = new Dictionary<Vector2Int, Action>
        {
            {
                new Vector2Int(10, 3), // Example grid position for Room 1
                () =>
                {
                    DestroyGameObjectList(hider1List); // Destroy hiders
                    SetActiveGameObjectList(enemy1List, true);
                }
            },
            {
                new Vector2Int(3, 12), // Example grid position for Room 2
                () =>
                {
                    DestroyGameObjectList(hider2List); // Destroy hiders
                    SetActiveGameObjectList(enemy2List, true);
                }
            },
            {
                new Vector2Int(20, 12), // Example grid position for Room 3
                () =>
                {
                    DestroyGameObjectList(hider3List); // Destroy hiders
                    SetActiveGameObjectList(enemy3List, true);
                }
            },
            {
                new Vector2Int(10, 11), // Example grid position for Room 4
                () =>
                {
                    DestroyGameObjectList(hider4List); // Destroy hiders
                    SetActiveGameObjectList(enemy4List, true);
                }
            },

             {
                new Vector2Int(21, 20), // Example grid position for Room 4
                () =>
                {
                    DestroyGameObjectList(hider5List); // Destroy hiders
                    SetActiveGameObjectList(Allay1List, true);
                }
            }
        };
    }

  private void LevelGrid_OnAnyUnitMovedGridPosition(object sender, LevelGrid.OnAnyUnitMovedGridPositionEventArgs e)
{
    Vector2Int currentGridPosition = new Vector2Int(e.toGridPosition.x, e.toGridPosition.z);
    Debug.Log($"Unit moved to position: {currentGridPosition}");

    if (roomTriggers.ContainsKey(currentGridPosition) && !triggeredPositions.Contains(currentGridPosition))
    {
        Debug.Log($"Triggering actions for position: {currentGridPosition}");
        triggeredPositions.Add(currentGridPosition);
        roomTriggers[currentGridPosition].Invoke();
    }
    else
    {
        Debug.Log($"No action for position: {currentGridPosition} or already triggered.");
    }
}


    private void SetActiveGameObjectList(List<GameObject> gameObjectList, bool isActive)
{
    Debug.Log($"Setting GameObjects in list to active: {isActive}");
    foreach (GameObject gameObject in gameObjectList)
    {
        if (gameObject != null)
        {
            gameObject.SetActive(isActive);
        }
        else
        {
            Debug.LogWarning("GameObject in list is null!");
        }
    }
}

private void DestroyGameObjectList(List<GameObject> gameObjectList)
{
    Debug.Log("Destroying GameObjects in list.");
    foreach (GameObject gameObject in gameObjectList)
    {
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Debug.LogWarning("GameObject in list is null!");
        }
    }
}


}
