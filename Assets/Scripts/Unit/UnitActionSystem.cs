using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance{get; private set;}

    public event EventHandler OnSelectedUnitChange;
    public event EventHandler OnSelectedActionChange;
    public event EventHandler<bool> OnBusyChange;
    public event EventHandler OnActionStarted;



   [SerializeField] private Unit selectedUnit;
   [SerializeField] private LayerMask unitsLayerMask;

   private BaseAction selectedAction;
    private bool isBusy;    


   private void Awake()
   {
    if(Instance != null){
        Debug.LogError("Error, more than one UnitActionSystem");
        Destroy(gameObject);
        return;
    }
     Instance = this;
   }

   private void Start(){
    SetSelectedUnit(selectedUnit);
   }

   private void Update()
   {

        if(isBusy)
        {
            return;
        }

        if(EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if(TryHandleUnitSelection())
        {
            return;
        }
        HandleSelectedAction();

 
   }

private void HandleSelectedAction()
{
    if (Input.GetMouseButtonDown(0))
    {
        GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

        if(!selectedAction.IsValidActionGridPosition(mouseGridPosition))
        {
            return;
        }

        // Check if action points can be spent
        if(!selectedUnit.TrySpendActionPointsToTakeAction(selectedAction))
        {
            Debug.Log("Not enough action points!");
            return;
        }

        SetBusy();
        selectedAction.TakeAction(mouseGridPosition, ClearBusy);
        OnActionStarted?.Invoke(this, EventArgs.Empty);
    }
}


    private void SetBusy()
    {
        isBusy = true;
        OnBusyChange?.Invoke(this, isBusy);
    }
    private void ClearBusy()
    {
        isBusy = false;
        OnBusyChange?.Invoke(this, isBusy);
    }

   private bool TryHandleUnitSelection()
   {
    if(Input.GetMouseButtonDown(0)){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(  Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitsLayerMask))
      {
            if (raycastHit.transform.TryGetComponent<Unit>(out Unit unit))
            {
                if(unit == selectedUnit)
                {
                    return false;
                }
                SetSelectedUnit(unit);  // Use SetSelectedUnit to trigger the visual update
                return true;
            }
      }

      };

      return false;
   }

   private void SetSelectedUnit(Unit unit)
   {
    selectedUnit = unit;
    SetSelecterdAction(unit.GetMoveAction());
    OnSelectedUnitChange?.Invoke(this, EventArgs.Empty);
   }

   public void SetSelecterdAction(BaseAction baseAction)
   {
        selectedAction = baseAction;
        OnSelectedActionChange?.Invoke(this, EventArgs.Empty);
   }

   public Unit GetSelectedUnit()
   {
        return selectedUnit;
   }

   public BaseAction GetSelectedAction()
   {
    return selectedAction;
   }

}
