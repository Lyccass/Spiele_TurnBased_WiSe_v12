using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    [SerializeField]private Transform gridDebugObjectPrefab;
     public static LevelGrid Instance{get; private set;}
    private GridSystem gridSystem;
    [SerializeField] private int width;
    [SerializeField] private int height;

    private void Awake() 
    {
        if(Instance != null){
        Debug.LogError("Error, more than one UnitActionSystem");
        Destroy(gameObject);
        return;
    }
     Instance = this;
        gridSystem = new GridSystem(width,height,2f);
        gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
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
         RemoveUnitAtGridPosition(fromGridPosition, unit);
         AddUnitAtGridPosition(toGridPosition, unit);
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


};
