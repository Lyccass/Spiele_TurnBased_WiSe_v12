using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class Unit : MonoBehaviour
{

    public static event EventHandler OnAnyActionPointChange;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;
    [SerializeField] private bool isEnemy;  
    private HealthSystem healthSystem;
    private GridPosition gridPosition;
    [SerializeField] public int MaxActionPoints = 2;
    private BaseAction[] baseActionArray;
    private int actionPoints;
    

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        baseActionArray = GetComponents<BaseAction>();
        actionPoints = MaxActionPoints;
    }


    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
        TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChange;
        healthSystem.OnDead += HealthSystem_OnDead;

        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
    }
    private void Update() 
    {
        //own struct has the comparsion functions in it otherwise this would not work
         GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if(newGridPosition!= gridPosition)
        {
            //Unit Changed Gridposition - order is important cause eventhandling
            GridPosition oldGridPosition = gridPosition;
            gridPosition = newGridPosition;
            LevelGrid.Instance.UnitMovedGridPosition(this, oldGridPosition, newGridPosition);
        }
    }
 
    public T GetAction <T>() where T : BaseAction
    {
        foreach (BaseAction baseAction in baseActionArray)
        {
            if(baseAction is T)
            {
                return (T)baseAction;
            }

        }
        return null;
    }
    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public UnityEngine.Vector3 GetWordPosition()
    {
        return transform.position;
    }

    public BaseAction[] GetBaseActionArray()
    {
        return baseActionArray;
    }

public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
{
    if(CanSpendActionPointsToTakeAction(baseAction))
    {
        SpendActionPoints(baseAction.GetActionPointsCost());
        return true;
    }
    else
    {
        return false;
    }
}
    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if(actionPoints >= baseAction.GetActionPointsCost())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void SpendActionPoints (int amount)
    {
        actionPoints -= amount;
        OnAnyActionPointChange?.Invoke(this, EventArgs.Empty);
    }
   
    public int GetActionPoints()
   {
      return actionPoints;
   }
   
   private void TurnSystem_OnTurnChange(object sender, EventArgs e)
   {
     if ((IsEnemy() && TurnSystem.Instance.IsPlayerTurn()) || 
             (!IsEnemy() && TurnSystem.Instance.IsPlayerTurn()))
{
    actionPoints = MaxActionPoints;
    OnAnyActionPointChange?.Invoke(this, EventArgs.Empty);
}

   }

   public bool IsEnemy()
   {
    return isEnemy;
   }

   public void Damage(int damageAmount)
   {
        healthSystem.Damage(damageAmount);
   }

   private void HealthSystem_OnDead(object sender , EventArgs e)
   {
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);
        Destroy(gameObject);
        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
   }

   public float GetHealthNomalized()
   {
    return healthSystem.GetHealthNormalized();
   }

   
}
