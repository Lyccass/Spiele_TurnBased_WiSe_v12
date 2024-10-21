using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : MonoBehaviour
{
    private Vector3 targetPosition;
    private bool isMoving = false; // To keep track of movement state
    [SerializeField]private Animator unitAnimator;
    [SerializeField]float rotateSpeed = 10f;
    [SerializeField] private int maxMoveDistance = 1;
    private Unit unit;

   private void Awake() 
   {
    unit = GetComponent<Unit>();  // Assign the Unit component to the unit variable
    targetPosition = transform.position;
   }



    private void Update() {
        
        float stoppingDistance = 0.1f;
        if (isMoving && Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            // Move toward target position
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            float moveSpeed = 4f;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
            //smoothing the rotation
            
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime*rotateSpeed);
            unitAnimator.SetBool("isWalking",true);
        }
        else
        {
            unitAnimator.SetBool("isWalking",false);
            isMoving = false; // Stop moving when close enough
        }
    }
 
  public void Move(GridPosition gridPosition)  
    {
        this.targetPosition = LevelGrid.Instance.GetWorldPositionn(gridPosition);
        isMoving = true;  // Start moving
    }

public bool IsValidActionGridPosition(GridPosition gridPosition)
{
 List<GridPosition> validGridPositionList = GetValidGridPositionList();
 return validGridPositionList.Contains(gridPosition);
}

public List<GridPosition> GetValidGridPositionList()
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

}
