using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{
  [SerializeField] private TextMeshProUGUI textMeshPro;
  [SerializeField] private Button button;

  public void setBaseAction(BaseAction baseAction)
  {
    textMeshPro.text = baseAction.GetActionName();
  }


}
