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

private void Start() 
{ 
  endTurnButton.onClick.AddListener(() =>
  {
    TurnSystem.Instance.NextTurn();
  });

  TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChange;
  UpdateTurnText();
}


private void TurnSystem_OnTurnChange(object sender, EventArgs e)
{
  UpdateTurnText();
}
private void UpdateTurnText()

{
  activeTurn.text = "Active Turn: " + TurnSystem.Instance.GetTurnNumber();
}

}
