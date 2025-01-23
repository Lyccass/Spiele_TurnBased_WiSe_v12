using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeeleAction : BaseAction
{
    public static event EventHandler<OnMeleeAttackEventArgs> OnAnyMeleeAttack;
    public event EventHandler<OnMeleeAttackEventArgs> OnMeleeAttack;

    public class OnMeleeAttackEventArgs : EventArgs
    {
    public Unit targetUnit;
    public Unit attackingUnit;
    }

    private enum State
    {
        MeeleBeforeHit,
        MeeleAfterHit,
    }

    private int maxMeeleDistance = 1;
    private State state;
    private float stateTimer;
    private Unit targetUnit;

    private void Update()
    {
        if(!isActive)
        {
            return;
        }


        stateTimer -= Time.deltaTime;
        switch(state)
        {
            case State.MeeleBeforeHit:
                 
                UnityEngine.Vector3 aimDir = (targetUnit.GetWordPosition() - unit.GetWordPosition()).normalized;
                float rotateSpeed = 10f;
                transform.forward = UnityEngine.Vector3.Lerp(transform.forward, aimDir, Time.deltaTime*rotateSpeed);
                break;
                
            case State.MeeleAfterHit:
               break;
    }
    if (stateTimer <= 0f)
    {
        NextState();
    }
    }

    private void NextState()
    {
        switch (state)
        {
            case State.MeeleBeforeHit:
                state = State.MeeleAfterHit;
                float AfterHitStateTime = 0.5f;
                stateTimer = AfterHitStateTime;
                Attack();
                break;
            case State.MeeleAfterHit:
                ActionComplete();
                break;
     
        }
    }

    private void Attack()
{
    // Trigger events for melee attack
    OnAnyMeleeAttack?.Invoke(this, new OnMeleeAttackEventArgs
    {
        targetUnit = targetUnit,
        attackingUnit = unit
    });

    OnMeleeAttack?.Invoke(this, new OnMeleeAttackEventArgs
    {
        targetUnit = targetUnit,
        attackingUnit = unit
    });

    // Deal damage to the target unit
    targetUnit.Damage(UnityEngine.Random.Range(7, 10));
}


    public override string GetActionName()
    {
        return"Meele";
    }

    public override EnemyAIAction GetBestEnemyAIAction(GridPosition gridPosition)
{
    return new EnemyAIAction(gridPosition, 200f);
}


    public override List<GridPosition> GetValidGridPositionList()
    {
       
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

    for (int x= -maxMeeleDistance; x <= maxMeeleDistance; x++)
    {
        
        for (int z= -maxMeeleDistance; z <= maxMeeleDistance; z++)
        {
            GridPosition offsetGridPosition = new GridPosition(x,z);
            GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

            if(!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
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

            validGridPositionList.Add(testGridPosition);

        }
    }
    return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
       targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

       state = State.MeeleBeforeHit;
       float beforeHitStateTime = 0.7f;
       stateTimer = beforeHitStateTime;
       ActionStart(onActionComplete);
    }

    public int GetMaxMeeleDistance()
    {
        return maxMeeleDistance;
    }

}
