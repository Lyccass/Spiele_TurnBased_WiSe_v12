using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpinAction : BaseAction
{

    private float totalSpinAmount;


    private void Update() 
    {
        if(!isActive)
        {
            return;
        }
    
         float spinAmount = 360f * Time.deltaTime;
         transform.eulerAngles += new Vector3(0,spinAmount,0);

          totalSpinAmount += spinAmount;
         if(totalSpinAmount >= 360f)
         {
           ActionComplete();
         }
        
        
    }
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
       
        totalSpinAmount = 0f;
        ActionStart(onActionComplete);
    }

      public override string GetActionName()
    {
       return "Spin";
    }

    public override List<GridPosition> GetValidGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();

        return new List<GridPosition>
        {
            unitGridPosition
        };
    }

    public override int GetActionPointsCost()
    {
        return 1;
    }

   public override EnemyAIAction GetBestEnemyAIAction(GridPosition gridPosition)
{
    return new EnemyAIAction(gridPosition, 0f);
}

}