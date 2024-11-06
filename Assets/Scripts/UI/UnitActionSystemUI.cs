using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainerTransform;
    [SerializeField] private TextMeshProUGUI actionPointText;
    private List <ActionButtonUI> actionButtonUIList;

    private void Awake()
    {
        actionButtonUIList = new List<ActionButtonUI>();
    }

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChange += UnitActionSystem_OnSelectedUnitChange;
        UnitActionSystem.Instance.OnSelectedActionChange += UnitActionSystem_OnSelectedActionChange;
        UnitActionSystem.Instance.OnActionStarted += UnitActionSystem_OnActionStarted;
        TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChange;
        Unit.OnAnyActionPointChange += Unit_OnAnyActionPointChange;
        UpdateActionPoints();
        CreatUnitActionButtons();
        UpdateSelectedVisual();
    }


   private void CreatUnitActionButtons()
   {
    foreach (Transform buttonTransform in actionButtonContainerTransform)
    {
        Destroy(buttonTransform.gameObject);
    }

    actionButtonUIList.Clear();

    Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
    foreach (BaseAction baseAction in selectedUnit.GetBaseActionArray())
    {
        Transform actionButtonTransform = Instantiate(actionButtonPrefab,actionButtonContainerTransform);
        ActionButtonUI actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>();
        actionButtonUI.setBaseAction(baseAction);

        actionButtonUIList.Add(actionButtonUI);
    }
   }

   private void UnitActionSystem_OnSelectedUnitChange(object sender, EventArgs e)
   {
    CreatUnitActionButtons();
    UpdateSelectedVisual();
    UpdateActionPoints();
   }
    private void UnitActionSystem_OnSelectedActionChange(object sender, EventArgs e)
   {
    UpdateSelectedVisual();
   }

   private void UnitActionSystem_OnActionStarted(object sender, EventArgs e)
   {
    UpdateActionPoints();
   }


   private void UpdateSelectedVisual()
   {
        foreach(ActionButtonUI actionButtonUI in actionButtonUIList)
        {
            actionButtonUI.UpdateSelectedVisual();
        }
   }

    private void UpdateActionPoints()
   {
    Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
    actionPointText.text = "Action Points: " + selectedUnit.GetActionPoints();;
   }

private void TurnSystem_OnTurnChange(object sender, EventArgs e)
{
    UpdateActionPoints();
}
//guarentees the update of action points
private void Unit_OnAnyActionPointChange(object sender, EventArgs e)
{
      UpdateActionPoints();
}

}
