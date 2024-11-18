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
  activeTurn.text = "Active Turn: " + TurnSystem.Instance.GetTurnNumber();
}

private void UpdateEnemyTurnVisual()
{
  enemyTurnVisual.SetActive(!TurnSystem.Instance.IsPlayerTurn());
}

private void UpdatedEndTurnButton()
{
  endTurnButton.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());
}

}
