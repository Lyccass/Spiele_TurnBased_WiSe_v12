using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{

    private GridPosition gridPosition;
    private GridSystem gridSystem;

    public GridObject(GridSystem gridSystem, GridPosition gridPosition)
    {
        this.gridPosition = gridPosition;
        this.gridSystem = gridSystem;
    }

    public override string ToString()
    {
        return gridPosition.ToString();
    }


}
