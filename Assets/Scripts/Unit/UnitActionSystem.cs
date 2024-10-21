using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance{get; private set;}
    public event EventHandler OnSelectedUnitChange;
   [SerializeField] private Unit selectedUnit;
   [SerializeField] private LayerMask unitsLayerMask;


   private void Awake()
   {
    if(Instance != null){
        Debug.LogError("Error, more than one UnitActionSystem");
        Destroy(gameObject);
        return;
    }
     Instance = this;
   }

   private void Update()
   {
        if (Input.GetMouseButtonDown(0))
        {
            if(TryHandleUnitSelection()) return;
           
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

            if(selectedUnit.GetMoveAction().IsValidActionGridPosition(mouseGridPosition))
            {
                selectedUnit.GetMoveAction().Move(mouseGridPosition);
            }

        }
   }

   private bool TryHandleUnitSelection()
   {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      if(  Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitsLayerMask))
      {
            if (raycastHit.transform.TryGetComponent<Unit>(out Unit unit))
            {
                SetSelectedUnit(unit);  // Use SetSelectedUnit to trigger the visual update
                return true;
            }

      };
      return false;
   }

   private void SetSelectedUnit(Unit unit)
   {
    selectedUnit = unit;
    OnSelectedUnitChange?.Invoke(this, EventArgs.Empty);
   }

   public Unit GetSelectedUnit()
   {
        return selectedUnit;
   }

}
