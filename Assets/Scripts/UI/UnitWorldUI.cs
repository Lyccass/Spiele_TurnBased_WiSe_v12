using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UnitWorldUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI actionPointsText;
    [SerializeField] private TextMeshProUGUI healthText; 
    [SerializeField] private Unit unit;
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private GameObject fullHPSquare;
    [SerializeField] private GameObject emptyHPSquare;
    [SerializeField] private Transform hpContainer;
    [SerializeField] private GameObject fullAPSquare; 
    [SerializeField] private GameObject emptyAPSquare;
    [SerializeField] private Transform apContainer;

    private List<GameObject> hpSquares = new List<GameObject>();
    private List<GameObject> apSquares = new List<GameObject>(); 

    private void Start()
    {
        // Subscribe to events
        Unit.OnAnyActionPointChange += Unit_OnAnyActionPointChange;
        healthSystem.OnDamaged += HealthSystem_OnHealthChanged;
        healthSystem.OnHealed += HealthSystem_OnHealthChanged;

        // Initialize UI
        InitializeHPUI();
        InitializeAPUI();
        UpdateActionPointText();
        UpdateHPText();
    }

    private void OnDestroy()
    {
        // Unsubscribe from events
        Unit.OnAnyActionPointChange -= Unit_OnAnyActionPointChange;
        healthSystem.OnDamaged -= HealthSystem_OnHealthChanged;
        healthSystem.OnHealed -= HealthSystem_OnHealthChanged;
    }



       private void Unit_OnAnyActionPointChange(object sender, EventArgs e)
    {
        UpdateActionPointText();
        UpdateAPUI();
    }

    private void HealthSystem_OnHealthChanged(object sender, EventArgs e)
    {
        UpdateHPUI();
        UpdateHPText(); 
    }

    private void UpdateActionPointText()
    {
        actionPointsText.text = unit.GetActionPoints().ToString() + " AP";
    }

    private void UpdateHPText()
    {
        healthText.text = healthSystem.GetCurrentHealth().ToString() + " HP"; // New function
    }


    private void InitializeHPUI()
    {
        // Clear existing squares
        foreach (GameObject square in hpSquares)
        {
            Destroy(square);
        }
        hpSquares.Clear();

        // Create new squares
        for (int i = 0; i < healthSystem.GetMaxHealth(); i++)
        {
            GameObject hpSquare = Instantiate(fullHPSquare, hpContainer);
            hpSquares.Add(hpSquare);
        }
    }

    private void UpdateHPUI()
    {
        int currentHP = healthSystem.GetCurrentHealth();

        for (int i = 0; i < hpSquares.Count; i++)
        {
            Destroy(hpSquares[i]); // Remove old
            GameObject newSquare = Instantiate(i < currentHP ? fullHPSquare : emptyHPSquare, hpContainer);
            hpSquares[i] = newSquare;
        }
    }

    private void InitializeAPUI()
    {
        // Clear existing AP squares
        foreach (GameObject square in apSquares)
        {
            Destroy(square);
        }
        apSquares.Clear();

        // Create new AP squares
        for (int i = 0; i < unit.GetActionPoints(); i++)
        {
            GameObject apSquare = Instantiate(fullAPSquare, apContainer);
            apSquares.Add(apSquare);
        }
    }

    private void UpdateAPUI()
    {
        int currentAP = unit.GetActionPoints();

        for (int i = 0; i < apSquares.Count; i++)
        {
            Destroy(apSquares[i]); // Remove old
            GameObject newSquare = Instantiate(i < currentAP ? fullAPSquare : emptyAPSquare, apContainer);
            apSquares[i] = newSquare;
        }
    }
}
