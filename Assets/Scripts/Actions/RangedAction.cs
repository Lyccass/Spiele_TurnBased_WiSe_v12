using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RangedAction : BaseAction
{
    public event EventHandler<OnShootEventArgs> OnShoot;
    public class OnShootEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit shootingUnit;
    }

      private enum State
    {
        Aming,
        Shooting,
        Cooldown,
    }
    [SerializeField] private LayerMask obstacleLayerMask;

    [SerializeField] private int maxRangedDistance = 7;
    private float stateTimer;
    private State state;
    private Unit targetUnit;
    private bool canShootBullet;
    [SerializeField] private int minDamage = 3;
    [SerializeField] private int maxDamage = 6;



     private void Update() 
    {
        if(!isActive)
        {
            return;
        }
    
        stateTimer -= Time.deltaTime;
        switch(state)
        {
            case State.Aming:
                float rotateSpeed = 10f;
                UnityEngine.Vector3 aimDir = (targetUnit.GetWordPosition() - unit.GetWordPosition()).normalized;
                transform.forward = UnityEngine.Vector3.Lerp(transform.forward, aimDir, Time.deltaTime*rotateSpeed);
                break;
            case State.Shooting:
                if(canShootBullet)
                {
                    Shoot();
                    canShootBullet = false;
                }
                break;
            case State.Cooldown:
                break;
        }
        
           if(stateTimer <= 0f)
                {
                    NextState();
                }
    }

private void NextState()
{
    switch (state)
    {
        case State.Aming:
            state = State.Shooting;
            float shootingStateTime = 0.1f;
            stateTimer = shootingStateTime;
            break;
        case State.Shooting:
            state = State.Cooldown; // Fix: Transition to Cooldown
            float coolOffStateTime = 0.5f;
            stateTimer = coolOffStateTime;
            break;
        case State.Cooldown:
            ActionComplete();
            break;
    }
}


private void Shoot()
{
    OnShoot?.Invoke(this, new OnShootEventArgs
    {
        targetUnit = targetUnit,
        shootingUnit = unit
    });

    int randomDamage = UnityEngine.Random.Range(minDamage, maxDamage);
    targetUnit.Damage(randomDamage);
}

public override string GetActionName()
    {
        return "Ranged";
    }
public override List<GridPosition> GetValidGridPositionList()
{
    GridPosition unitGridPosition = unit.GetGridPosition();
    return GetValidGridPositionList(unitGridPosition);

}
public List<GridPosition> GetValidGridPositionList(GridPosition unitGridPosition)
    {
         List<GridPosition> validGridPositionList = new List<GridPosition>();



    for (int x= -maxRangedDistance; x <= maxRangedDistance; x++){
        for (int z= -maxRangedDistance; z <= maxRangedDistance; z++)
        {
            GridPosition offsetGridPosition = new GridPosition(x,z);
            GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
            if(!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
            {
                continue;
            }
            //design choice to limit range more circluar
            int testDistance = Mathf.Abs(x)+ Mathf.Abs(z);
            if(testDistance > maxRangedDistance)
            {
                continue;
            }

            //GridPosition is empty no unit
            if(!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
            {
                continue;
            }
            Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
            if(targetUnit.IsEnemy() == unit.IsEnemy())
            {
            //test if both units are on opposite sides
              continue; 
            }

            UnityEngine.Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPositionn(unitGridPosition);

            UnityEngine.Vector3 rangedDir = (targetUnit.GetWordPosition() - unitWorldPosition).normalized;
            float unitShoulderHeight =1.7f;
            if(
            Physics.Raycast(
                unitWorldPosition+UnityEngine.Vector3.up*unitShoulderHeight,
                rangedDir,
                UnityEngine.Vector3.Distance(unitWorldPosition, targetUnit.GetWordPosition()),
                obstacleLayerMask))
            {

                //blocked by obstacle
                continue;

            }

            validGridPositionList.Add(testGridPosition);

        }
    }
    return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
      

        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        state = State.Aming;
        float aimingStateTime = 1f;
        stateTimer = aimingStateTime;

        canShootBullet = true;

        ActionStart(onActionComplete);
    
    }

    public Unit GetTargetUnit()
    {
        return targetUnit;
    }

    public int GetMaxRangeDistance()
    {
        return maxRangedDistance;
    }

    
    public override EnemyAIAction GetBestEnemyAIAction(GridPosition gridPosition)
    {
        Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        return new EnemyAIAction{
            gridPosition = gridPosition,
            actionValue = 100 + Mathf.RoundToInt((1- targetUnit.GetHealthNomalized()) * 100f),
        };
    }

    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
       return GetValidGridPositionList(gridPosition).Count;

    }
}
