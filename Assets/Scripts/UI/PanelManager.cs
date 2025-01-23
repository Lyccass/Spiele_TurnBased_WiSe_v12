using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
     public void ShowPanel(GameObject panel)
    {
        panel.SetActive(true);
    }

    
    public void TogglePanel(GameObject panel)
    {
        panel.SetActive(false);  
    }
    
}

