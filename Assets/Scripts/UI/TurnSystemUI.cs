using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TurnSystemUI : MonoBehaviour
{
  [SerializeField]  private Button endTurnButton;
  [SerializeField] private TextMeshProUGUI activeTurn;
  [SerializeField] private GameObject enemyTurnVisual;
 

private void Start() 
{ 
  endTurnButton.onClick.AddListener(() =>
  {
    TurnSystem.Instance.NextTurn();
  });

  TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChange;
  UpdateTurnText();
  UpdateEnemyTurnVisual();
  UpdatedEndTurnButton();
}


private void TurnSystem_OnTurnChange(object sender, EventArgs e)
{
  UpdateTurnText();
  UpdateEnemyTurnVisual();
  UpdatedEndTurnButton();
}
private void UpdateTurnText()

{
  int turnNumber = TurnSystem.Instance.GetTurnNumber();
  int playerTurn = (turnNumber + 1) / 2; // Divide by 2 and add 1 to get the desired result
  activeTurn.text = "Turn: " + playerTurn;
}

public void UpdateEnemyTurnVisual()
{
    if (enemyTurnVisual == null)
    {
        Debug.LogError("EnemyTurnVisual is not assigned!");
        return;
    }

    //s
    bool shouldShowEnemyUI = !TurnSystem.Instance.IsPlayerTurn() && !UnitActionSystem.Instance.IsBusy();
    
    enemyTurnVisual.SetActive(shouldShowEnemyUI);
}


private void UpdatedEndTurnButton()
{
  endTurnButton.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());
}

}

