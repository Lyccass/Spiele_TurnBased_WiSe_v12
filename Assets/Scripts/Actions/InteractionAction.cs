using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionAction : BaseAction
{
    private int maxInteractDistance = 1;

    private void Update() 
    {
        if(!isActive)
        {
            return;
        }
    }
    public override string GetActionName()
    {
        return "Interact";
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

    for (int x= -maxInteractDistance; x <= maxInteractDistance; x++)
    {
        
        for (int z= -maxInteractDistance; z <= maxInteractDistance; z++)
        {
            GridPosition offsetGridPosition = new GridPosition(x,z);
            GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

             // Exclude the grid position the character is standing on
            if (testGridPosition == unitGridPosition)
            {
                continue;
            }

            if(!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
            {
                continue;
            }

           Door door = LevelGrid.Instance.GetDoorAtGridPosition(testGridPosition);
           if( door == null)

           {
            //No door here
            continue;
           }


            validGridPositionList.Add(testGridPosition);

        }
    }
    return validGridPositionList;
    }



    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        Debug.Log("Interacted");
        Door door = LevelGrid.Instance.GetDoorAtGridPosition(gridPosition);
        door.Interact(OnInteractComplete);
        ActionStart(onActionComplete);
    }


    private void OnInteractComplete()
    {
        ActionComplete();
    }
}
