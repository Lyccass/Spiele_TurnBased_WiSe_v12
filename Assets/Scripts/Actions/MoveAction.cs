using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveAction : BaseAction
{
    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;
    private List<Vector3> positionList;
    private int currentPositionIndex;
    private bool isMoving = false; // To keep track of movement statd
    [SerializeField]float rotateSpeed = 10f;
    [SerializeField] private int maxMoveDistance = 1;
  
    private void Update() {

        if(!isActive)
        {
            return;
        }

        Vector3 targetPosition = positionList[currentPositionIndex];
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime*rotateSpeed);
        float stoppingDistance = 0.1f;
        if (isMoving && Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            // Move toward target position
            
            float moveSpeed = 4f;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
            //smoothing the rotation
        
           
        }
        else
        {
   
            currentPositionIndex++;
            if(currentPositionIndex >= positionList.Count)
            {

                 isMoving = false; // Stop moving when close enough
                 OnStopMoving?.Invoke(this, EventArgs.Empty);
                 ActionComplete();

            }
           

        }

    
    }
 
  public override void TakeAction(GridPosition gridPosition, Action onActionComplete)  
    {
        List<GridPosition> pathGridPositionList = Pathfinding.Instance.FindPath(unit.GetGridPosition(), gridPosition, out int pathLength);

        currentPositionIndex = 0;
        positionList = new List<Vector3>();

        foreach(GridPosition pathGridPosition in pathGridPositionList)
        {
            positionList.Add(LevelGrid.Instance.GetWorldPositionn(pathGridPosition));
        }

        isMoving = true;  // Start moving
        OnStartMoving?.Invoke(this, EventArgs.Empty);
        ActionStart(onActionComplete);
    }


public override List<GridPosition> GetValidGridPositionList()
{
    List<GridPosition> validGridPositionList = new List<GridPosition>();

GridPosition unitGridPosition = unit.GetGridPosition();

    for (int x= -maxMoveDistance; x <= maxMoveDistance; x++){
        for (int z= -maxMoveDistance; z <= maxMoveDistance; z++)
        {
            GridPosition offsetGridPosition = new GridPosition(x,z);
            GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
            if(!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
            {
                continue;
            }

            //Same GridPosition
            if(testGridPosition == unitGridPosition)
            {
                continue;
            }

            //GridPosition is already occupied with a UNIT
            if(LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
            {
                continue;
            }

            if(!Pathfinding.Instance.IsWalkableGridPosition(testGridPosition))
            {
                continue;
            }

            if(!Pathfinding.Instance.HasPath(unitGridPosition, testGridPosition))
            {
                continue;
            } 

            int pathfindingDistanceMultpllier = 10;
           if( Pathfinding.Instance.GetPathLength(unitGridPosition,testGridPosition)> maxMoveDistance*pathfindingDistanceMultpllier)
           {
            //path is too
                continue;
           }



            validGridPositionList.Add(testGridPosition);

        }
    }
    return validGridPositionList;
}

    public override string GetActionName()
    {
       return "Move";
    }

    
    public override EnemyAIAction GetBestEnemyAIAction(GridPosition gridPosition)
    {
        int targetCountAtGridPosition = unit.GetAction<RangedAction>().GetTargetCountAtPosition(gridPosition);
        return new EnemyAIAction{
            gridPosition = gridPosition,
            actionValue = targetCountAtGridPosition * 10,
        };
    }

}
