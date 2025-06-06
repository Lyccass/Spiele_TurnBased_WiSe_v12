using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    
    public static GridSystemVisual Instance{get; private set;}

    [Serializable] public struct GridVisualTypeMaterial
    {
        public GridVisualType gridVisualType;
        public Material material;

    }
    public enum GridVisualType
    {
        White,
        Green,
        Red,
        LightRed,
        Yellow
    }

    [SerializeField] private Transform gridSystemVisualSinglePrefab;
    [SerializeField] private List<GridVisualTypeMaterial> gridVisualTypeMaterialList;
    private GridSystemVisualSingle[,] gridSystemVisualSingleArray;
    private void Awake()
   {
    if(Instance != null){
        Debug.LogError("Error, more than one UnitActionSystem");
        Destroy(gameObject);
        return;
    }
     Instance = this;
   }


    private void Start() 
    {
        gridSystemVisualSingleArray = new GridSystemVisualSingle[
            LevelGrid.Instance.GetWidth(),
             LevelGrid.Instance.GetHeight()
            ] ;

        for(int x = 0 ; x < LevelGrid.Instance.GetWidth(); x++ )
        {
             for(int z = 0 ; z < LevelGrid.Instance.GetHeight(); z++ )
            {
                GridPosition gridPosition = new GridPosition(x,z);
                Transform gridSystemVisualSingleTransform =
                 Instantiate(gridSystemVisualSinglePrefab, LevelGrid.Instance.GetWorldPositionn(gridPosition), Quaternion.identity);

                  gridSystemVisualSingleArray[x,z] = gridSystemVisualSingleTransform.GetComponent<GridSystemVisualSingle>();
            }
        }

        UnitActionSystem.Instance.OnSelectedActionChange += UnitActionSystem_OnSelectedActionChange;
        LevelGrid.Instance.OnAnyUnitMovedGridPosition += LevelGrid_OnAnyUnitMovedPosition;
        UpdateGridVisual();
    }

    public void HideAllGridPositions()
    {
        
        for(int x = 0 ; x < LevelGrid.Instance.GetWidth(); x++ )
        {
             for(int z = 0 ; z < LevelGrid.Instance.GetHeight(); z++ )
            {
                  gridSystemVisualSingleArray[x,z].Hide();
            }
        }

    }

    private void ShowGridPositionRange(GridPosition gridPosition, int range, GridVisualType gridVisualType)
    {
        List<GridPosition> gridPositionList = new List<GridPosition>();

        for (int x = -range; x<= range; x++)
        {
            for (int z = -range; z<= range; z++)
                 {
                GridPosition testGridPosition = gridPosition + new GridPosition(x , z);

                if(!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                    {
                        continue;
                    }
                    //design choice to limit range more circluar
                int testDistance = Mathf.Abs(x)+ Mathf.Abs(z);
                    if(testDistance > range)
                {
                     continue;
                }

                gridPositionList.Add(testGridPosition);

             }
        }
        ShowGridPositionList(gridPositionList, gridVisualType);
    }

      private void ShowGridPositionRangeSquare(GridPosition gridPosition, int range, GridVisualType gridVisualType)
    {
        List<GridPosition> gridPositionList = new List<GridPosition>();

        for (int x = -range; x<= range; x++)
        {
            for (int z = -range; z<= range; z++)
                 {
                GridPosition testGridPosition = gridPosition + new GridPosition(x , z);

                if(!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                    {
                        continue;
                    }

                gridPositionList.Add(testGridPosition);

             }
        }
        ShowGridPositionList(gridPositionList, gridVisualType);
    }

    public void ShowGridPositionList(List<GridPosition> gridPositionsList, GridVisualType gridVisualType)
    {
        foreach (GridPosition gridPosition in gridPositionsList)
        {
             gridSystemVisualSingleArray[gridPosition.x,gridPosition.z].
                Show(GetGridVisualTypeMaterial(gridVisualType));
        }

    }

    private void UpdateGridVisual()
    {
        HideAllGridPositions();
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        
        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();
        GridVisualType gridVisualType;
        switch (selectedAction)
        {
                default:
                case MoveAction moveAction:
                    gridVisualType = GridVisualType.White;
                    break;
                case SpinAction spinAction:
                    gridVisualType = GridVisualType.Yellow;
                    break;
                case RangedAction rangedAction:
                    gridVisualType = GridVisualType.Red;

                    ShowGridPositionRange(selectedUnit.GetGridPosition(), rangedAction.GetMaxRangeDistance(), GridVisualType.LightRed);
                    break;

                case AoeAction AoeAction:
                    gridVisualType = GridVisualType.Yellow;
                    break;

                case MeeleAction meeleAction:
                    gridVisualType = GridVisualType.Red;
                    break;

                case InteractionAction interactionAction:
                    gridVisualType = GridVisualType.White;
                    break;

                

        }
        ShowGridPositionList( 
                selectedAction.GetValidGridPositionList(), gridVisualType);

    }

    private void UnitActionSystem_OnSelectedActionChange(object sender , EventArgs e)
    {
        UpdateGridVisual();
    }

   private void LevelGrid_OnAnyUnitMovedPosition(object sender , EventArgs e)
   {
        UpdateGridVisual();
   }

   private Material GetGridVisualTypeMaterial(GridVisualType gridVisualType)
   {
        foreach (GridVisualTypeMaterial gridVisualTypeMaterial in gridVisualTypeMaterialList)
        {
            if(gridVisualTypeMaterial.gridVisualType == gridVisualType)
            {
                return gridVisualTypeMaterial.material;
            }
        }
        Debug.LogError("ColorMaterial not found " +  gridVisualType);
        return null;
   }

}
