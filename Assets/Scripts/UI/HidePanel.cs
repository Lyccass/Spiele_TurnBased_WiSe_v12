using UnityEngine;

public class HidePanelAfterTime : MonoBehaviour
{
    public GameObject uiPanel;        // Das UI-Panel, das ausgeblendet werden soll
    public float hideDelay = 12f;    // Zeit in Sekunden, bis das Panel unsichtbar wird

    private void Start()
    {
        if (uiPanel != null)
        {
            Invoke(nameof(HidePanel), hideDelay); // Blendet das Panel nach der angegebenen Zeit aus
        }
        else
        {
            Debug.LogWarning("UI-Panel ist nicht zugewiesen!");
        }
    }

    private void HidePanel()
    {
        if (uiPanel != null)
        {
            uiPanel.SetActive(false); // Deaktiviert das Panel
        }
    }
}
