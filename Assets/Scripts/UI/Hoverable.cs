using UnityEngine;

public class Hoverable : MonoBehaviour
{
    [SerializeField] private string tooltipText;

    private void OnMouseEnter()
    {
        Tooltip.Instance.StartHover(tooltipText);
    }

    private void OnMouseExit()
    {
        Tooltip.Instance.StopHover();
    }
}
