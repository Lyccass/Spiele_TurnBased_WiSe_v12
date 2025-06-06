using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{
  [SerializeField] private TextMeshProUGUI textMeshPro;
  [SerializeField] private Button button;
  [SerializeField] private GameObject selectedGameObject;

  private BaseAction baseAction;

  public void setBaseAction(BaseAction baseAction)
  {
    this.baseAction = baseAction;
    textMeshPro.text = baseAction.GetActionName();

    button.onClick.AddListener(() => {
      UnitActionSystem.Instance.SetSelecterdAction(baseAction);
    });
  }

public void UpdateSelectedVisual()
{
  BaseAction selectedBaseAction = UnitActionSystem.Instance.GetSelectedAction();
  selectedGameObject.SetActive(selectedBaseAction == baseAction);
}

}
