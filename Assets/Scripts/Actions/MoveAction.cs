using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveAction : BaseAction
{
    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;
    private Vector3 targetPosition;
    private bool isMoving = false; // To keep track of movement statd
    [SerializeField]float rotateSpeed = 10f;
    [SerializeField] private int maxMoveDistance = 1;
  

   protected override void Awake() 
   {
    base.Awake();
    targetPosition = transform.position;
   }



    private void Update() {

        if(!isActive){
            return;
        }

        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        
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
   
            isMoving = false; // Stop moving when close enough
            OnStopMoving?.Invoke(this, EventArgs.Empty);
            ActionComplete();

        }

        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime*rotateSpeed);
    }
 
  public override void TakeAction(GridPosition gridPosition, Action onActionComplete)  
    {
        
        this.targetPosition = LevelGrid.Instance.GetWorldPositionn(gridPosition);
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
        int targetCountAtGridPosition = unit.GetRangedAction().GetTargetCountAtPosition(gridPosition);
        return new EnemyAIAction{
            gridPosition = gridPosition,
            actionValue = targetCountAtGridPosition * 10,
        };
    }

}
