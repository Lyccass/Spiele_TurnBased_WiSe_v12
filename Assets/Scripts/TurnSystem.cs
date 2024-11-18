using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    public static TurnSystem Instance{get; private set;}
    private int turnNumber = 1;
    public event EventHandler OnTurnChange;
    private bool isPlayerTurn = true;
    


   private void Awake()
   {
    if(Instance != null){
        Debug.LogError("Error, more than one TurnSystem!" + transform + " - " + Instance);
        Destroy(gameObject);
        return;
    }
     Instance = this;
   }


public void NextTurn()
    {
        turnNumber++;
        isPlayerTurn = !isPlayerTurn;
        OnTurnChange?.Invoke(this, EventArgs.Empty);
    }


public int GetTurnNumber()
{
    return turnNumber;
}

public bool IsPlayerTurn()
{
    return isPlayerTurn;
}
}
