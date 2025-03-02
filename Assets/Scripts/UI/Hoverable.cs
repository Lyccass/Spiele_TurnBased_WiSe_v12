using UnityEngine;
using System.Collections;

public class Hoverable : MonoBehaviour
{
    [SerializeField] private string tooltipText;
    private Coroutine hoverCoroutine;

    private void OnMouseEnter()
    {
        // Start the delayed hover coroutine
        hoverCoroutine = StartCoroutine(DelayedHover());
    }

    private void OnMouseExit()
    {
        // If the coroutine is running, cancel it
        if (hoverCoroutine != null)
        {
            StopCoroutine(hoverCoroutine);
            hoverCoroutine = null;
        }
        Tooltip.Instance.StopHover();
    }

    private IEnumerator DelayedHover()
    {
        // Wait for 1 second before activating tooltip and sound
        yield return new WaitForSeconds(0.5f);
          Tooltip.Instance.StartHover(tooltipText);
        AudioManager.Instance.PlayUI("hover2");
        
    }
}
