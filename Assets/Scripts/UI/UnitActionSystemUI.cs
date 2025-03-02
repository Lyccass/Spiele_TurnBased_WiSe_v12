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
    [SerializeField] private GameObject RangerIcon;
    [SerializeField] private GameObject MeleeIcon;
    [SerializeField] private GameObject HealerIcon;
    [SerializeField] private GameObject EyeIcon;

    [SerializeField] private CanvasGroup uiCanvasGroup;

    
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
        UpdateHealthPoints();
        InitializeAPUI();
        UpdateAPUI();
        InitializeHPUI();
        UpdateHPUI();
        UpdateUnitIcon();


        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        if (selectedUnit != null)
        {
            selectedUnit.healthSystem.OnDamaged += HealthSystem_OnChanged;
            selectedUnit.healthSystem.OnHealed += HealthSystem_OnChanged;

        }
       
    }

    private void UnitActionSystem_OnSelectedActionChange(object sender, EventArgs e)
    {
        UpdateSelectedVisual();
    }

    private void UnitActionSystem_OnActionStarted(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }
    private void TurnSystem_OnTurnChange(object sender, EventArgs e)
    {   
        UpdateActionPoints();
        UpdateHealthPoints();
        UpdateHPUI();
        UpdateUnitIcon();
    }

    private void Unit_OnAnyActionPointChange(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }

    private void HealthSystem_OnChanged(object sender, EventArgs e)
    {
    UpdateHPUI();
    UpdateHealthPoints();
    }

    private void Unit_OnAnyUnitSpawned(object sender, EventArgs e)
    {
        InitializeHPUI();
        InitializeAPUI();
        UpdateActionPoints();
        UpdateHealthPoints();
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

    private void UpdateHealthPoints()
    {
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        if (selectedUnit == null) return;

        healthPointText.text = "Health Points: " + selectedUnit.healthSystem.GetCurrentHealth();
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

    }

    private void UpdateUnitIcon()
    {   
        MeleeIcon.SetActive(false);
        HealerIcon.SetActive(false);
        RangerIcon.SetActive(false);
        EyeIcon.SetActive(false);
       
        
        // Get the selected unit
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        if (selectedUnit == null) return;

        // Compare the unit's GameObject name
        string unitName = selectedUnit.gameObject.name;

        if (!TurnSystem.Instance.IsPlayerTurn())
        {
            EyeIcon.SetActive(true);
        }

        else if (unitName.Contains("Healer"))
        {
            HealerIcon.SetActive(true);
        }
        else if (unitName.Contains("Melee"))
        {
            MeleeIcon.SetActive(true);
        }
        else if (unitName.Contains("Ranger"))
        {
            RangerIcon.SetActive(true);
        }
        
    }

    public void DisableUI(bool disable)
    {
        if (disable)
        {
            // Set the CanvasGroup alpha to 0 and disable interaction
            uiCanvasGroup.alpha = 0;
            uiCanvasGroup.interactable = false;
            uiCanvasGroup.blocksRaycasts = false;
        }
        else
        {
            // Set the CanvasGroup alpha to 1 and re-enable interaction
            uiCanvasGroup.alpha = 1;
            uiCanvasGroup.interactable = true;
            uiCanvasGroup.blocksRaycasts = true;
        }
    }

}
