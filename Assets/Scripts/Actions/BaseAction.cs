using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class BaseAction : MonoBehaviour
{
    public static event EventHandler OnAnyActionStarted;
    public static event EventHandler OnAnyActionComplete;

    protected bool isActive;
    protected Unit unit;
    protected Action onActionComplete;
    protected float _priorityMultiplier = 1.0f;  

    private bool isUnlocked = false; // NEW: Controls whether action is usable

    protected virtual void Awake() 
    {
        unit = GetComponent<Unit>();
    }

    public abstract string GetActionName();
    public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);

    public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        if (!isUnlocked) return false; // Prevents locked actions from appearing
        List<GridPosition> validGridPositionList = GetValidGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }

    public abstract List<GridPosition> GetValidGridPositionList();

    public virtual int GetActionPointsCost()
    {
        return 1;
    }

    protected void ActionStart(Action onActionComplete)
    {
        if (!isUnlocked) return; // Prevent locked actions from being executed
        isActive = true;
        this.onActionComplete = onActionComplete;
        OnAnyActionStarted?.Invoke(this, EventArgs.Empty);
    }

    protected void ActionComplete()
    {
        isActive = false;
        onActionComplete();
        OnAnyActionComplete?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetUnit()
    {
        return unit;
    }

    public EnemyAIAction GetBestEnemyAIAction()
    {
        if (!isUnlocked) return null; // Prevent AI from using locked actions

        List<EnemyAIAction> enemyAIActionList = new List<EnemyAIAction>();
        List<GridPosition> validActionGridPositionList = GetValidGridPositionList();

        foreach (GridPosition gridPosition in validActionGridPositionList)
        {
            EnemyAIAction enemyAIAction = GetBestEnemyAIAction(gridPosition);
            enemyAIActionList.Add(enemyAIAction);
        }

        if (enemyAIActionList.Count > 0)
        {
            enemyAIActionList.Sort((EnemyAIAction a, EnemyAIAction b) => b.actionValue.CompareTo(a.actionValue));
            return enemyAIActionList[0];
        }
        else
        {
            return null; // No possible AI actions
        }
    }

    public abstract EnemyAIAction GetBestEnemyAIAction(GridPosition gridPosition);

    // NEW: Unlocks this action so it can be used    

    public bool IsUnlocked()
    {
        return isUnlocked;
    }

    public void UnlockAction()
    {
        isUnlocked = true;
    }

    public void LockAction() // New: Allows actions to start as locked
    {
        isUnlocked = false;
    }

}
