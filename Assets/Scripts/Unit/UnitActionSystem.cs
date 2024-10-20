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
   [SerializeField] private List<Unit> unitsList = new List<Unit>();
    private int currentUnitIndex = 0;  //use this for showing UI whose turn it is
    

   private void Awake()
   {
    if(Instance != null){
        Debug.LogError("Error, more than one UnitActionSystem");
        Destroy(gameObject);
        return;
    }
     Instance = this;
   }

    private void Start()
    {
        // Find all units in the scene and add them to the list
        unitsList.AddRange(FindObjectsOfType<Unit>());

        // Sort the list by speed in descending order (fastest to slowest)
         unitsList.Sort((unitA, unitB) => unitB.GetSpeed().CompareTo(unitA.GetSpeed()));
       
        if (unitsList.Count > 0)
        {
            SetSelectedUnit(unitsList[currentUnitIndex]);
        }
    }

   private void Update()
   {
        if (Input.GetMouseButtonDown(0))
        {
            
            if (selectedUnit != null)
            {
            Vector3 newTargetPosition = MouseWorld.GetPosition();
            //Debug.Log("Target position: " + newTargetPosition);  
            selectedUnit.Move(newTargetPosition);
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            SelectNextUnit(); // Move to the next unit's turn
        }
   }

   
 private void SetSelectedUnit(Unit unit)
   {
    selectedUnit = unit;
    OnSelectedUnitChange?.Invoke(this, EventArgs.Empty);
   }

public void SelectNextUnit()
    {
        currentUnitIndex++;

        // If we reach the end of the list, loop back to the start
        if (currentUnitIndex >= unitsList.Count)
        {
            currentUnitIndex = 0;
        }

        // Select the new unit
        SetSelectedUnit(unitsList[currentUnitIndex]);
    }

   public Unit GetSelectedUnit()
   {
        return selectedUnit;
   }

}
