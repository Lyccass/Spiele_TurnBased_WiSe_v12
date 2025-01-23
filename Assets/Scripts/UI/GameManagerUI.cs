using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManagerUI : MonoBehaviour
{
    public static GameManagerUI Instance { get; private set; }
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameWonPanel;
    [SerializeField] private GameObject gamePausedPanel;


    private void Awake()
    {
        // Ensure only one instance exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicates
            return;
        }

        Instance = this; // Set the instance
    }

    void Start()
    {
         gameOverPanel.SetActive(false);
         gameWonPanel.SetActive(false);
         gamePausedPanel.SetActive(false);
    }

    void Update()
    {
        GameObject.Find("LivingEnemies").GetComponent<TextMeshProUGUI>().text = GameManager.Instance.enemies.ToString();
        GameObject.Find("DeadEnemies").GetComponent<TextMeshProUGUI>().text = GameManager.Instance.killedEnemies.ToString();
    }

    public void ShowGameOverScreen(string result)
    {
        if (result == "Win")
        {
            gameWonPanel.SetActive(true);
        }
        else
        {
            gameOverPanel.SetActive(true);
        }
    }

    public void ShowPausedScreen()
    {
        gamePausedPanel.SetActive(true);
    }
     public void HidePausedScreen()
    {
        gamePausedPanel.SetActive(false);
    }

    
}
