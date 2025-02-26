using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionUnlocker : MonoBehaviour
{
    public static event Action OnUnlockNewAction; // UI update event

    [SerializeField] private List<BaseAction> preUnlockedActions; // Actions that start unlocked
    private HashSet<BaseAction> unlockedActions = new HashSet<BaseAction>();

    private void Start()
    {
        // Ensure pre-unlocked actions are added at the beginning
        foreach (BaseAction action in preUnlockedActions)
        {
            if (action != null)
            {
                action.UnlockAction();
                unlockedActions.Add(action);
            }
        }
    }

    public void UnlockAction(BaseAction actionToUnlock)
    {
        if (actionToUnlock == null || unlockedActions.Contains(actionToUnlock)) return;

        actionToUnlock.UnlockAction();
        unlockedActions.Add(actionToUnlock);

        Debug.Log($"Unlocked action: {actionToUnlock.GetActionName()} for {gameObject.name}");

        OnUnlockNewAction?.Invoke(); // Notify UI to refresh buttons
    }

    public bool IsActionUnlocked(BaseAction action)
    {
        return unlockedActions.Contains(action);
    }
}
