using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelGrid : MonoBehaviour
{
    [SerializeField]private Transform gridDebugObjectPrefab;
     public static LevelGrid Instance{get; private set;}
    public event EventHandler<OnAnyUnitMovedGridPositionEventArgs> OnAnyUnitMovedGridPosition;
    public class OnAnyUnitMovedGridPositionEventArgs : EventArgs
    {
        public Unit unit;
        public GridPosition fromGridPosition;
        public GridPosition toGridPosition;
    }

    private GridSystem<GridObject> gridSystem;
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private int cellSize;
    

    private void Awake() 
    {
        if(Instance != null){
        Debug.LogError("Error, more than one UnitActionSystem");
        Destroy(gameObject);
        return;
    }
     Instance = this;
        gridSystem = new GridSystem<GridObject>(width,height,cellSize, 
                (GridSystem<GridObject> g, GridPosition gridPosition) => new GridObject(g, gridPosition) );
    }
     
       

    private void Start()
    {
        Pathfinding.Instance.Setup(width, height, cellSize);
    }

    public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
       GridObject gridObject = gridSystem.GetGridObject(gridPosition);
       gridObject.AddUnit(unit);

    }

  public List<Unit> GetUnitListAtGridPosition(GridPosition gridPosition)
{
    GridObject gridObject = gridSystem.GetGridObject(gridPosition);
    return gridObject.GetUnitList();
}


    public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.RemoveUnit(unit);
    }

   public void UnitMovedGridPosition(Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition)
{
    if (unit.IsEnemy()) // Ensure this works only with friendly units
    {
      //  Debug.Log("Unit is not friendly, ignoring movement.");
        RemoveUnitAtGridPosition(fromGridPosition, unit);
        AddUnitAtGridPosition(toGridPosition, unit);
        return;
    }

    RemoveUnitAtGridPosition(fromGridPosition, unit);
    AddUnitAtGridPosition(toGridPosition, unit);

    OnAnyUnitMovedGridPosition?.Invoke(this, new OnAnyUnitMovedGridPositionEventArgs
    {
        unit = unit,
        fromGridPosition = fromGridPosition,
        toGridPosition = toGridPosition,
    });
}




    public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);
    public Vector3 GetWorldPositionn(GridPosition gridPosition) => gridSystem.GetWorldPosition(gridPosition);
    public bool IsValidGridPosition(GridPosition gridPosition) => gridSystem.IsValidGridPosition(gridPosition);
    public int GetWidth() => gridSystem.GetWidth();
    public int GetHeight() => gridSystem.GetHeight();


    public bool HasAnyUnitOnGridPosition(GridPosition gridPosition)
    {
         GridObject gridObject = gridSystem.GetGridObject(gridPosition);
         return gridObject.HasAnyUnit();
    }

      public Unit GetUnitAtGridPosition(GridPosition gridPosition)
    {
         GridObject gridObject = gridSystem.GetGridObject(gridPosition);
         return gridObject.GetUnit();
    }

public int GetGridWidth()
{
    return width;
}

public int GetGridHeight()
{
    return height;
}

public IInteractable GetInteractableAtGridPosition(GridPosition gridPosition)
{
    GridObject gridObject = gridSystem.GetGridObject(gridPosition);
    return gridObject.GetInteractable();
}
    
public void SetInteractableAtGridPosition(GridPosition gridPosition, IInteractable interactable)
{
    GridObject gridObject = gridSystem.GetGridObject(gridPosition);
    gridObject.SetInteractable(interactable);
   
}

 public void ClearInteractableAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.ClearInteractable();
    }

}
