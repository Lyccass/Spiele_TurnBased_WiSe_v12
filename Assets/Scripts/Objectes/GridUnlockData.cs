using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GridUnlockData
{
    public Vector2Int unlockPosition; // The specific tile where the action unlocks
    public BaseAction actionToUnlock; // The action that gets unlocked
}
