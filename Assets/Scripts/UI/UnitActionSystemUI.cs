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
        TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChange;
        Unit.OnAnyActionPointChange += Unit_OnAnyActionPointChange;
        Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned; // New event to handle HP updates

        UpdateActionPoints();
        CreateUnitActionButtons();
        UpdateSelectedVisual();
        InitializeAPUI();
        InitializeHPUI(); // Initialize HP squares
    }

    private void CreateUnitActionButtons()
    {
        foreach (Transform buttonTransform in actionButtonContainerTransform)
        {
            Destroy(buttonTransform.gameObject);
        }

        actionButtonUIList.Clear();

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        foreach (BaseAction baseAction in selectedUnit.GetBaseActionArray())
        {
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
        InitializeAPUI(); // Re-initialize AP UI when unit changes
        InitializeHPUI(); // Re-initialize HP UI when unit changes
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
        actionPointText.text = "Action Points: " + selectedUnit.GetActionPoints().ToString();
        UpdateAPUI(); // Update the AP squares whenever AP text changes
    }

    private void TurnSystem_OnTurnChange(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }

    // Guarantees the update of action points
    private void Unit_OnAnyActionPointChange(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }

    // Guarantees the update of health points
    private void Unit_OnAnyUnitSpawned(object sender, EventArgs e)
    {
        UpdateHPUI(); // Update the health UI when a new unit is spawned
    }

    private void InitializeAPUI()
    {
        // Clear existing AP squares
        foreach (GameObject square in apSquares)
        {
            Destroy(square);
        }
        apSquares.Clear();

        // Create new AP squares based on current unit's action points
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        int maxAP = selectedUnit.MaxActionPoints; // Max AP

        for (int i = 0; i < maxAP; i++)
        {
            GameObject apSquare = Instantiate(fullAPSquare, apContainer);
            apSquares.Add(apSquare);
        }
    }

    private void UpdateAPUI()
    {
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        int currentAP = selectedUnit.GetActionPoints();

        // Update AP squares based on current action points
        for (int i = 0; i < apSquares.Count; i++)
        {
            Destroy(apSquares[i]); // Remove old square
            GameObject newSquare = Instantiate(i < currentAP ? fullAPSquare : emptyAPSquare, apContainer);
            apSquares[i] = newSquare;
        }
    }

    private void InitializeHPUI()
    {
        // Clear existing HP squares
        foreach (GameObject square in hpSquares)
        {
            Destroy(square);
        }
        hpSquares.Clear();

        // Create new HP squares based on current unit's health
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        int maxHP = selectedUnit.healthSystem.GetMaxHealth(); // Get max HP from the health system

        for (int i = 0; i < maxHP; i++)
        {
            GameObject hpSquare = Instantiate(fullHPSquare, hpContainer);
            hpSquares.Add(hpSquare);
        }
    }

    private void UpdateHPUI()
    {
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        int currentHP = selectedUnit.healthSystem.GetCurrentHealth(); // Get current health from health system

        // Update HP squares based on current health
        for (int i = 0; i < hpSquares.Count; i++)
        {
            Destroy(hpSquares[i]); // Remove old square
            GameObject newSquare = Instantiate(i < currentHP ? fullHPSquare : emptyHPSquare, hpContainer);
            hpSquares[i] = newSquare;
        }

        // Update HP text
        healthPointText.text = "Health Points: " + currentHP.ToString();
    }
}
