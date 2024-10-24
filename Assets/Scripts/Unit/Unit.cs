using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    private GridPosition gridPosition;
    private MoveAction moveAction;
    private SpinAction spinAction;

    private void Awake()
    {
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
    }


    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
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

   
   
}
