using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class TutorialPopupUI : MonoBehaviour
{
    public static TutorialPopupUI Instance { get; private set; }

    [SerializeField] private GameObject popupPanel; // The UI panel for the tutorial
    [SerializeField] private TextMeshProUGUI popupText; // The text field inside the chatbox
    [SerializeField] private GameObject inputBlocker; // NEW: A transparent UI element that blocks clicks

    private List<string> currentMessages;
    private int currentMessageIndex;
    private bool isActive = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        popupPanel.SetActive(false); // Hide UI initially
        inputBlocker.SetActive(false); // Hide input blocker initially
    }

    private void Update()
    {
        if (isActive && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            ShowNextMessage();
        }
    }

    public void ShowPopup(List<string> messages)
    {
        if (messages == null || messages.Count == 0) return;

        currentMessages = messages;
        currentMessageIndex = 0;

        popupPanel.SetActive(true);
        inputBlocker.SetActive(true); // Blocks mouse input
        isActive = true;

        ShowNextMessage();
    }

private void ShowNextMessage()
{
    if (currentMessageIndex < currentMessages.Count)
    {
        Debug.Log("[TutorialPopupUI] About to play 'text' UI sound");
        AudioManager.Instance.PlayUI("text");
        popupText.text = currentMessages[currentMessageIndex];
        currentMessageIndex++;
    }
    else
    {
        HidePopup();
    }
}

    public void HidePopup()
    {
        popupPanel.SetActive(false);
        inputBlocker.SetActive(false); // Re-enable mouse interactions
        isActive = false;
    }

    public bool IsPopupActive()
{
    return isActive;
}

}
