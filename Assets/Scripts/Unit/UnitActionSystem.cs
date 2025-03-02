using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }

    public event EventHandler OnSelectedUnitChange;
    public event EventHandler OnSelectedActionChange;
    public event EventHandler<bool> OnBusyChange;
    public event EventHandler OnActionStarted;

    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitsLayerMask;

    private BaseAction selectedAction;
    public bool isBusy;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Error, more than one UnitActionSystem");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        // Only set a unit at start if one is assigned in the inspector.
        if (selectedUnit != null)
        {
            SetSelectedUnit(selectedUnit);
        }
    }

    private void Update()
    {
        if (GameManager.Instance.GetCurrentGameState() == GameState.GameOver)
        {
            return;
        }

        if (isBusy)
        {
            return;
        }
        if (!TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (TryHandleUnitSelection())
        {
            return;
        }

        // Only handle action if we have a selected unit.
        if (selectedUnit != null)
        {
            HandleSelectedAction();
        }
    }

    private void HandleSelectedAction()
    {
        // Make sure we have a valid selected action.
        if (selectedAction == null)
        {
            return;
        }

        if (InputManager.Instance.IsMouseButtonDown())
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

            if (!selectedAction.IsValidActionGridPosition(mouseGridPosition))
            {
                return;
            }

            // Check if action points can be spent.
            if (!selectedUnit.TrySpendActionPointsToTakeAction(selectedAction))
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
        if (InputManager.Instance.IsMouseButtonDown())
        {
            Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseScreenPosition());
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitsLayerMask))
            {
                if (raycastHit.transform.TryGetComponent<Unit>(out Unit unit))
                {
                    // Deselect if the unit is already selected.
                    if (unit == selectedUnit)
                    {
                        return false;
                    }
                    // Optionally, you can also decide not to select enemy units.
                    if (unit.IsEnemy())
                    {
                        return false;
                    }
                    SetSelectedUnit(unit);
                    return true;
                }
            }
        }
        return false;
    }

    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        if (selectedUnit != null)
        {
            // Automatically set a default action, for example MoveAction.
            SetSelecterdAction(unit.GetAction<MoveAction>());
        }
        else
        {
            // Clear the action if no unit is selected.
            selectedAction = null;
            OnSelectedActionChange?.Invoke(this, EventArgs.Empty);
        }
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

    public bool IsBusy()
    {
        return isBusy;
    }
}
