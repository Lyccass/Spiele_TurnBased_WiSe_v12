using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeAction : BaseAction
{
    [SerializeField] private Transform fireballProjectilePrefab;
   
    [SerializeField] private int maxFireballDistance = 3;

    private void Update()
    {
        if (!isActive)
        {
                return;
        }

    }

    public override string GetActionName()
    {
        return "Fireball";
    }
       public override int GetActionPointsCost()
    {
        return 3;
    }


    public override EnemyAIAction GetBestEnemyAIAction(GridPosition gridPosition)
    {
       return new EnemyAIAction {
        gridPosition = gridPosition,
        actionValue = 0,
       };
    }

  public override List<GridPosition> GetValidGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

    for (int x= -maxFireballDistance; x <= maxFireballDistance; x++)
    {
        
        for (int z= -maxFireballDistance; z <= maxFireballDistance; z++)
        {
            GridPosition offsetGridPosition = new GridPosition(x,z);
            GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

            if(!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
            {
                continue;
            }
            //design choice to limit range more circluar
            int testDistance = Mathf.Abs(x)+ Mathf.Abs(z);
            if(testDistance > maxFireballDistance)
            {
                continue;
            }

            validGridPositionList.Add(testGridPosition);

        }
    }
    return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
{
    // Instantiate the fireball projectile at the unit's position
    Transform fireballProjectileTransform = Instantiate(fireballProjectilePrefab, unit.GetWordPosition(), Quaternion.identity);
    FireBallProjectile fireBallProjectile = fireballProjectileTransform.GetComponent<FireBallProjectile>();

    // Pass the active unit as an argument to the Setup method
    fireBallProjectile.Setup(gridPosition, OnFireballBehaviourComplete, unit);
    Debug.Log("Fireball");
    AudioManager.Instance.PlaySFX("Fireball");
    ActionStart(onActionComplete);
}


    private void OnFireballBehaviourComplete()
    {
        ActionComplete();
    }
}
