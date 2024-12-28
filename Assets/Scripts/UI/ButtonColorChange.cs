using UnityEngine;
using TMPro; // Importiere TextMeshPro
using UnityEngine.EventSystems;

public class ButtonColorChangeTMP : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI buttonText; // Referenz zum TextMeshPro-Text
    public Color hoverColor = Color.red; // Farbe beim Hovern
    public Color normalColor = Color.black; // Standardfarbe

    private void Start()
    {
        // Standardfarbe beim Start setzen
        if (buttonText != null)
        {
            buttonText.color = normalColor;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Schriftfarbe ändern, wenn der Mauszeiger über dem Button ist
        if (buttonText != null)
        {
            buttonText.color = hoverColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Schriftfarbe zurücksetzen, wenn der Mauszeiger den Button verlässt
        if (buttonText != null)
        {
            buttonText.color = normalColor;
        }
    }
}
