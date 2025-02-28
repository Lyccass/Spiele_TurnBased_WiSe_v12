using UnityEngine;

public class HidePanelAfterTime : MonoBehaviour
{
    public GameObject uiPanel;        // The UI panel to hide
    public float hideDelay = 12f;     // Time in seconds before the panel becomes invisible

    private static bool panelIsDeactivated = false;  // Static variable to track the panel state

    private void Start()
    {
        // If the panel was previously deactivated, make sure it stays deactivated
        if (panelIsDeactivated)
        {
            uiPanel.SetActive(false);
        }
       /* else
        {
            // Set the panel to active and start the timer to deactivate it
            uiPanel.SetActive(true);
            Invoke(nameof(HidePanel), hideDelay);
        }
        */
    }

    /* private void HidePanel()
    {
        if (uiPanel != null)
        {
            uiPanel.SetActive(false);  // Deactivate the panel
            panelIsDeactivated = true; // Set the static variable to indicate panel is deactivated
        }
    */
}
