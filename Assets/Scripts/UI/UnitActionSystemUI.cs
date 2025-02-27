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
    [SerializeField] private TextMeshProUGUI healthPointText; // Added HP text
    [SerializeField] private GameObject fullAPSquare; // Added AP visual for UI
    [SerializeField] private GameObject emptyAPSquare;
    [SerializeField] private Transform apContainer; // Parent for AP squares

    [SerializeField] private GameObject fullHPSquare; // Added HP square visual
    [SerializeField] private GameObject emptyHPSquare;
    [SerializeField] private Transform hpContainer; // Parent for HP squares

    private List<ActionButtonUI> actionButtonUIList;
    private List<GameObject> apSquares = new List<GameObject>(); // AP squares list
    private List<GameObject> hpSquares = new List<GameObject>(); // HP squares list

    private void Awake()
    {
        actionButtonUIList = new List<ActionButtonUI>();
    }

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChange += UnitActionSystem_OnSelectedUnitChange;
        UnitActionSystem.Instance.OnSelectedActionChange += UnitActionSystem_OnSelectedActionChange;
        UnitActionSystem.Instance.OnActionStarted += UnitActionSystem_OnActionStarted;
        UnitActionUnlocker.OnUnlockNewAction += CreateUnitActionButtons;
        TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChange;
        Unit.OnAnyActionPointChange += Unit_OnAnyActionPointChange;
        Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned; // Handles HP updates when new units are spawned

        UpdateActionPoints();
        CreateUnitActionButtons();
        UpdateSelectedVisual();
        InitializeAPUI();
        InitializeHPUI();
    }

    private void OnDestroy()
    {
        // Ensure proper cleanup of event listeners
        UnitActionSystem.Instance.OnSelectedUnitChange -= UnitActionSystem_OnSelectedUnitChange;
        UnitActionSystem.Instance.OnSelectedActionChange -= UnitActionSystem_OnSelectedActionChange;
        UnitActionSystem.Instance.OnActionStarted -= UnitActionSystem_OnActionStarted;
        UnitActionUnlocker.OnUnlockNewAction -= CreateUnitActionButtons;
        TurnSystem.Instance.OnTurnChange -= TurnSystem_OnTurnChange;
        Unit.OnAnyActionPointChange -= Unit_OnAnyActionPointChange;
        Unit.OnAnyUnitSpawned -= Unit_OnAnyUnitSpawned;
    }

    private void CreateUnitActionButtons()
    {
        foreach (Transform buttonTransform in actionButtonContainerTransform)
        {
            Destroy(buttonTransform.gameObject);
        }

        actionButtonUIList.Clear();

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        if (selectedUnit == null) return;

        foreach (BaseAction baseAction in selectedUnit.GetBaseActionArray())
        {
            if (!baseAction.IsUnlocked()) continue; // Skip locked actions

            Transform actionButtonTransform = Instantiate(actionButtonPrefab, actionButtonContainerTransform);
            ActionButtonUI actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>();
            actionButtonUI.setBaseAction(baseAction);

            actionButtonUIList.Add(actionButtonUI);
        }
    }

    private void UnitActionSystem_OnSelectedUnitChange(object sender, EventArgs e)
    {
        CreateUnitActionButtons();
        UpdateSelectedVisual();
        UpdateActionPoints();
        InitializeAPUI();
        InitializeHPUI();
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
        foreach (ActionButtonUI actionButtonUI in actionButtonUIList)
        {
            actionButtonUI.UpdateSelectedVisual();
        }
    }

    private void UpdateActionPoints()
    {
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        if (selectedUnit == null) return;

        actionPointText.text = "Action Points: " + selectedUnit.GetActionPoints();
        UpdateAPUI();
    }

    private void TurnSystem_OnTurnChange(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }

    private void Unit_OnAnyActionPointChange(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }

    private void Unit_OnAnyUnitSpawned(object sender, EventArgs e)
    {
        UpdateHPUI(); // Update HP UI when a new unit is spawned
    }

    private void InitializeAPUI()
    {
        foreach (GameObject square in apSquares)
        {
            Destroy(square);
        }
        apSquares.Clear();

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        if (selectedUnit == null) return;

        int maxAP = selectedUnit.MaxActionPoints;
        for (int i = 0; i < maxAP; i++)
        {
            GameObject apSquare = Instantiate(fullAPSquare, apContainer);
            apSquares.Add(apSquare);
        }
    }

    private void UpdateAPUI()
    {
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        if (selectedUnit == null) return;

        int currentAP = selectedUnit.GetActionPoints();
        for (int i = 0; i < apSquares.Count; i++)
        {
            Destroy(apSquares[i]); // Remove old square
            GameObject newSquare = Instantiate(i < currentAP ? fullAPSquare : emptyAPSquare, apContainer);
            apSquares[i] = newSquare;
        }
    }

    private void InitializeHPUI()
    {
        foreach (GameObject square in hpSquares)
        {
            Destroy(square);
        }
        hpSquares.Clear();

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        if (selectedUnit == null) return;

        int maxHP = selectedUnit.healthSystem.GetMaxHealth();
        for (int i = 0; i < maxHP; i++)
        {
            GameObject hpSquare = Instantiate(fullHPSquare, hpContainer);
            hpSquares.Add(hpSquare);
        }
    }

    private void UpdateHPUI()
    {
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        if (selectedUnit == null) return;

        int currentHP = selectedUnit.healthSystem.GetCurrentHealth();
        for (int i = 0; i < hpSquares.Count; i++)
        {
            Destroy(hpSquares[i]); // Remove old square
            GameObject newSquare = Instantiate(i < currentHP ? fullHPSquare : emptyHPSquare, hpContainer);
            hpSquares[i] = newSquare;
        }

        healthPointText.text = "Health Points: " + currentHP.ToString();
    }
}
