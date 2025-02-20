using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScripting : MonoBehaviour
{
    [SerializeField] private string currentMapName = "FirstMap"; // Default active map
    [SerializeField] private List<MapFogTrigger> mapFogTriggers;

    private Dictionary<string, List<MapFogTrigger>> mapRoomTriggers;
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

        // Organize fog reveal triggers by map
        mapRoomTriggers = new Dictionary<string, List<MapFogTrigger>>();

        foreach (MapFogTrigger trigger in mapFogTriggers)
        {
            if (!mapRoomTriggers.ContainsKey(trigger.mapName))
            {
                mapRoomTriggers[trigger.mapName] = new List<MapFogTrigger>();
            }

            mapRoomTriggers[trigger.mapName].Add(trigger);
        }
    }

    private void LevelGrid_OnAnyUnitMovedGridPosition(object sender, LevelGrid.OnAnyUnitMovedGridPositionEventArgs e)
    {
        Vector2Int currentGridPosition = new Vector2Int(e.toGridPosition.x, e.toGridPosition.z);
        Debug.Log($"Unit moved to position: {currentGridPosition} on map {currentMapName}");

        if (mapRoomTriggers.ContainsKey(currentMapName))
        {
            foreach (var trigger in mapRoomTriggers[currentMapName])
            {
                if (trigger.triggerPositions.Contains(currentGridPosition) && !triggeredPositions.Contains(currentGridPosition))
                {
                    Debug.Log($"Triggering fog reveal at: {currentGridPosition}");
                    triggeredPositions.Add(currentGridPosition);
                    DestroyGameObjectList(trigger.hidersToDestroy);
                    SetActiveGameObjectList(trigger.unitsToActivate, true);
                }
            }
        }
        else
        {
            Debug.Log($"No action for position: {currentGridPosition} or no triggers set for map {currentMapName}.");
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

    public void SetCurrentMap(string mapName)
    {
        currentMapName = mapName;
        Debug.Log($"Switched to map: {mapName}");
    }
}

[Serializable]
public class MapFogTrigger
{
    public string mapName; // Name of the map
    public List<Vector2Int> triggerPositions; // Multiple positions for multi-tile fog reveal
    public List<GameObject> hidersToDestroy; // Fog tiles to remove
    public List<GameObject> unitsToActivate; // Enemies or allies to enable
}
