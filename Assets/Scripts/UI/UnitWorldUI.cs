using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Runtime.CompilerServices;
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
        Unit.OnAnyActionPointChange += Unit_OnAnyActionPointChange;
        healthSystem.OnDamaged +=  HealthSystem_OnDamaged;
        UpdateActionPointText();
        UpdateHealthBar();
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

    private void HealthSystem_OnDamaged(object sender, EventArgs e)
    {
        UpdateHealthBar();
    }
}


