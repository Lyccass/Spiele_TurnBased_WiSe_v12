using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Unit : MonoBehaviour
{

    public static event EventHandler OnAnyActionPointChange;

    private GridPosition gridPosition;
    private MoveAction moveAction;
    private SpinAction spinAction;
    [SerializeField] public int MaxActionPoints = 2;
    private BaseAction[] baseActionArray;
    private int actionPoints;

    private void Awake()
    {
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        baseActionArray = GetComponents<BaseAction>();
        actionPoints = MaxActionPoints;
    }


    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
        TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChange;
    }
    private void Update() 
    {
        //own struct has the comparsion functions in it otherwise this would not work
         GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if(newGridPosition!= gridPosition)
        {
            //Unit Changed Gridposition
            LevelGrid.Instance.UnitMovedGridPosition(this, gridPosition, newGridPosition);
            gridPosition = newGridPosition;
        }
    }

    public MoveAction GetMoveAction()
    {
        return moveAction;
    }

     public SpinAction GetSpinAction()
    {
        return spinAction;
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
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
        actionPoints = MaxActionPoints;
        OnAnyActionPointChange?.Invoke(this, EventArgs.Empty);
   }
}
