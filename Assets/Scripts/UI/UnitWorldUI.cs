using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UnitWorldUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI actionPointsText;
    [SerializeField] private Unit unit;
    [SerializeField] private Image healthbarImage;
    [SerializeField] private HealthSystem healthSystem;

    private void Start()
    {
        // Subscribe to events
        Unit.OnAnyActionPointChange += Unit_OnAnyActionPointChange;
        healthSystem.OnDamaged += HealthSystem_OnHealthChanged;
        healthSystem.OnHealed += HealthSystem_OnHealthChanged;

        // Initialize UI
        UpdateActionPointText();
        UpdateHealthBar();
    }

    private void OnDestroy()
    {
        // Unsubscribe from events to prevent memory leaks
        Unit.OnAnyActionPointChange -= Unit_OnAnyActionPointChange;
        healthSystem.OnDamaged -= HealthSystem_OnHealthChanged;
        healthSystem.OnHealed -= HealthSystem_OnHealthChanged;
    }

    private void UpdateActionPointText()
    {
        actionPointsText.text = unit.GetActionPoints().ToString() + " AP";
    }

    private void Unit_OnAnyActionPointChange(object sender, EventArgs e)
    {
        UpdateActionPointText();
    }

    private void UpdateHealthBar()
    {
        healthbarImage.fillAmount = healthSystem.GetHealthNormalized();
    }

    private void HealthSystem_OnHealthChanged(object sender, EventArgs e)
    {
        UpdateHealthBar();
    }
}
